using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TMC_API.Connection
{
    public class SerialConnection : Connection
    {
        SerialPort port;
        public SerialConnection(string Port, int BaudRate)
        {
            port = new SerialPort(Port);
            port.BaudRate = BaudRate;
            base.StartCalculatingStats();
        }
        ~SerialConnection()
        {
            StopCalculatingStats();
        }
        public override void Connect()
        {
            try
            {
                port.Open();
                IsConnected = true;
                port.DataReceived += Port_DataReceived;
            }
            catch 
            {
                IsConnected = false;
            }
        }

        public override void Disconnect()
        {
            port.Close();
            base.StopCalculatingStats();
            IsConnected= false;
            port.DataReceived -= Port_DataReceived;
        }
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (port.BytesToRead > 0)
            {
                int len = port.BytesToRead;
                byte[] data = new byte[len];
                port.Read(data, 0, len);
               // byte[] data = port.ReadData(port.BytesToRead);
                InvokeNewDataCallback(data);
            }

        }

        public override void SendPacket(byte[] data)
        {
            base.SendPacket(data);
            port.Write(data, 0, data.Length);
        }
    }
}
