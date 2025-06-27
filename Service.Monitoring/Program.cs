using Microsoft.Extensions.Hosting;

namespace Service.Monitoring;

public class Program
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddTracingServices();
            })
            .Build();

        await host.StartAsync();
        await host.StopAsync();
    }
}