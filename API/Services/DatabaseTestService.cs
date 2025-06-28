namespace API.Services;

using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The <c>DatabaseTestService</c> is a hosted background service responsible for periodically
/// verifying the connectivity of MongoDB and MSSQL databases. It implements <see cref="IHostedService"/>
/// to run on application startup and <see cref="IDisposable"/> for cleanup.
/// </summary>
public class DatabaseTestService : IHostedService, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseTestService> _logger;
    private readonly IServiceProvider _services;
    private bool _isTesting;
    private Timer? _timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseTestService"/> class.
    /// </summary>
    /// <param name="logger">Logger instance used to log information and errors.</param>
    /// <param name="services">Service provider used to resolve scoped services like DbContext or MongoClient.</param>
    /// <param name="configuration">Application configuration settings.</param>
    public DatabaseTestService(
        ILogger<DatabaseTestService> logger,
        IServiceProvider services,
        IConfiguration configuration)
    {
        _logger = logger;
        _services = services;
        _configuration = configuration;
    }

    /// <summary>
    /// Starts the background service and optionally triggers an immediate database test.
    /// </summary>
    /// <param name="cancellationToken">Token to signal cancellation.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Database Test Service starting");

        if (_configuration.GetValue<bool>("DatabaseTesting:RunOnStartup"))
        {
            _ = TestDatabasesAsync();
        }

        if (_configuration.GetValue<bool>("DatabaseTesting:Enabled"))
        {
            var intervalMinutes = _configuration.GetValue<int>("DatabaseTesting:IntervalMinutes", 60);
            _timer = new Timer(
                _ => _ = TestDatabasesAsync(),
                null,
                TimeSpan.FromMinutes(intervalMinutes),
                TimeSpan.FromMinutes(intervalMinutes));
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the background service and disposes the timer.
    /// </summary>
    /// <param name="cancellationToken">Token to signal cancellation.</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Database Test Service stopping");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Disposes resources used by the service.
    /// </summary>
    public void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Executes both MongoDB and MSSQL connection tests asynchronously.
    /// Prevents concurrent executions with a locking flag.
    /// </summary>
    private async Task TestDatabasesAsync()
    {
        if (_isTesting) return;

        _isTesting = true;
        try
        {
            _logger.LogInformation("Starting database connectivity tests");

            await TestMongoDBAsync();
            await TestMSSQLAsync();

            _logger.LogInformation("Database connectivity tests completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database connectivity test failed");
        }
        finally
        {
            _isTesting = false;
        }
    }

    /// <summary>
    /// Tests the MongoDB connection by listing available databases.
    /// </summary>
    private async Task TestMongoDBAsync()
    {
        try
        {
            using var scope = _services.CreateScope();
            var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();

            using var cursor = await mongoClient.ListDatabaseNamesAsync();
            var databases = await cursor.ToListAsync();

            _logger.LogInformation("MongoDB connection successful. Databases: {Databases}",
                string.Join(", ", databases));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoDB connection test failed");
            throw;
        }
    }

    /// <summary>
    /// Tests the MSSQL database connection using Entity Framework Core.
    /// </summary>
    private async Task TestMSSQLAsync()
    {
        try
        {
            using var scope = _services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (!await dbContext.Database.CanConnectAsync())
            {
                throw new Exception("Unable to connect to MSSQL database");
            }

            _logger.LogInformation("MSSQL connection successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MSSQL connection test failed");
            throw;
        }
    }
}
