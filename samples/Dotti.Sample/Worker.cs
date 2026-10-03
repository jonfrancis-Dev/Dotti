namespace Dotti.Sample;

public partial class Worker(ILogger<Worker> logger) : BackgroundService
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Worker running at: {Time}")]
    private partial void LogWorkerRunning(DateTimeOffset time);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            LogWorkerRunning(DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
