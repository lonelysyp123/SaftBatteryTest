using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaftBatteryTest.View;
using SaftBatteryTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Serialization;

namespace SaftBatteryTest.Model
{
    public class ChannelViewModel : ObservableObject
    {
        private string _title = "code:";
        public string Title
        {
            get => _title;
            set
            {
                SetProperty(ref _title, value);
            }
        }

        private int _channelBoxN;
        public int ChannelBoxN
        {
            get => _channelBoxN;
            set
            {
                SetProperty(ref _channelBoxN, value);
            }
        }

        public SolidColorBrush _numColor = new SolidColorBrush(Colors.Green);
        public SolidColorBrush NumColor
        {
            get => _numColor;
            set
            {
                SetProperty(ref _numColor, value);
            }
        }

        public SolidColorBrush _chColor = new SolidColorBrush(Colors.Transparent);
        public SolidColorBrush ChColor
        {
            get => _chColor;
            set
            {
                SetProperty(ref _chColor, value);
            }
        }

        private bool _startIsEnabled = true;
        public bool StartIsEnabled
        {
            get => _startIsEnabled;
            set
            {
                SetProperty(ref _startIsEnabled, value);
            }
        }

        private bool _stopIsEnabled = false;
        public bool StopIsEnabled
        {
            get => _stopIsEnabled;
            set
            {
                SetProperty(ref _stopIsEnabled, value);
            }
        }

        private bool _stepSetIsEnabled = false;
        public bool StepSetIsEnabled
        {
            get => _stepSetIsEnabled;
            set
            {
                SetProperty(ref _stepSetIsEnabled, value);
            }
        }

        private Visibility _isShowGrayScreen;
        public Visibility IsShowGrayScreen
        {
            get => _isShowGrayScreen;
            set
            {
                SetProperty(ref _isShowGrayScreen, value);
            }
        }

        private double _vol;
        public double Vol
        {
            get => _vol;
            set
            {
                SetProperty(ref _vol, value);
            }
        }

        private double _elc;
        public double Elc
        {
            get => _elc;
            set
            {
                SetProperty(ref _elc, value);
            }
        }

        public RelayCommand StartChannelCommand { get; set; }
        public RelayCommand StopChannelCommand { get; set; }
        public RelayCommand StepSetCommand { get; set; }
        public RelayCommand OpenDataCommand { get; set; }

        private StepSettingViewModel Stepviewmodel;
        public bool IsSelected = false;
        private StoreModel store;

        public ChannelViewModel()
        {
            StartChannelCommand = new RelayCommand(StartChannel);
            StopChannelCommand = new RelayCommand(StopChannel);
            StepSetCommand = new RelayCommand(StepSet);
            OpenDataCommand = new RelayCommand(OpenData);

            Stepviewmodel = new StepSettingViewModel();
            store = new StoreModel();
        }

        private void OpenData()
        {
            DataAnalysisView view = new DataAnalysisView(store, Stepviewmodel);
            view.Show();
        }

        private void StepSet()
        {
            // 启动时修改通道对应参数
            ShowSetView();
        }

        public void StopChannel()
        {
            Console.WriteLine("关闭通道");
            // 物理停止设备通道
            ModbusTcpClient.GetInstance().WriteCtrlDev(ChannelBoxN, 4);
            // 结束读取实时数据线程
            StartIsEnabled = true;
            StopIsEnabled = false;
            StepSetIsEnabled = false;
            IsShowGrayScreen = Visibility.Visible;
        }

        public void StartChannel()
        {
            if (StartChannelView())
            {
                // 将Step中的信息写入设备
                for (int i = 0; i < Stepviewmodel.StepList.Count; i++)
                {
                    ModbusTcpClient.GetInstance().WriteStep(ChannelBoxN, i, Stepviewmodel.StepList[i]);
                }

                // 物理启动设备通道
                ModbusTcpClient.GetInstance().WriteCtrlDev(ChannelBoxN, 1);

                Thread daqth = new Thread(DaqTh);
                daqth.IsBackground = true;
                daqth.Start();
            }
        }

        /// <summary>
        /// 界面内容更新
        /// </summary>
        /// <returns>true:启动；false:取消</returns>
        public bool StartChannelView()
        {
            // 1.打开启动配置界面 2.启动读取实时数据线程
            if (ShowSetView())
            {
                IsShowGrayScreen = Visibility.Collapsed;
                StartIsEnabled = false;
                StopIsEnabled = true;
                StepSetIsEnabled = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 采集子线程
        /// </summary>
        private void DaqTh()
        {
            while (StartIsEnabled == false)
            {
                Thread.Sleep(1000);

                double tmp1 = ReadCurrVol(ChannelBoxN - 1);
                Vol = tmp1;
                store.VolCollect.Add(tmp1);

                double tmp2 = ReadCurrElc(ChannelBoxN - 1);
                Elc = tmp2;
                store.ElcCollect.Add(tmp2);
            }
        }

        /// <summary>
        /// 显示设置界面
        /// </summary>
        /// <returns>true:启动；false:取消</returns>
        public bool ShowSetView()
        {
            StepSettingView view = new StepSettingView(Stepviewmodel);
            if (view.ShowDialog() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取实时电压
        /// </summary>
        /// <returns>电压值(0.001V)</returns>
        private double ReadCurrVol(int ChIndex)
        {
            int address = 2000 + ChIndex * 100 + 0;
            int vol = BitConverter.ToInt32(ModbusTcpClient.GetInstance().ReadFunc((ushort)address, 2), 0);
            return vol * 0.001;
        }

        /// <summary>
        /// 获取实时电流
        /// </summary>
        /// <returns>电流值(0.001A)</returns>
        private double ReadCurrElc(int ChIndex)
        {
            int address = 2000 + ChIndex * 100 + 2;
            int elc = BitConverter.ToInt32(ModbusTcpClient.GetInstance().ReadFunc((ushort)address, 2), 0);
            return elc * 0.001;
        }
    }
}
