using System.Text.Json;
using HardwareTech.Application.Exceptions;

namespace HardwareTech.API.Middlewares
{
    public class ExceptionMiddleware //Separamos las responsabilidades, ya que el servicio no sabe ni debe saber que codigo corresponde
    {
        private readonly RequestDelegate _next; //Representa el siguiente paso del pipeline

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next; //ASP.NET inyecta automaticamente el siguiente paso
        }

        public async Task InvokeAsync(HttpContext context) //El metodo qye ASP.NET ejecuta en cada request
        {
            try
            {
                await _next(context); //Continua al siguiente paso (Controlador)
            }
            catch (Exception exception) //Captura cualquier excepcion
            {
                await HandleExceptionAsync( //De capturar una excepcion, ejecuta el siguiente metodo
                    context,
                    exception);
            }
        }

        private static async Task HandleExceptionAsync(  //Metodo que se ejecuta al capturar una excepcion
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType =
                "application/json";

            int statusCode = exception switch //Este switch, es el que, dependiendo la excepcion que se capture,
                                              //va a ejecutar las excepciones personalizadas que definimos antes.
            {
                NotFoundException =>
                    StatusCodes.Status404NotFound, //404

                ValidationException =>
                    StatusCodes.Status400BadRequest, //400

                BusinessException =>
                    StatusCodes.Status409Conflict, //409

                _ =>
                    StatusCodes.Status500InternalServerError //500 El _ significa, en cualquier otro caso...
            };

            context.Response.StatusCode = statusCode; //Construye la respuesta http con el codigo correspondiente en cada caso

            var response = new //creamos un objeto anonimo, ya que se usa una sola vez, podriamos sino hacer un dto ErrorResponse,
                               //pero los dto los hizo alejo asi que si esta mal es culpa de el porque no se le ocurrio esto... xd
            {
                StatusCode = statusCode,
                Message = exception.Message
            };

            var json =
                JsonSerializer.Serialize(response); //Convertimos el objeto a JSON para que ahora si, pueda viajar por http

            await context.Response.WriteAsync(json); //Escribe el json dentro del body
        }
    }
}
