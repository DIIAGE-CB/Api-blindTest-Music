/// <summary>
/// The <c>LogRetentionService</c> class is a background service that periodically deletes log files
/// older than 6 months from a specified directory. It is intended for use in ASP.NET Core applications
/// to manage log file retention and cleanup.
/// </summary>
public class LogRetentionService : BackgroundService
{
    private readonly string _logDirectory;
    private readonly ILogger<LogRetentionService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogRetentionService"/> class.
    /// </summary>
    /// <param name="logger">The logger used to record information and errors.</param>
    /// <param name="configuration">The application configuration used to locate the log directory.</param>
    public LogRetentionService(ILogger<LogRetentionService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _logDirectory = Path.GetDirectoryName(configuration["Serilog:WriteTo:1:Args:path"]) ?? "Logs";
    }

    /// <summary>
    /// Executes the background service logic. This method runs continuously until cancellation is requested.
    /// It deletes old log files every 10 days.
    /// </summary>
    /// <param name="stoppingToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous execution operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("LogRetentionService started. Log directory: {LogDirectory}", _logDirectory);

        while (!stoppingToken.IsCancellationRequested)
        {
            DeleteOldLogFiles();

            await Task.Delay(TimeSpan.FromDays(10), stoppingToken);
        }

        _logger.LogInformation("LogRetentionService stopped.");
    }

    /// <summary>
    /// Deletes log files from the log directory that are older than 6 months.
    /// Logs actions and errors during the deletion process.
    /// </summary>
    private void DeleteOldLogFiles()
    {
        try
        {
            if (!Directory.Exists(_logDirectory))
            {
                _logger.LogWarning("Log directory does not exist: {LogDirectory}", _logDirectory);
                return;
            }

            var logFiles = Directory.GetFiles(_logDirectory, "*.log");

            if (logFiles.Length == 0)
            {
                _logger.LogInformation("No log files found in the directory.");
            }

            foreach (var file in logFiles)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.CreationTime < DateTime.UtcNow.AddMonths(-6))
                {
                    try
                    {
                        fileInfo.Delete();
                        _logger.LogInformation("Deleted old log file: {LogFile}", fileInfo.FullName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error deleting log file: {LogFile}", fileInfo.FullName);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteOldLogFiles.");
        }
    }
}
