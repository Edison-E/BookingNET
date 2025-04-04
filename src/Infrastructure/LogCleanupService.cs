using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookPro.Infrastructure;

public class LogCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LogCleanupService> _logger;

    public LogCleanupService(IServiceScopeFactory scopeFactory, ILogger<LogCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbConext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                try
                {
                    int deletedLogs = await dbConext.Database.ExecuteSqlRawAsync
                        ("DELETE FROM Logs WHERE Timestamp < DATEADD(DAY, -30, GETDATE())");

                    _logger.LogInformation($"[LogCleanup] {deletedLogs} registros eliminados.");

                    await dbConext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[LogCleanup] Error al limpiar logs.");
                }
            }
        }

        await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
    }
}
