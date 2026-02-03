using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Common;

namespace TaskManagement.API.Extensions
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddCustomValidation(
            this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var response = new ErrorResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Validation failed",
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
    }
}
