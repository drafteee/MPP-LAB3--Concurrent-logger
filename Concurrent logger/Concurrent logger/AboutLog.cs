using System;
using System.Threading;

namespace Concurrent_logger
{
    public class AboutLog
    {
        public string message,messageTest;
        public LogLevel level;

        public AboutLog(LogLevel level, string message)
        {
            this.message = String.Format("[ {0} ]- {1} {2} {3} {4} ", DateTime.Now, Thread.CurrentThread.ManagedThreadId, level, message, Environment.NewLine);
            messageTest = message;
            this.level = level;
        }

        public string GetMessage() => message;
        public void Print()
        {
            Console.WriteLine(message);
        }
    }
}