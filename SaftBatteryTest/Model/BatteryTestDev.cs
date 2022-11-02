using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SaftBatteryTest.Model
{
    public class BatteryTestDev : ObservableObject
    {
        public List<ChannelModel> Channels { get; set; }
        public BitmapSource Image { get; set; }
        public string Address { get; set; }

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
            CommunicationState = "Disconnected";
        }

        /// <summary>
        /// 通过协议初始化InitChannel
        /// </summary>
        public void InitChannel()
        {

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
        /// 设备连接
        /// </summary>
        /// <returns>true:连接成功; false:连接失败</returns>
        public bool Connect()
        {
            bool IsConnected = false;
            //! 这里默认连接成功
            IsConnected = true;
            CommunicationState = "Connected";
            InitChannel(4);
            return IsConnected;
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
    }
}
