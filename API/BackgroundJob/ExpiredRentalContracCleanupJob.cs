
using Application.Abstractions;
using Application.Repositories;
using Quartz;

namespace API.BackgroundJob
{
    public class ExpiredRentalContracCleanupJob : IJob
    {
        private readonly IRentalContractService _service;
        private readonly ILogger<LateReturnWarningJob> _logger;

        public ExpiredRentalContracCleanupJob(
            IRentalContractService service,
            ILogger<LateReturnWarningJob> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Quartz Expired rental contract cleanup job started...");
            await _service.ExpiredContractCleanUpAsync();
        }
    }
}
