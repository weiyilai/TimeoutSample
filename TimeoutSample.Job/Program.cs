using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Threading.Tasks;

namespace TimeoutSample.Job
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            // configuration builder
            var config = new ConfigurationBuilder().
                //From NuGet Package Microsoft.Extensions.Configuration.Json
                SetBasePath(System.IO.Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).
                Build();

            // host builder
            var builder = new HostBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddHttpClient();
                   services.AddTransient<MyService>();
                   services.AddNLog(config);
               }).UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var runner = services.GetRequiredService<Runner>();
                    runner.DoAction("Action1");

                    var myService = services.GetRequiredService<MyService>();
                    var result = await myService.ConnectAsync();
                    //if (result)
                    //{
                    //    Console.WriteLine(true);
                    //}
                    //else
                    //{
                    //    throw new Exception("is false");
                    //}
                }
                catch (Exception ex)
                {
                    // NLog: catch any exception and log it.
                    logger.Error(ex, ex.ToString());
                }
                finally
                {
                    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                    LogManager.Shutdown();
                }
            }

            Console.WriteLine("Press ANY key to exit");
            Console.ReadKey();
        }
    }
}
