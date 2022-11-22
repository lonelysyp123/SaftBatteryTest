using CommunityToolkit.Mvvm.ComponentModel;
using SaftBatteryTest.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SaftBatteryTest.Model
{
    public class BatteryTestDev : ObservableObject, IBatteryTestDev
    {
        public List<ChannelModel> Channels { get; set; }
        public BitmapSource Image { get; set; }
        public string Address { get; set; }
        public int DefaultPort { get; set; }
        private ModbusTcpClient client;

        private string _communicationState;
        public string CommunicationState 
        { 
            get => _communicationState; 
            set
            {
                SetProperty(ref _communicationState, value);
            }
        }

        public BatteryTestDev(string address)
        {
            DirectoryInfo directory = new DirectoryInfo("./Resource/Image");
            FileInfo[] files = directory.GetFiles("PC.png");
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(files[0].FullName, UriKind.Absolute);
            bi.EndInit();
            Image = bi;
            Address = address;
            DefaultPort = 502;
            CommunicationState = "Disconnected";

            // 初始化连接
            
        }

        /// <summary>
        /// 通过协议初始化InitChannel
        /// </summary>
        public void InitChannel()
        {
            int count = client.ReadChNums();
            Channels = new List<ChannelModel>();
            for (int i = 0; i < count; i++)
            {
                Channels.Add(new ChannelModel());
            }
        }

        /// <summary>
        /// 通过协议初始化InitChannel(测试用)
        /// </summary>
        /// <param name="count">通道数</param>
        public void InitChannel(int count)
        {
            // 这里默认创建4个
            Channels = new List<ChannelModel>();
            for (int i = 0; i < count; i++)
            {
                Channels.Add(new ChannelModel());
            }
            Channels[0].Title += Address;
        }

        /// <summary>
        /// 设备断开连接
        /// </summary>
        /// <returns>true:成功; false:失败</returns>
        public bool DisConnect()
        {
            bool IsDisConnected = false;
            //! 这里默认断开成功
            IsDisConnected = true;
            CommunicationState = "DisConnected";
            Channels.Clear();
            return IsDisConnected;
        }

        /// <summary>
        /// 设备连接
        /// </summary>
        /// <returns>true:连接成功; false:连接失败</returns>
        public bool Connect()
        {
            bool IsConnected = false;
            client = new ModbusTcpClient();
            if (client.Connect(Address, DefaultPort))
            {
                IsConnected = true;
                CommunicationState = "Connected";
            }
            else
            {
                Console.WriteLine("连接失败");
                return false;
            }
            InitChannel();
            return IsConnected;
        }

        /// <summary>
        /// 接受信息
        /// </summary>
        /// <returns>接收到的信息</returns>
        /// <exception cref="NotImplementedException"></exception>
        public byte[] ReceiveMsg()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 心跳机制
        /// </summary>
        /// <returns>返回false代表连接断开</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool KeepAlive()
        {
            throw new NotImplementedException();
        }
    }
}
