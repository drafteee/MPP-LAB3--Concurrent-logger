using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Concurrent_logger;
using System.Threading.Tasks;

namespace Concurrent_logger_Tests
{
    [TestClass]
    public class LoggerTests
    {
        private volatile int index;
        [TestMethod]
        public void TestMethod_TestOneTarget()
        {
            index = 0;
            byte bufferLimit = 1;
            int logsCount = 10000;
            TestTarget testTarget = new TestTarget();
            StringBuilder stringBuilder = new StringBuilder();
            ILoggerTarget[] logTarget = new ILoggerTarget[] { testTarget };
            Logger logger = new Logger(bufferLimit,logTarget);

            Task.Run(() =>
            {
                logger.ProcessingQueue();
            });

            for (int i = 0; i < logsCount; i++)
            {
                string a = "task " + i;
                Task.Run(() =>
                {
                    logger.Log(LogLevel.Info, a);
                    index++;
                });
            }

            while (index != logsCount)
                if (index == logsCount)
                {
                    logger.FlushReamainLogs();
                }

            for (int i = 0; i < logsCount; i++)
                stringBuilder.Append(i);
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(stringBuilder.ToString()), testTarget.GetMessage());
            testTarget.Close();
        }

        [TestMethod]
        public void TestMethod_TestTwoTargets()
        {
            index = 0;
            byte bufferLimit = 1;
            int logsCount = 10;
            TestTarget firstTestTarget = new TestTarget();
            TestTarget secondTestTarget = new TestTarget();
            StringBuilder stringBuilder = new StringBuilder();
            ILoggerTarget[] logTarget = new ILoggerTarget[] { firstTestTarget, secondTestTarget };
            Logger logger = new Logger(bufferLimit, logTarget);
            Task.Run(() =>
            {
                logger.ProcessingQueue();
            });

            for (int i = 0; i < logsCount; i++)
            {
                string a = "task " + i;
                Task.Run(() =>
                {
                    logger.Log(LogLevel.Info, a);
                    index++;
                });
            }

            while (index != logsCount)
                if (index == logsCount)
                {
                    logger.FlushReamainLogs();

                    for (int i = 0; i < logsCount; i++)
                        stringBuilder.Append(i);
                    CollectionAssert.AreEqual(Encoding.Default.GetBytes(stringBuilder.ToString()), firstTestTarget.GetMessage());
                    firstTestTarget.Close();
                    CollectionAssert.AreEqual(Encoding.Default.GetBytes(stringBuilder.ToString()), secondTestTarget.GetMessage());
                    secondTestTarget.Close();
                }
        }

        [TestMethod]
        public void TestMethod_TestUdpTarget()
        {
            index = 0;
            byte bufferLimit = 5;
            int logsCount = 100;
            TestUdpServer udpServer = new TestUdpServer("127.0.0.1", 9000);
            LoggerTargetUdp targetUdp = new LoggerTargetUdp("127.0.0.1", 9000, "127.0.0.1", 10000);
            StringBuilder stringBuilder = new StringBuilder();
            ILoggerTarget[] logTarget = new ILoggerTarget[] { targetUdp };
            udpServer.StartReceive();

            Logger logger = new Logger(bufferLimit, logTarget);
            Task.Run(() =>
            {
                logger.ProcessingQueue();
            });

            for (int i = 0; i < logsCount; i++)
            {
                string a = "task " + i;
                Task.Run(() =>
                {
                    logger.Log(LogLevel.Info, a);
                    index++;
                });
            }

            while (index != logsCount)
                if (index == logsCount)
                {
                    logger.FlushReamainLogs();

                    for (int i = 0; i < logsCount; i++)
                        stringBuilder.Append(i);
                    udpServer.Synchronize();
                    udpServer.Close();
                    CollectionAssert.AreEqual(Encoding.Default.GetBytes(stringBuilder.ToString()), udpServer.GetMessage());
                }
        }
    }
}
