using InjectionMap.Tracing;
using System;
using System.Diagnostics;
using System.Text;

namespace InjectionMap.Test
{
    class TraceLogger : ILogger
    {
        public void Write(string message, LogLevel loglevel = LogLevel.Info, string source = null, string category = null, DateTime? logtime = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"#### InjectionMap Testoutput ####");
            sb.AppendLine($"Message: {message}");
            sb.AppendLine($"Level: {loglevel}");

            if (!string.IsNullOrEmpty(source))
            {
                sb.AppendLine($"Source: {source}");
            }

            if (!string.IsNullOrEmpty(category))
            {
                sb.AppendLine($"Category: {category}");
            }
            
            sb.AppendLine($"Time: {logtime ?? DateTime.Now}");
            sb.AppendLine($"####");

            Trace.WriteLine(sb.ToString());
        }
    }
}
