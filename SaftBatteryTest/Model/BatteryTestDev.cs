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
        // MBAP 头描述 {|传输标志(2byte)|协议标志(2byte)|长度(2byte)|单元号标志(1byte)|}
        // 设备信息
        private byte[] IntSwVersion =   { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x03, 0xe8, 0x00, 0x02 };
        private byte[] ExtSwVersion =   { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x03, 0xea, 0x00, 0x01 };
        private byte[] ChNums =         { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x03, 0xf5, 0x00, 0x01 };

        public List<ChannelViewModel> Channels { get; set; }
        public BitmapSource Image { get; set; }
        public string Address { get; set; }
        public int DefaultPort { get; set; }
        private int Index = -1;

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
            if (ModbusTcpClient.Instance.DisConnect())
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

        /// <summary>
        /// 设备连接
        /// </summary>
        /// <returns>true:连接成功; false:连接失败</returns>
        public bool DevOnline()
        {
            if (ModbusTcpClient.Instance.Connect(Address, DefaultPort))
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
            string Version = (string)ModbusTcpClient.Instance.ReadFunc(IntSwVersion, "String");
            return Version;
        }

        /// <summary>
        /// 获取通道数量
        /// </summary>
        /// <returns>通道数量</returns>
        public int ReadChNums()
        {
            int Nums = (int)ModbusTcpClient.Instance.ReadFunc(ChNums, "UInt16");
            return Nums;
        }

        /// <summary>
        /// 写入指定通道，指定工步的工作模式
        /// </summary>
        /// <param name="ChIndex">指定通道</param>
        /// <param name="ID">指定工步</param>
        /// <param name="Mode">工作模式</param>
        public void WriteWorkMode(int ChIndex, int ID, WorkMode Mode)
        {
            ModbusTcpClient.Instance.WriteFunc(ChIndex, ID, (int)Mode);
        }

        /// <summary>
        /// 写入指定通道，指定工步的下一步号
        /// </summary>
        /// <param name="ChIndex">指定通道</param>
        /// <param name="ID">指定工步</param>
        /// <param name="NextS">下一步号</param>
        public void WriteNextStep(int ChIndex, int ID, int NextS)
        {
            ModbusTcpClient.Instance.WriteFunc(ChIndex, ID, NextS);
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
