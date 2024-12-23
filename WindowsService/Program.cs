using Application.Services;
using Domain.Interfaces;
using infrastructure.MessageBroker;
using Serilog;


namespace WindowsService;

public class Program
{
    public static void Main(string[] args)
    {
        //Serilog
        string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(logDirectory, "log-.txt"), rollingInterval: RollingInterval.Day)
            .CreateLogger();


        try
        {

            CreateHostBuilder(args).Build().Run();

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An unhandled exception occurred during application startup.");
        }
        finally
        {
            Log.CloseAndFlush();
        }

    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IMessageBrokerService, RabbitMqService>();
                services.AddSingleton<DataProcessingService>();
                services.AddHostedService<Worker>();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddSerilog();
            })
            .UseWindowsService();
}
