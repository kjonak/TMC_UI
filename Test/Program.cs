// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Net;
using System.Text;

void n(byte[] data)
{
    
    Console.WriteLine(Encoding.UTF8.GetString(data, 0, data.Length));
}

TMC_API.Connection.NetworkConnection conn = new("192.168.1.255", 6969, 6970);
conn.RegisterNewDataCallback(n);
conn.Connect();





var last = DateTime.Now;
while(true)
{
    if((DateTime.Now - last) > TimeSpan.FromMilliseconds(500))
    {
        var data = Encoding.UTF8.GetBytes("ABCD");
        conn.SendPacket(data);
        last = DateTime.Now;
    }

}


