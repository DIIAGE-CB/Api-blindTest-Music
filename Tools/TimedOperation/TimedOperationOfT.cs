using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Tools.TimedOperation;

/*
 * The `TimedOperation<TResult>` class is a generic helper for timing the execution duration
 * of an operation (sync or async), logging start, success, and error events automatically.
 * It uses IDisposable to measure elapsed time and logs using an injected ILogger instance.
 */
public class TimedOperation<TResult> : IDisposable
{
    private readonly Stopwatch _stopwatch;
    private readonly ILogger _logger;
    private readonly string _operationName;
    private readonly LogLevel _successLevel;
    private readonly LogLevel _errorLevel;
    private readonly Action<TResult, TimeSpan>? _resultLogger;
    private readonly Action<Exception, TimeSpan>? _errorHandler;
    private TResult? _result;
    private Exception? _exception;

    /// <summary>
    /// Internal constructor initializes the timer, logger, and configuration, and starts timing.
    /// </summary>
    internal TimedOperation(
        ILogger logger,
        string operationName,
        LogLevel successLevel = LogLevel.Information,
        LogLevel errorLevel = LogLevel.Error,
        Action<TResult, TimeSpan>? resultLogger = null,
        Action<Exception, TimeSpan>? errorHandler = null)
    {
        _logger = logger;
        _operationName = operationName;
        _successLevel = successLevel;
        _errorLevel = errorLevel;
        _resultLogger = resultLogger;
        _errorHandler = errorHandler;
        _stopwatch = Stopwatch.StartNew();

        LogStart();
    }

    /// <summary>
    /// Factory method to create a new TimedOperation instance with the specified parameters.
    /// </summary>
    public static TimedOperation<TResult> Measure(
        ILogger logger,
        string operationName,
        LogLevel successLevel = LogLevel.Information,
        LogLevel errorLevel = LogLevel.Error,
        Action<TResult, TimeSpan>? resultLogger = null,
        Action<Exception, TimeSpan>? errorHandler = null)
    {
        return new TimedOperation<TResult>(
            logger,
            operationName,
            successLevel,
            errorLevel,
            resultLogger,
            errorHandler);
    }

    /// <summary>
    /// Executes the specified asynchronous operation while timing it, logging start, success or error automatically.
    /// </summary>
    public static async Task<TResult> ExecuteTimedAsync(
        ILogger logger,
        string operationName,
        Func<Task<TResult>> operation,
        LogLevel successLevel = LogLevel.Information,
        LogLevel errorLevel = LogLevel.Error,
        Action<TResult, TimeSpan>? resultLogger = null,
        Action<Exception, TimeSpan>? errorHandler = null)
    {
        using var timer = Measure(logger, operationName, successLevel, errorLevel, resultLogger, errorHandler);
        return await timer.ExecuteAsync(operation);
    }

    /// <summary>
    /// Executes the specified synchronous operation while timing it, logging start, success or error automatically.
    /// </summary>
    public static TResult ExecuteTimed(
        ILogger logger,
        string operationName,
        Func<TResult> operation,
        LogLevel successLevel = LogLevel.Information,
        LogLevel errorLevel = LogLevel.Error,
        Action<TResult, TimeSpan>? resultLogger = null,
        Action<Exception, TimeSpan>? errorHandler = null)
    {
        using var timer = Measure(logger, operationName, successLevel, errorLevel, resultLogger, errorHandler);
        return timer.Execute(operation);
    }

    /// <summary>
    /// Executes the given asynchronous operation, capturing result or exception for logging on Dispose.
    /// </summary>
    /// <param name="operation">An asynchronous function returning TResult.</param>
    public async Task<TResult> ExecuteAsync(Func<Task<TResult>> operation)
    {
        try
        {
            _result = await operation();
            return _result;
        }
        catch (Exception ex)
        {
            _exception = ex;
            throw;
        }
    }

    /// <summary>
    /// Executes the given synchronous operation, capturing result or exception for logging on Dispose.
    /// </summary>
    /// <param name="operation">A synchronous function returning TResult.</param>
    public TResult Execute(Func<TResult> operation)
    {
        try
        {
            _result = operation();
            return _result;
        }
        catch (Exception ex)
        {
            _exception = ex;
            throw;
        }
    }

    /// <summary>
    /// Stops timing, logs the outcome (success or failure), and invokes optional callbacks.
    /// </summary>
    public void Dispose()
    {
        _stopwatch.Stop();

        if (_exception != null)
        {
            LogError(_exception);
            _errorHandler?.Invoke(_exception, _stopwatch.Elapsed);
        }
        else if (_result != null)
        {
            LogSuccess();
            _resultLogger?.Invoke(_result, _stopwatch.Elapsed);
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Logs the start of the operation with timestamp.
    /// </summary>
    private void LogStart()
    {
        _logger.Log(
            _successLevel,
            "Starting {Operation} (StartTime: {StartTime:HH:mm:ss.fff})",
            _operationName,
            DateTime.UtcNow);
    }

    /// <summary>
    /// Logs a success message with elapsed time and memory usage.
    /// </summary>
    private void LogSuccess()
    {
        _logger.Log(
            _successLevel,
            "Completed {Operation} in {ElapsedMs}ms (Memory: {MemoryMB}MB)",
            _operationName,
            _stopwatch.ElapsedMilliseconds,
            Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024);
    }

    /// <summary>
    /// Logs an error message with exception details, elapsed time, and memory usage.
    /// </summary>
    /// <param name="ex">Exception caught during the operation.</param>
    private void LogError(Exception ex)
    {
        _logger.Log(
            _errorLevel,
            ex,
            "Failed {Operation} after {ElapsedMs}ms (Memory: {MemoryMB}MB)",
            _operationName,
            _stopwatch.ElapsedMilliseconds,
            Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024);
    }
}