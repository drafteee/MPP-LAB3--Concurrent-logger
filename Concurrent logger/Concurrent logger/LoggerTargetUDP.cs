using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Concurrent_logger
{
    public class LoggerTargetUdp : ILoggerTarget
    {
        private string clientIp, serverIp;
        private int clientPort, serverPort;
        private UdpClient udpClient;

        public LoggerTargetUdp(string serverIp, int serverPort, string clientIp, int clientPort)
        {
            this.serverIp = serverIp;
            this.serverPort = serverPort;
            this.clientIp = clientIp;
            this.clientPort = clientPort;
        }

        public bool Flush(AboutLog logInfo)
        {
            Write(Encoding.Default.GetBytes(logInfo.GetMessage().ToArray()));
            udpClient.Close();
            return true;
        }

        public Task<bool> FlushAsync(AboutLog logInfo)
        {
            return Task.FromResult(true);
        }

        public void Write(byte[] log)
        {
            try
            {
                udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(clientIp), clientPort));
                udpClient.Send(log, log.Length, new IPEndPoint(IPAddress.Parse(serverIp), serverPort));
            }
            finally
            {
                udpClient.Close();
            }
        }

        public void Close()
        { }
    }
}