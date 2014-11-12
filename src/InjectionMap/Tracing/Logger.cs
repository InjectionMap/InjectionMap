using System;

namespace InjectionMap.Tracing
{
    class Logger : ILogger
    {
        public void Write(string message, string source = null, string category = null, DateTime? logtime = null)
        {
            // this  is only a dummy logger that does nothing.
        }
    }
}
