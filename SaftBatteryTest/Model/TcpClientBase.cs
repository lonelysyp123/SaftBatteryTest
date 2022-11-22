using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SaftBatteryTest.Model
{
    public class TcpClientBase
    {
        protected Socket Client;
        public TcpClientBase()
        {

        }

        public bool Connect(string ip, int port)
        {
            //! 这里默认连接成功
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Client.Connect(point);
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接失败:{0}", ex.Message);
                return false;
            }
            return true;
        }

        public bool DisConnect()
        {
            try
            {
                if (Client.Connected)
                {
                    Client.Disconnect(false);
                    Client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("断开连接失败:{0}", ex.Message);
                return false;
            }
            return true;
        }

        public void SendMsg(byte[] bytes)
        {
            if (Client.Connected)
            {
                Client.Send(bytes);
            }
        }

        public byte[] ReceiveMsg()
        {
            if(Client.Connected)
            {
                byte[] buffer = new byte[Client.ReceiveBufferSize];
                int count = Client.Receive(buffer);
                return buffer;
            }
            return null;
        }

        public byte[] Request(byte[] bytes)
        {
            if (Client.Connected)
            {
                Client.Send(bytes);
                byte[] buffer = new byte[Client.ReceiveBufferSize];
                int count = Client.Receive(buffer);
                return buffer;
            }
            return null;
        }

        public void KeepAlive()
        {

        }
    }
}
