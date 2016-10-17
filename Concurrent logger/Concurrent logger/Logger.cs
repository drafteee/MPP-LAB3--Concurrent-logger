using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrent_logger
{
    class Logger : ILogger
    {
        private byte limit;
        private List<AboutLog> bufferLogs;
        private List<Task> listTasks;
        private ILoggerTarget[] targets;
        private volatile int bufferId;
        private class ThreadInfo
        {
            public List<AboutLog> logs;
            public int threadId;
        }
        public Logger(byte limit, ILoggerTarget[] targets)
        {
            this.limit = limit;
            this.targets = targets;
            bufferLogs = new List<AboutLog>();
        }
        public void Log(LogLevel level, string message)
        {
            if (bufferLogs.Count == limit)
            {
                listTasks.Add(Task.Run(() =>
                {
                    FlushLogs(new ThreadInfo { logs = bufferLogs, threadId = bufferId++ });
                }));
                bufferLogs = new List<AboutLog>();
            }
            bufferLogs.Add(new AboutLog(level,message));
        }

        private void FlushLogs(ThreadInfo threadInfo)
        {

        }

    }
}
