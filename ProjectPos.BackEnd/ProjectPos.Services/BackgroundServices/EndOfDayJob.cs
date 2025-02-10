using Microsoft.Extensions.Logging;
using ProjectPos.Services.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.BackgroundServices
{
    public class EndOfDayJob : IJob
    {
        private readonly IDayEndSalesSummaryService _service;
        private readonly ILogger<EndOfDayJob> _logger;
        public EndOfDayJob(
            IDayEndSalesSummaryService service,
            ILogger<EndOfDayJob> logger)
        {
            _service = service;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var job = _service.CreateDayEndSalesSummaryAsync();
            if(job.Result.IsSuccess)
            {
                _logger.LogInformation($"Day End Sales Summary Created Successfully");
            }
            else
            {
                _logger.LogError($"Day End Sales Summary Creation Failed: {job.Result.Message}");
            }
            return Task.CompletedTask;
        }
    }
}
