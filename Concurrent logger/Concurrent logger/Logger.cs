using System;
using System.Collections.Generic;
using System.Threading;

namespace Concurrent_logger
{
    public class Logger : ILogger
    {
        private byte limit;
        private List<AboutLog> bufferLogs;
        private ILoggerTarget[] targets;
        private Queue<List<AboutLog>> queueBuffers;
        private volatile int index;
        private volatile int flag = 0;
        private object locker = new object();

        public Logger(byte limit, ILoggerTarget[] targets)
        {
            this.limit = limit;
            this.targets = targets;
            bufferLogs = new List<AboutLog>();
            queueBuffers = new Queue<List<AboutLog>>();
        }

        public void Log(LogLevel level, string message)//вызывает с нескольк потоков
        {
            Monitor.Enter(locker);

            while (index != Convert.ToInt32(message.Substring(message.Length - (message.Length - 5), message.Length - 5)))
                Monitor.Wait(locker);

            if (bufferLogs.Count == limit)
            {
                while (flag != 0) { }
                queueBuffers.Enqueue(bufferLogs);
                bufferLogs = new List<AboutLog>();
            }

            bufferLogs.Add((new AboutLog(level, message)));
            index++;


            Monitor.PulseAll(locker);
            Monitor.Exit(locker);
            (new AboutLog(level, message)).Print();

        }

        public void FlushReamainLogs()
        {
            foreach (ILoggerTarget currentTarget in targets)
            {
                foreach (AboutLog log in bufferLogs)
                    currentTarget.Flush(log);
            }
        }
        public void ProcessingQueue()
        {
            while (true)
            {
                flag++;

                for (int i = 0; i < queueBuffers.Count; i++)
                {
                    var list = queueBuffers.Dequeue();
                    foreach (AboutLog log in list)
                        foreach (ILoggerTarget currentTarget in targets)
                        {
                            currentTarget.Flush(log);
                        }
                }
                flag--;
            }
        }
    }
}