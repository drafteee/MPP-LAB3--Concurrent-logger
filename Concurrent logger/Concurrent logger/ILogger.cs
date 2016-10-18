namespace Concurrent_logger
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }
}