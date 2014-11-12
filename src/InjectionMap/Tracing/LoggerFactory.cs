
namespace InjectionMap.Tracing
{
    public delegate ILogger CreateLoggerCallback();

    public class LoggerFactory : ILoggerFactory
    {
        public static CreateLoggerCallback LoggerCallback = () => new Logger();

        public ILogger CreateLogger()
        {
            return LoggerCallback();
        }
    }
}
