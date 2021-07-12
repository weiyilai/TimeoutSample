using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeoutSample.Job
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNLog(
             this IServiceCollection services,
             IConfiguration config)
        {
            services.
                AddTransient<Runner>().
                AddLogging(loggingBuilder =>
                {
                    // configure Logging with NLog
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                });

            return services;
        }
    }
}
