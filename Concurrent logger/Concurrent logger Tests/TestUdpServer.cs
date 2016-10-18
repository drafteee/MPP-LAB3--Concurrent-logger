using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Concurrent_logger_Tests
{
    class TestUdpServer
    {
        private UdpClient udpClient;
        private string serverIP;
        private int serverPort;
        private StringBuilder stringBuilder;
        private volatile bool isReadSocket;
        private Task receiveAsync;

        public TestUdpServer(string serverIP, int serverPort)
        {
            stringBuilder = new StringBuilder();
            this.serverIP = serverIP;
            this.serverPort = serverPort;
        }

        public void StartReceive()
        {
            isReadSocket = true;
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(serverIP), serverPort));
            receiveAsync = Task.Factory.StartNew(() => TaskReceive());
        }

        public void TaskReceive()
        {
            try
            {
                while (isReadSocket)
                {
                    IPEndPoint clientPoint = null;
                    var receiveBytes = udpClient.Receive(ref clientPoint);
                    string message = Encoding.Default.GetString(receiveBytes);
                    int index = message.IndexOf("task") + 5;
                    stringBuilder.Append(message.Substring(index, message.Length - index - 2));
                }
            }
            finally
            {
                udpClient.Close();
            }
        }

        public void Synchronize()
        {
            receiveAsync.Wait(1500);
            isReadSocket = false;
            //            receiveAsync.Dispose();
            receiveAsync = null;
        }

        public byte[] GetMessage()
        {
            return Encoding.Default.GetBytes(stringBuilder.ToString());
        }

        public void Close()
        {
            udpClient.Close();
        }
    }
}
