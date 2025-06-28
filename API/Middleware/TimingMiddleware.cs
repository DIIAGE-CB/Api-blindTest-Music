using System.Diagnostics;

namespace API.Middleware;

/// <summary>
/// <c>TimingMiddleware</c> is a custom ASP.NET Core middleware that logs the execution time of each HTTP request.
/// It tracks and logs the start, completion, and any exception that occurs during request handling.
/// </summary>
public class TimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TimingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware component in the pipeline.</param>
    /// <param name="logger">Logger instance used to log timing and error information.</param>
    public TimingMiddleware(RequestDelegate next, ILogger<TimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Handles an incoming HTTP request, measures its execution duration, and logs timing information.
    /// </summary>
    /// <param name="context">The HTTP context representing the current request.</param>
    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var path = context.Request.Path;

        try
        {
            _logger.LogInformation("Starting request {Path}", path);

            // Call the next middleware or request handler in the pipeline
            await _next(context);

            _logger.LogInformation("Completed request {Path} in {ElapsedMs}ms",
                path, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in request {Path} after {ElapsedMs}ms",
                path, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}