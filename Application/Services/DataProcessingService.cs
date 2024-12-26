using Domain.Entites;
using Domain.Interfaces;

namespace Application.Services;

public class DataProcessingService
{

    private readonly IMessageBrokerService _messageBrokerService;

    public DataProcessingService(IMessageBrokerService messageBrokerService)
    {
        _messageBrokerService = messageBrokerService;
    }

    public async Task ProcessAndSendDataAsync()
    {
        var dataPoints = GenerateDataPoints();
        await _messageBrokerService.SendDataAsync(dataPoints);
    }

    private List<DataPoint> GenerateDataPoints()
    {
        var dataPoints = new List<DataPoint>();

        for (int i = 0; i < 5500; i++)  // تولید 5500 داده در هر ثانیه
        {
            dataPoints.Add(new DataPoint
            {
                Name = $"DataPoint{i}",
                Value = new Random().Next(1, 1000),
                Time = DateTime.Now
            });
        }
        return dataPoints;
    }
    

}

