using System;
using System.IO;
using System.Text;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Yota.Logging
{
    internal class CustomSink : ILogEventSink
    {
        private readonly Action<string> _error;
        private readonly LogEventLevel _logEventLevel;
        private readonly MessageTemplateTextFormatter _formatter;

        public CustomSink(Action<string> error, LogEventLevel logEventLevel)
        {
            _error = error;
            _logEventLevel = logEventLevel;

            _formatter = new MessageTemplateTextFormatter(Constants.Template);
        }

        public void Emit(LogEvent logEvent)
        {
            if ((int) _logEventLevel <= (int) logEvent.Level)
            {
                var stringWriter = new StringWriter(new StringBuilder(256));
                _formatter.Format(logEvent, stringWriter);
                var str = stringWriter.ToString();
                _error(str);
            }
        }
    }
}