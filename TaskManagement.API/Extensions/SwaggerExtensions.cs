using Microsoft.OpenApi;

namespace TaskManagement.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(
            this IServiceCollection services)
        {
            services.AddSwaggerGen();

            return services;
        }
    }
}
