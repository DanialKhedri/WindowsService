using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService;


public class Worker : BackgroundService
{

    #region Ctor
    private readonly ILogger<Worker> _logger;
    private readonly DataProcessingService _dataProcessingService;

    public Worker(ILogger<Worker> logger, DataProcessingService dataProcessingService)
    {
        _logger = logger;
        _dataProcessingService = dataProcessingService;
    }
    #endregion

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        await WaitForNetworkAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Processing data...");
            await _dataProcessingService.ProcessAndSendDataAsync();
            await Task.Delay(1000, stoppingToken);  // هر ثانیه
        }


    }



    private async Task WaitForNetworkAsync(CancellationToken cancellationToken)
    {
        bool isNetworkAvailable = false;

        while (!isNetworkAvailable && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                // بررسی وضعیت شبکه محلی
                isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();

                // اگر شبکه در دسترس است، بررسی اتصال به اینترنت
                if (isNetworkAvailable)
                {
                    isNetworkAvailable = await IsInternetAvailableAsync();
                }

                if (!isNetworkAvailable)
                {
                    _logger.LogInformation("Network is not available. Waiting...");
                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while checking network status: {ex.Message}");
            }
        }

        if (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        else
        {
            _logger.LogInformation("Network is available.");
        }
    }

    private async Task<bool> IsInternetAvailableAsync()
    {
        try
        {
            using (var client = new HttpClient())
            {
                // ارسال یک درخواست به Google برای بررسی اتصال اینترنت
                var result = await client.GetAsync("https://www.google.com");
                return result.IsSuccessStatusCode;
            }
        }
        catch
        {
            return false;
        }
    }



}
