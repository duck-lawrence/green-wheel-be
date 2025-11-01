using Application.Abstractions;
using Quartz;

namespace API.BackgroundJob
{
    public class LateReturnWarningJob : IJob
    {
        private readonly IRentalContractService _service;
        private readonly ILogger<LateReturnWarningJob> _logger;

        public LateReturnWarningJob(
            IRentalContractService service,
            ILogger<LateReturnWarningJob> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Quartz LateReturnWarning job started...");
            await _service.LateReturnContractWarningAsync(); ;
        }
    }
}
