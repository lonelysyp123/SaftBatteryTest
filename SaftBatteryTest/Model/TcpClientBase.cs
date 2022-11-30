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
        protected Socket Client;
        protected ushort index = 0;
        public TcpClientBase()
        {
            
        }

        /// <summary>
        /// TCP连接
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号</param>
        /// <returns>true:连接成功；false:连接失败</returns>
        public bool Connect(string ip, int port)
        {
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

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns>true:断开成功；false:断开失败</returns>
        public bool DisConnect()
        {
            try
            {
                if (Client.Connected)
                {
                    Client.Disconnect(false);
                    Client.Close();
                    Client = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("断开连接失败:{0}", ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="bytes">信息内容</param>
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

        /// <summary>
        /// 接受信息
        /// </summary>
        /// <returns>信息内容</returns>
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

        /// <summary>
        /// 请求响应
        /// </summary>
        /// <param name="bytes">请求内容</param>
        /// <returns>响应内容</returns>
        public byte[] Request(byte[] bytes)
        {
            SendMsg(bytes);
            return ReceiveMsg();
        }

        /// <summary>
        /// 保活机制
        /// </summary>
        public void KeepAlive()
        {

        }
    }
}
