using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

/// <summary>
/// The <c>StackTraceEnricher</c> class is a custom Serilog enricher that adds the caller method name
/// and Unix timestamp to each log event. It implements the <see cref="ILogEventEnricher"/> interface
/// and is typically used for enhanced logging and traceability.
/// </summary>
public class StackTraceEnricher : ILogEventEnricher
{
    /// <summary>
    /// Enriches a <see cref="LogEvent"/> by adding a Unix timestamp and caller information based on the current stack trace.
    /// </summary>
    /// <param name="logEvent">The log event to be enriched with additional properties.</param>
    /// <param name="propertyFactory">The factory used to create new log event properties.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var caller = GetCallerFromStackTrace();
        var unixTimestamp = logEvent.Timestamp.ToUnixTimeSeconds();

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UnixTimestamp", unixTimestamp));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Caller", caller));
    }

    /// <summary>
    /// Retrieves the method name of the caller from the current stack trace, skipping internal Serilog frames.
    /// </summary>
    /// <returns>A string containing the caller's type and method name, or "Unknown" if not resolvable.</returns>
    private string GetCallerFromStackTrace()
    {
        try
        {
            var stackTrace = new StackTrace();
            var callingMethod = stackTrace.GetFrames()
                ?.Skip(13) // Skip Serilog internals and this method
                .FirstOrDefault()?
                .GetMethod();

            return callingMethod != null
                ? $"{callingMethod.DeclaringType?.Name}.{callingMethod.Name}"
                : "Unknown";
        }
        catch (Exception)
        {
            return "Unknown";
        }
    }
}
