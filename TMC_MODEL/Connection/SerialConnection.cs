using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
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
        }
        public override void Connect()
        {
            port.Close();
            port.Open();
            port.DataReceived += Port_DataReceived;
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (port.BytesToRead > 0)
            {
                byte[] data = new byte[port.BytesToRead];
                port.Read(data, 0, port.BytesToRead);
               // byte[] data = port.ReadData(port.BytesToRead);
                InvokeNewDataCallback(data);
            }

        }
    }
}
