using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MiniDropBox.Application.Interfaces.UnitOfWork;
using System.Net;
using System.Text.Json;

namespace MiniDropBox.Infraestructure.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            try
            {
                // Start transaction
                await unitOfWork.BeginTransactionAsync();

                // Execute next middleware/controller
                await _next(context);

                // Auto-commit transaction if no exception occurred
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback transaction if an exception occurred
                await unitOfWork.RollbackAsync();
                _logger.LogError(ex, "An unexpected error ocurred.");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Something went wrong. Please try again later."
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
