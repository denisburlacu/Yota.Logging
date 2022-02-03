using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Yota.Logging
{
    public class LoggerConfigurator
    {
        private readonly IServiceCollection _serviceCollection;
        internal LoggerConfiguration LoggerConfiguration;

        public LoggerConfigurator(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            LoggerConfiguration = new LoggerConfiguration();
        }

        internal void Build()
        {
            _serviceCollection.AddLogging(configure => configure.AddSerilog());
            Log.Logger = LoggerConfiguration.CreateLogger();
        }
    }
}