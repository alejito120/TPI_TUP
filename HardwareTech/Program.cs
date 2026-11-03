using HardwareTech.API.Middlewares;
using HardwareTech.Infraestructure.Extensions;
using HardwareTech.Application.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HardwareTech.Infraestructure.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRepositories(); //Registramos los repositorios de la extension
builder.Services.AddApplicationServices(); //Registramos los servicios de la extension

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>(); //Queremos que el middleware envuelva los controladores.
app.MapControllers();
app.Run();

