using Domain.Entites;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
namespace infrastructure.MessageBroker;

public class RabbitMqService : IMessageBrokerService
{

    private readonly ILogger<RabbitMqService> _logger;
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMqService(ILogger<RabbitMqService> logger)
    {
        _logger = logger;
        InitializeMessageBroker();
    }

    private async Task InitializeMessageBroker()
    {
        // اتصال به RabbitMQ
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();


        // اعلام صف
        await _channel.QueueDeclareAsync(
             queue: "dataQueue",  // نام صف
             durable: false,      // صف دایمی نباشد
             exclusive: false,    // صف اختصاصی نباشد
             autoDelete: false,   // صف حذف خودکار نشود
             arguments: null);    // هیچ آرگومانی برای صف نداریم



    }

    // ارسال داده به RabbitMQ
    public async Task SendDataAsync(List<DataPoint> dataPoints, CancellationToken cancellationToken = default)
    {
        try
        {
            // تبدیل داده‌ها به JSON
            var jsonMessage = JsonConvert.SerializeObject(dataPoints);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            // تنظیم BasicProperties به صورت دستی
            var basicProperties = new BasicProperties
            {
                ContentType = "application/json",  // نوع محتوا
                Persistent = true  // ارسال پیام به صورت پایدار
            };

            // ارسال پیام به صف
            await _channel.BasicPublishAsync(
                exchange: "",  // استفاده از اکسچنج پیش‌فرض
                routingKey: "dataQueue",  // نام صف
                mandatory: false,  // عدم اجباری بودن ارسال پیام
                basicProperties: basicProperties,  // تنظیمات پیام
                body: body,  // داده پیام
                cancellationToken: cancellationToken  // پشتیبانی از لغو عملیات
            );

            _logger.LogInformation("Message published to RabbitMQ: 5000 Data");
        }
        catch (Exception ex)
        {
            // لاگ خطا
            _logger.LogError(ex, "Error while publishing message to RabbitMQ.");
        }
    }


}
