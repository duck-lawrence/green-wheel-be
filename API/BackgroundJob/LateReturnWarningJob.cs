using Application.Abstractions;

namespace API.BackgroundJob
{
    public class LateReturnWarningJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LateReturnWarningJob> _logger;

        public LateReturnWarningJob(
            IServiceProvider serviceProvider,
            ILogger<LateReturnWarningJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("LateReturnWarningJob started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<IRentalContractService>();
                        await service.LateReturnContractWarningAsync();

                        _logger.LogInformation("Late return contract warning executed.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing immediate warning job");
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
