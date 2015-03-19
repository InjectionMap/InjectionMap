using InjectionMap.Tracing;
using System;
using System.Diagnostics;
using System.Text;

namespace InjectionMap.Integration.UnitTests
{
    class TraceLogger : ILogger
    {
        public void Write(string message, LogLevel loglevel = LogLevel.Info, string source = null, string category = null, DateTime? logtime = null)
        {
            //if ((int)loglevel > (int)InjectionMap.Tracing.LogLevel.Warning)
            //    return;
            
            var sb = new StringBuilder();
            sb.AppendLine("#### InjectionMap Testoutput ####");
            sb.AppendLine(string.Format("Message: {0}", message));
            sb.AppendLine(string.Format("Level: {0}", loglevel));

            if (!string.IsNullOrEmpty(source))
                sb.AppendLine(string.Format("Source: {0}", source));
            
            if (!string.IsNullOrEmpty(category))
                sb.AppendLine(string.Format("Category: {0}", category));
            
            sb.AppendLine(string.Format("Time: {0}", logtime ?? DateTime.Now));
            sb.AppendLine("####");

            Trace.WriteLine(sb.ToString());
        }
    }
}
