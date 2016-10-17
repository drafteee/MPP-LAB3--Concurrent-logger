using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrent_logger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input limit of buffer:");

            byte limit = Convert.ToByte(Console.ReadLine());
            FileStream fileStream = new FileStream(@"C:\My Documents\GitHub\MPP-LAB3--Concurrent-logger\Concurrent logger\Concurrent logger\output.txt", FileMode.OpenOrCreate);
            LoggerTarget loggetTarget = new LoggerTarget(fileStream);
            ILoggerTarget[] iLoggerTarget = new ILoggerTarget[] { loggetTarget };
            Logger logger = new Logger(limit,iLoggerTarget);

            for (int i = 0; i < 500; i++)
            {
                logger.Log(LogLevel.Info, "task" + i);
            }

            fileStream.Close();
            fileStream.Dispose();

            Console.ReadKey();
        }
    }
}
