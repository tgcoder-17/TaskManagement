
using Serilog;

namespace TaskManagement.API.Extensions
{
    public static class LoggerExtensions
    {
        public static IHostBuilder UseCustomSerilog(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog((context, services, loggerConfig) =>
            {
                var configuration = context.Configuration;

                var logFolder = configuration["Logging:LogFolder"] ?? "Logs";
                var logFile = configuration["Logging:LogFile"] ?? "log-.txt";

                var logPath = Path.Combine(
                    AppContext.BaseDirectory,
                    logFolder,
                    logFile);

                Directory.CreateDirectory(
                    Path.GetDirectoryName(logPath)!);

                loggerConfig
                    .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.File(
                        path: logPath,
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 7);
            });
        }
    }

}
