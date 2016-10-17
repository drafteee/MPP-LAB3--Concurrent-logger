using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrent_logger
{
    class LoggerTarget : ILoggerTarget
    {
        private FileStream fileStream;

        public LoggerTarget(FileStream fileStr)
        {
            fileStream = fileStr;
        }
        public bool Flush()
        {
            throw new NotImplementedException();
        }   
                
        public Task<bool> FlushAsync()
        {
            throw new NotImplementedException();
        } 
    }
}
