using Microsoft.Extensions.Logging;

namespace Tools.TimedOperation;

/* 
 * The non-generic static `TimedOperation` class is a helper for timing and logging void operations 
 * (operations without return values). It provides overloads to measure and execute timed operations 
 * asynchronously and synchronously, reusing the generic TimedOperation<object> under the hood.
 */
public static class TimedOperation
{
    /// <summary>
    /// Creates a new TimedOperation instance configured for void operations with optional completion and error handlers.
    /// </summary>
    public static TimedOperation<object> Measure(
        ILogger logger,
        string operationName,
        LogLevel successLevel = LogLevel.Information,
        LogLevel errorLevel = LogLevel.Error,
        Action<TimeSpan>? completionLogger = null,
        Action<Exception, TimeSpan>? errorHandler = null)
    {
        Action<object, TimeSpan>? resultLogger = completionLogger != null
            ? (_, elapsed) => completionLogger(elapsed)
            : null;

        return new TimedOperation<object>(
            logger,
            operationName,
            successLevel,
            errorLevel,
            resultLogger,
            errorHandler);
    }

    /// <summary>
    /// Executes the specified asynchronous void operation with timing and logging.
    /// </summary>
    /// <param name="operation">An asynchronous void operation to execute.</param>
    public static async Task ExecuteTimedAsync(
        ILogger logger,
        string operationName,
        Func<Task> operation,
        LogLevel successLevel = LogLevel.Information,
        LogLevel errorLevel = LogLevel.Error,
        Action<TimeSpan>? completionLogger = null,
        Action<Exception, TimeSpan>? errorHandler = null)
    {
        using var timer = Measure(logger, operationName, successLevel, errorLevel, completionLogger, errorHandler);
        await timer.ExecuteAsync(async () => { await operation(); return null!; });
    }

    /// <summary>
    /// Executes the specified synchronous void operation with timing and logging.
    /// </summary>
    /// <param name="operation">A synchronous void operation to execute.</param>
    public static void ExecuteTimed(
        ILogger logger,
        string operationName,
        Action operation,
        LogLevel successLevel = LogLevel.Information,
        LogLevel errorLevel = LogLevel.Error,
        Action<TimeSpan>? completionLogger = null,
        Action<Exception, TimeSpan>? errorHandler = null)
    {
        using var timer = Measure(logger, operationName, successLevel, errorLevel, completionLogger, errorHandler);
        timer.Execute(() => { operation(); return null!; });
    }

    /// <summary>
    /// Wrapper to run an operation with timing, logging, and error handling simplified.
    /// </summary>
    public static Task<T> TimedCallAsync<T>(
        ILogger logger,
        string operationName,
        Func<Task<T>> func,
        Action<T, TimeSpan>? resultLogger = null,
        Action<Exception, TimeSpan>? errorHandler = null)
    {
        return TimedOperation<T>.ExecuteTimedAsync(
            logger,
            operationName,
            func,
            resultLogger: resultLogger,
            errorHandler: errorHandler);
    }

    /// <summary>
    /// Wrapper to run a synchronous operation with timing, logging, and error handling simplified.
    /// </summary>
    public static T TimedCall<T>(
        ILogger logger,
        string operationName,
        Func<T> func,
        Action<T, TimeSpan>? resultLogger = null,
        Action<Exception, TimeSpan>? errorHandler = null)
    {
        return TimedOperation<T>.ExecuteTimed(
            logger,
            operationName,
            func,
            resultLogger: resultLogger,
            errorHandler: errorHandler);
    }
}
