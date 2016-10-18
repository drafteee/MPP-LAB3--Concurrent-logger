using System.Threading.Tasks;

namespace Concurrent_logger
{
    public interface ILoggerTarget
    {
        bool Flush(AboutLog log);

        Task<bool> FlushAsync(AboutLog log);
    }
}