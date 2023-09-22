using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TMC_API.Connection
{
    public class NetworkConnection : Connection
    {
        string _TargetIP;
        int _ListenPort;
        int _TargetPort;
        public UdpClient udpClient;
        private bool _ShouldListen = false;
        public NetworkConnection(string TargetIP, int TargetPort, int ListenPort) 
        {
            udpClient = new UdpClient();
            _ListenPort = ListenPort;
            _TargetIP = TargetIP;
            _TargetPort = TargetPort;
        }

        public override void Connect()
        {
           
            _ShouldListen = true;
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, _ListenPort));
            IsConnected = true;
            var from = new IPEndPoint(0, 0);
            var task = Task.Run(() =>
            {
                while (_ShouldListen)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    InvokeNewDataCallback(recvBuffer);
                   
                }
            });
        }
        public override void SendPacket(byte[] data)
        {
            udpClient.Send(data, data.Length, _TargetIP, _TargetPort);
        }
        public override void Disconnect()
        {
            _ShouldListen = false;
            udpClient.Close();
        }
    }
}
