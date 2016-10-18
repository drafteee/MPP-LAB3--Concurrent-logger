using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Concurrent_logger;
using System.IO;

namespace Concurrent_logger_Tests
{
    class TestTarget : ILoggerTarget
    {
        private MemoryStream memStream;
        private StringBuilder message = new StringBuilder();

        public TestTarget()
        {
            memStream = new MemoryStream();
        }

        public void Write(AboutLog logInfo)
        {
            message.Append(logInfo.messageTest.Substring(5));
            byte[] log = Encoding.Default.GetBytes(logInfo.GetMessage().ToArray());
            memStream.Write(log, 0, log.Length);
        }

        public bool Flush(AboutLog logInfo)
        {
            Write(logInfo);
            memStream.Flush();
            return true;
        }

        public async Task<bool> FlushAsync(AboutLog logInfo)
        {
            Write(logInfo);
            await memStream.FlushAsync();
            return true;
        }

        public byte[] GetMessage()
        {
            return Encoding.Default.GetBytes(message.ToString());
        }

        public void Close()
        {
            memStream.Close();
            memStream.Dispose();
        }
    }
}
