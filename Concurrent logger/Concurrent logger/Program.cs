using System;
using System.IO;
using System.Threading.Tasks;

namespace Concurrent_logger
{
    public class Program
    {
        public static volatile int index;

        private static void Main(string[] args)
        {
            Console.Write("Input limit of buffer:");
            byte limit = Convert.ToByte(Console.ReadLine());

            Console.Write("Input count logs:");
            int countLogs = Convert.ToInt32(Console.ReadLine());

            FileStream fileStream = new FileStream(@"C:\My Documents\GitHub\MPP-LAB3--Concurrent-logger\Concurrent logger\Concurrent logger\output.txt", FileMode.OpenOrCreate);
            LoggerTarget loggetTarget = new LoggerTarget(fileStream);
            ILoggerTarget[] iLoggerTarget = new ILoggerTarget[] { loggetTarget };
            Logger logger = new Logger(limit, iLoggerTarget);

            Task.Run(() =>
            {
                logger.ProcessingQueue();
            });

            for (int i = 0; i < countLogs; i++)
            {
                string a = "task " + i;
                Task.Run(() =>
                {
                    logger.Log(LogLevel.Info, a);
                    index++;
                });
            }

            while (index != countLogs)
                if (index == countLogs)
                {
                    logger.FlushReamainLogs();
                    fileStream.Close();
                    fileStream.Dispose();

                    Console.ReadKey();
                }
        }
    }
}