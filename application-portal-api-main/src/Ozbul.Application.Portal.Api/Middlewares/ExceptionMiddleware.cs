using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Ozbul.Application.Portal.Api.Models;

namespace Ozbul.Application.Portal.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException validationException)
            {
                _logger.LogError($"Validation exception occured: {validationException.Message}", validationException);

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new ErrorViewModel(validationException.Message));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something went wrong, encountered exception {exception}", exception);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new ErrorViewModel("Internal Server Error"));
            }
        }
    }
}