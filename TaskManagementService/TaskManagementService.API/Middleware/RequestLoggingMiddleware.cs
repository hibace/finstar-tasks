using System.Diagnostics;

namespace TaskManagementService.API.Middleware
{
    /// <summary>
    /// Промежуточное ПО для логирования HTTP-запросов
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;
                var statusCode = context.Response?.StatusCode;
                var method = context.Request?.Method;
                var path = context.Request?.Path.Value;

                _logger.LogInformation(
                    "HTTP {Method} {Path} responded {StatusCode} in {Elapsed} ms",
                    method,
                    path,
                    statusCode,
                    elapsed);
            }
        }
    }
} 