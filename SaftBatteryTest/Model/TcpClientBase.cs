using CommunityToolkit.Mvvm.ComponentModel;
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
    public class TcpClientBase : ObservableObject
    {
        private string _communicationState;
        public string CommunicationState
        {
            get => _communicationState;
            set
            {
                SetProperty(ref _communicationState, value);
            }
        }
        protected Socket Client;
        protected ushort index = 0;
        public TcpClientBase()
        {
            CommunicationState = "Disconnected";
        }

        public bool Connect(string ip, int port)
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Client.Connect(point);
                CommunicationState = "Connected";
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
                    Client = null;
                    CommunicationState = "DisConnected";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("断开连接失败:{0}", ex.Message);
                return false;
            }
            return true;
        }

        private void SendMsg(byte[] bytes)
        {
            if (Client.Connected)
            {
                byte[] numberBytes = BitConverter.GetBytes(index);
                bytes[0] = numberBytes[0];
                bytes[1] = numberBytes[1];
                Client.Send(bytes);
                index++;
            }
        }

        public byte[] ReceiveMsg()
        {
            if(Client.Connected)
            {
                byte[] buffer = new byte[128];
                int count = Client.Receive(buffer);
                return buffer;
            }
            return null;
        }

        public byte[] Request(byte[] bytes)
        {
            SendMsg(bytes);
            return ReceiveMsg();
        }

        public void KeepAlive()
        {

        }
    }
}
