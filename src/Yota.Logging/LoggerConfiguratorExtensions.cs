using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Yota.Logging
{
    public static class LoggerConfiguratorExtensions
    {
        public static LoggerConfigurator ToConsole(this LoggerConfigurator configurator)
        {
            configurator.LoggerConfiguration = configurator.LoggerConfiguration
                .WriteTo
                .Console(outputTemplate: Constants.Template);

            return configurator;
        }

        public static LoggerConfigurator ToElastic(this LoggerConfigurator configurator,string environment, string url)
        {
            configurator.LoggerConfiguration = configurator.LoggerConfiguration
                .WriteTo
                .Elasticsearch(new ElasticsearchSinkOptions(new Uri(url))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    ConnectionTimeout = TimeSpan.FromMinutes(1),
                    IndexFormat =
                        $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
                });

            return configurator;
        }
        
        public static LoggerConfigurator InLogLevel(this LoggerConfigurator configurator,LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    configurator.LoggerConfiguration = configurator.LoggerConfiguration.MinimumLevel.Fatal();
                    break;
                case LogLevel.Trace:
                    configurator.LoggerConfiguration = configurator.LoggerConfiguration.MinimumLevel.Verbose();
                    break;
                case LogLevel.Debug:
                    configurator.LoggerConfiguration = configurator.LoggerConfiguration.MinimumLevel.Debug();
                    break;
                case LogLevel.Information:
                    configurator.LoggerConfiguration = configurator.LoggerConfiguration.MinimumLevel.Information();
                    break;
                case LogLevel.Warning:
                    configurator.LoggerConfiguration = configurator.LoggerConfiguration.MinimumLevel.Warning();
                    break;
                case LogLevel.Error:
                    configurator.LoggerConfiguration = configurator.LoggerConfiguration.MinimumLevel.Error();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
            
            return configurator;
        }
        
        public static LoggerConfigurator WithCallback(this LoggerConfigurator configurator, Action<string> reader, LogEventLevel logEventLevel)
        {
            configurator.LoggerConfiguration = configurator.LoggerConfiguration
                .WriteTo.Sink(new CustomSink(reader, logEventLevel));

            return configurator;
        }
        
        public static LoggerConfigurator ToFile(this LoggerConfigurator configurator, string path = "")
        {
            const string fileNamePattern = "logs/log-.txt";
            var namePattern = !string.IsNullOrWhiteSpace(path) ? Path.Combine(Path.GetFullPath(path), fileNamePattern) : fileNamePattern;
            configurator.LoggerConfiguration =
                configurator.LoggerConfiguration
                    .WriteTo
                    .File(namePattern, rollingInterval: RollingInterval.Day);

            return configurator;
        }
    }
}