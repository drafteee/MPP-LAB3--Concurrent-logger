using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrent_logger
{
    class LoggerTarget : ILoggerTarget
    {
        private FileStream fileStream;

        public LoggerTarget(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        public bool Flush(AboutLog log)
        {
            Write(Encoding.Default.GetBytes(log.GetMessage().ToArray()));
            fileStream.Flush();
            return true;
        }

        public void Write(byte[] log)
        {
            fileStream.Write(log, 0, log.Length);
        }

        public async Task<bool> FlushAsync(AboutLog log)
        {
            Write(Encoding.Default.GetBytes(log.GetMessage().ToArray()));
            await fileStream.FlushAsync();
            return true;
        }
    }
}