using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Events;
using TaskManagementService.API.Extensions;

namespace TaskManagementService.API
{
    /// <summary>
    /// Точка входа в приложение
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        /// <param name="args">Аргументы командной строки</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Настройка Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/taskmanagement-.txt", 
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            builder.Host.UseSerilog();

            // Настройка OpenTelemetry
            builder.Services.AddOpenTelemetry()
                .WithTracing(tracerProviderBuilder => tracerProviderBuilder
                    .AddSource("TaskManagementService")
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService("TaskManagementService")
                            .AddTelemetrySdk()
                            .AddEnvironmentVariableDetector())
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddConsoleExporter()
                    .AddJaegerExporter(options =>
                    {
                        var jaegerHost = builder.Configuration.GetValue<string>("Jaeger:Host");
                        var jaegerPort = builder.Configuration.GetValue<int>("Jaeger:Port");
                        options.AgentHost = jaegerHost ?? "localhost";
                        options.AgentPort = jaegerPort > 0 ? jaegerPort : 6831;
                    }));

            // Добавление сервисов в контейнер
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add application services
            builder.Services.AddApplicationServices(builder.Configuration);

            var app = builder.Build();

            // Настройка конвейера HTTP-запросов
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRequestLogging();
            app.UseAuthorization();
            app.MapControllers();

            try
            {
                Log.Information("Запуск приложения Task Management Service");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Приложение неожиданно остановлено");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
} 