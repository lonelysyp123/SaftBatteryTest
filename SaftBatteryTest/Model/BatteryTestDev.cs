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

namespace SaftBatteryTest.Model
{
    public class BatteryTestDev : ModbusTcpClient, IBatteryTestDev
    {
        // MBAP 头描述 {|传输标志(2byte)|协议标志(2byte)|长度(2byte)|单元号标志(1byte)|}
        // 设备信息
        private byte[] IntSwVersion =   { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x03, 0xe8, 0x00, 0x02 };
        private byte[] ExtSwVersion =   { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x03, 0xea, 0x00, 0x01 };
        private byte[] ChNums =         { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x03, 0xf5, 0x00, 0x01 };

        // 实时数据
        private byte[] CurrVol =        { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x07, 0xd0, 0x00, 0x02 };
        private byte[] CurrElc =        { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x07, 0xd2, 0x00, 0x02 };

        public List<ChannelModel> Channels { get; set; }
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
            int count = ReadChNums();
            Channels = new List<ChannelModel>();
            for (int i = 0; i < count; i++)
            {
                Channels.Add(new ChannelModel() { ChannelBoxN = i+1 });
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
        public bool DevOffline()
        {
            if (DisConnect())
            {
                Channels.Clear();
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
            if (Connect(Address, DefaultPort))
            {
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
            string Version = (string)ReadFunc(IntSwVersion, "String");
            return Version;
        }

        /// <summary>
        /// 获取通道数量
        /// </summary>
        /// <returns>通道数量</returns>
        public int ReadChNums()
        {
            int Nums = (int)ReadFunc(ChNums, "UInt16");
            return Nums;
        }

        /// <summary>
        /// 获取实时电压
        /// </summary>
        /// <returns>电压值(0.001V)</returns>
        public double ReadCurrVol(int ChIndex)
        {
            int Vol = (int)ReadFunc(ChIndex, CurrVol, "UInt32");
            return Vol * 0.001;
        }

        /// <summary>
        /// 获取实时电流
        /// </summary>
        /// <returns>电流值(0.001A)</returns>
        public double ReadCurrElc(int ChIndex)
        {
            int Elc = (int)ReadFunc(ChIndex, CurrElc, "UInt32");
            return Elc * 0.001;
        }

        /// <summary>
        /// 写入指定通道，指定工步的工作模式
        /// </summary>
        /// <param name="ChIndex">指定通道</param>
        /// <param name="ID">指定工步</param>
        /// <param name="Mode">工作模式</param>
        public void WriteWorkMode(int ChIndex, int ID, WorkMode Mode)
        {
            WriteFunc(ChIndex, ID, (int)Mode);
        }

        /// <summary>
        /// 写入指定通道，指定工步的下一步号
        /// </summary>
        /// <param name="ChIndex">指定通道</param>
        /// <param name="ID">指定工步</param>
        /// <param name="NextS">下一步号</param>
        public void WriteNextStep(int ChIndex, int ID, int NextS)
        {
            WriteFunc(ChIndex, ID, NextS);
        }

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

        public void OpenChannelSetView()
        {
            if (Index != -1)
            {
                //Channels[Index].StartDev();
            }
            else
            {
                MessageBox.Show("未选中任何通道!");
            }
        }
    }
}
