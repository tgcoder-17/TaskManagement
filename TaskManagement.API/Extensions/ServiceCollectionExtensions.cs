using TaskManagement.API.Helpers;
using TaskManagement.API.Mappings;
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
            // AutoMapper
            var mapper = AutoMapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<JwtTokenService>();

            return services;
        }
    }
}
