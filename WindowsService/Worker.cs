using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService;


public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly DataProcessingService _dataProcessingService;

    public Worker(ILogger<Worker> logger, DataProcessingService dataProcessingService)
    {
        _logger = logger;
        _dataProcessingService = dataProcessingService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Processing data...");
            await _dataProcessingService.ProcessAndSendDataAsync();
            await Task.Delay(1000, stoppingToken);  // هر ثانیه
        }
    }

}
