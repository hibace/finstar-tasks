using TaskManagementService.API.Middleware;

namespace TaskManagementService.API.Extensions
{
    /// <summary>
    /// Расширения для IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Добавляет промежуточное ПО для логирования запросов
        /// </summary>
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
} 