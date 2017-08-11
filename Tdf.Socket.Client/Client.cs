using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.Socket.Client
{
    public class Client
    {
        private const int Buffer = 1024;
        public System.Net.Sockets.Socket Socket;

        public Client(IPEndPoint iep)
        {
            Socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Connect(iep);
        }

        public string Receive()
        {
            var data = new byte[Buffer];
            var recv = Socket.Receive(data);
            return Encoding.UTF8.GetString(data, 0, recv);
        }

        public void SendFile(string fileName)
        {
            Socket.SendFile(fileName);
        }

        public void Send(string msg)
        {
            var data = Encoding.UTF8.GetBytes(msg);
            Socket.Send(data);
        }

        public void Close()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
        }
    }
}
