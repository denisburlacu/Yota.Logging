using System;
using Microsoft.Extensions.DependencyInjection;

namespace Yota.Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddYotaLogging(this IServiceCollection serviceCollection)
        {
            var configurator = new LoggerConfigurator(serviceCollection);
            configurator.Build();

            return serviceCollection;
        }

        public static IServiceCollection AddYotaLogging(this IServiceCollection serviceCollection, Action<LoggerConfigurator> loggerConfigurator)
        {
            var logger = new LoggerConfigurator(serviceCollection);

            loggerConfigurator(logger);

            logger.Build();

            return serviceCollection;
        }
        
        public static IServiceCollection AddDefaultYotaLogging(this IServiceCollection serviceCollection)
        {
            var configurator = new LoggerConfigurator(serviceCollection);
            configurator
                .ToConsole()
                .ToFile()
                .Build();

            return serviceCollection;
        }
    }
}