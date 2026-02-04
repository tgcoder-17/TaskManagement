using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskManagement.API.Data;
using TaskManagement.API.Extensions;
using TaskManagement.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseCustomSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMemoryCache();

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddSwaggerDocumentation()
    .AddCustomValidation();

// EF Core  
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseGlobalExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
