
using Application.Abstractions;
using Application.Repositories;

namespace API.BackgroundJob
{
    public class ExpiredRentalContracCleanupJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpiredRentalContracCleanupJob> _logger;

        public ExpiredRentalContracCleanupJob(
            IServiceProvider serviceProvider,
            ILogger<ExpiredRentalContracCleanupJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("ExpiredRentalContracCleanupJob started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<IRentalContractService>();
                        await service.ExpiredContractCleanUpAsync();

                        _logger.LogInformation("Expired contract cleanup executed (immediate run).");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing immediate cleanup job");
                }

                var now = DateTime.Now;

                var nextRun = now.Date.AddDays(1);          // 00:00 ngày mai
                var delay = nextRun - now;

                _logger.LogInformation($"Next cleanup will run at {nextRun}");

                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
