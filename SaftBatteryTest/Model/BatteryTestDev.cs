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
using System.Windows.Media;
using System.Windows;
using System.Threading;

namespace SaftBatteryTest.Model
{
    public class BatteryTestDev : DevBase, IBatteryTestDev
    {
        public List<ChannelViewModel> Channels { get; set; }
        public BitmapSource Image { get; set; }
        public string Address { get; set; }
        public int DefaultPort { get; set; }
        private int Index = -1;
        private ModbusTcpClient client;

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
        }

        /// <summary>
        /// 通过协议初始化InitChannel
        /// </summary>
        public void InitChannel()
        {
            Index = -1;
            int count = ReadChNums();
            Channels = new List<ChannelViewModel>();
            for (int i = 0; i < count; i++)
            {
                Channels.Add(new ChannelViewModel() { ChannelBoxN = i+1 });
            }
        }

        /// <summary>
        /// 通过协议初始化InitChannel(测试用)
        /// </summary>
        /// <param name="count">通道数</param>
        public void InitChannel(int count)
        {
            // 这里默认创建4个
            Channels = new List<ChannelViewModel>();
            for (int i = 0; i < count; i++)
            {
                Channels.Add(new ChannelViewModel());
            }
            Channels[0].Title += Address;
        }

        /// <summary>
        /// 设备断开连接
        /// </summary>
        /// <returns>true:成功; false:失败</returns>
        public bool DevOffline()
        {
            if (client != null)
            {
                if (client.Disconnect())
                {
                    Channels.Clear();
                    CommunicationState = "DisConnected";
                    return true;
                }
                else
                {
                    Console.WriteLine("断开连接失败");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 设备连接
        /// </summary>
        /// <returns>true:连接成功; false:连接失败</returns>
        public bool DevOnline()
        {
            client = ModbusTcpClient.GetInstance();
            client.IP = Address;
            client.Port = DefaultPort;
            if (client.Connect())
            {
                CommunicationState = "Connected";
                return true;
            }
            else
            {
                Console.WriteLine("连接失败");
                return false;
            }
        }

        /// <summary>
        /// 获取软件版本号
        /// </summary>
        /// <returns>版本号</returns>
        public string ReadIntSwVersion()
        {
            string Version = BitConverter.ToString(client.ReadFunc(1000,1));
            return Version;
        }

        /// <summary>
        /// 获取通道数量
        /// </summary>
        /// <returns>通道数量</returns>
        public int ReadChNums()
        {
            int Nums = BitConverter.ToUInt16(client.ReadFunc(1013,1),0);
            return Nums;
        }

        /// <summary>
        /// 选择通道
        /// </summary>
        /// <param name="index">通道号</param>
        public void SelectChannel(int index)
        {
            Index = index;
            for (int i = 0; i < Channels.Count; i++)
            {
                if (Channels[i].ChannelBoxN == index)
                {
                    Channels[i].ChColor = new SolidColorBrush(Colors.LightGreen);
                }
                else
                {
                    Channels[i].ChColor = new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        /// <summary>
        /// 开始采集数据
        /// </summary>
        public void StartDaq()
        {
            if (Index != -1)
            {
                Channels[Index - 1].StartChannel();
            }
            else
            {
                MessageBox.Show("未选中任何通道!");
            }
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        public void StopDaq()
        {
            if (Index != -1)
            {
                Channels[Index - 1].StopChannel();
            }
            else
            {
                MessageBox.Show("未选中任何通道!");
            }
        }
    }
}
