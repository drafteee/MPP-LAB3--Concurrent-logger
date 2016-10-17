using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrent_logger
{
    public class AboutLog
    {
        private string message;
        public AboutLog(LogLevel level, string message)
        {
            this.message = String.Format("[ {0} ] {1} {2} {3}", DateTime.Now, level, message, Environment.NewLine);
        }
        public string GetMessage() => message;
    }
}
