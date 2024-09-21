
namespace Backend;

public class WCheckingTimes : BackgroundService
{
    private readonly ILogger<WCheckingTimes> _logger;

    public WCheckingTimes(ILogger<WCheckingTimes> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CheckingTimes background service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            //todo you were in here
            _logger.LogInformation($"Current Time: {DateTime.Now}");

            
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

        _logger.LogInformation("CheckingTimes background service is stopping.");
    }
}
