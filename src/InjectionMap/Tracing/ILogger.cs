using System;

namespace InjectionMap.Tracing
{
    public interface ILogger
    {
        /// <summary>
        /// Writes a message to the log provider
        /// </summary>
        /// <param name="message">The message to be logged</param>
        /// <param name="loglevel">The loglevel</param>
        /// <param name="source">The source of the log messag</param>
        /// <param name="category">The category that the log belonges to</param>
        /// <param name="logtime">The time when the log is created</param>
        void Write(string message, LogLevel loglevel = LogLevel.Info, string source = null, string category = null, DateTime? logtime = null);
    }
}
