using System;

namespace InjectionMap.Tracing
{
    class Logger : ILogger
    {
        public void Write(string message, LogLevel loglevel = LogLevel.Info, string source = null, string category = null, DateTime? logtime = null)
        {
            // Trace is not enabled in a Portable Class Library. This  is only a dummy logger that does nothing.
        }
    }
}
