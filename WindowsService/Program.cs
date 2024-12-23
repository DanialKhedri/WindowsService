using Application.Services;
using Domain.Interfaces;
using infrastructure.MessageBroker;


namespace WindowsService;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IMessageBrokerService, RabbitMqService>();  // ثبت سرویس RabbitMQ
                services.AddSingleton<DataProcessingService>();  // ثبت سرویس پردازش داده
                services.AddHostedService<Worker>();  // ثبت Worker
            }).UseWindowsService();


}
