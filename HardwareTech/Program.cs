using System.Text;
using HardwareTech.API.Middlewares;
using HardwareTech.Application.Extensions;
using HardwareTech.Application.Interfaces;
using HardwareTech.Infraestructure.Data;
using HardwareTech.Infraestructure.Extensions;
using HardwareTech.Infraestructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// 1. Base de datos con reintentos para la nube (Paso 2.3 de la guía de deploy)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddHttpClient<IDolarApiService, DolarApiService>();

// 2. Configuración de Autenticación con JWT Bearer
var jwtSecret = builder.Configuration["JwtSettings:Secret"]
    ?? throw new InvalidOperationException("La clave 'JwtSettings:Secret' no está configurada.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = System.Security.Claims.ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 3. Configuración de Swagger con botón "Authorize" compatible con Swashbuckle v10 y OpenAPI 2.0
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HardwareTech API",
        Version = "v1",
        Description = "API de gestión con autenticación JWT."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Ingresá únicamente el token JWT obtenido del login."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

// 4. Migración automática y Data Seeding en la nube protegido con try/catch
// para evitar que un fallo inicial de BD cause el error HTTP 500.30
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
    await DbInitializer.SeedAsync(app.Services);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Aviso: No se pudo conectar a la base de datos o ejecutar migraciones al iniciar la app.");
}

// 5. Middleware global de excepciones
app.UseMiddleware<ExceptionMiddleware>();

// 6. Swagger habilitado en todos los entornos (Paso 2.1 de la guía para Azure)
app.UseSwagger();
app.UseSwaggerUI();

// Redirección de la raíz a /swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.UseHttpsRedirection();

// 7. Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
