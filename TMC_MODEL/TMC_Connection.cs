using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net.Sockets;

namespace TMC_API
{
    public enum TMC_Connection_Type
        {
        SERIAL = 0,
        UDP = 1
        }

    public class TMC_Connection
    {

        SerialPort serialPort = new SerialPort();
        //Socket 
        private string? _IP, _PortName;
        private int? _Baudrate, _Port;
        private TMC_Connection_Type _type;
        private bool _connected = false;
        public bool Connected { get { return _connected; } }

        public delegate void Data(byte[] data);

        public Data? NewDataCallback;
        public bool ConnectSerial(string PortName, int BaudRate) 
        {
            if (_connected) { return false; }
            _type = TMC_Connection_Type.SERIAL;
            _PortName = PortName;
            _Baudrate = BaudRate;

            try
            {
                serialPort.PortName = _PortName;
                serialPort.BaudRate = BaudRate;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.DataBits = 8;

                serialPort.Open();
                serialPort.DataReceived += Serial_DataReceived;
                _connected = true;
                return true;
            }
            catch (Exception)
            {
               
                return false;
            }
        }
        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if(NewDataCallback == null)
                return;
            byte[] bytes = new byte[serialPort.BytesToRead];
            NewDataCallback(bytes);
        }

            
        public bool ConnectUDP(string IP, int Port)
        {
            if(_connected) { return false; }
            _type = TMC_Connection_Type.UDP;
            _IP = IP;
            _Port = Port;
            _connected = true;
            return true; 
        }
        public byte[]? GetPendingData()
        {
            return null;
        }

        public bool DataAvailable()
        {
            return false;
        }

        public bool SendBytes(byte[] data)
        {
            return true;
        }
        private void CloseConnection()
        {
            if(_type == TMC_Connection_Type.UDP)
            {

            }
            else if(_type == TMC_Connection_Type.SERIAL)
            {

            }
        }
    }
}
