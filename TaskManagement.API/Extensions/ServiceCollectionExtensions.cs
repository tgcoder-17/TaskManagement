using TaskManagement.API.Helpers;
using TaskManagement.API.Repositories.Implementations;
using TaskManagement.API.Repositories.Interfaces;
using TaskManagement.API.Services.Implementations;
using TaskManagement.API.Services.Interfaces;

namespace TaskManagement.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<JwtTokenService>();

            return services;
        }
    }
}
