using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaftBatteryTest.View;
using SaftBatteryTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace SaftBatteryTest.Model
{
    public class ChannelModel : ObservableObject
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

        public RelayCommand StartChannelCommand { get; set; }
        public RelayCommand StopChannelCommand { get; set; }
        public RelayCommand StepSetCommand { get; set; }

        private StepSettingViewModel viewModel;
        public bool IsSelected = false;

        public ChannelModel()
        {
            StartChannelCommand = new RelayCommand(StartChannel);
            StopChannelCommand = new RelayCommand(StopChannel);
            StepSetCommand = new RelayCommand(StepSet);

            viewModel = new StepSettingViewModel();
        }

        private void StepSet()
        {
            // TODO 启动时修改通道对应参数
            ShowSetView();
        }

        public void StopChannel()
        {
            Console.WriteLine("关闭通道");

            // TODO 结束读取实时数据线程

            IsShowGrayScreen = Visibility.Visible;
            StartIsEnabled = true;
            StopIsEnabled = false;
            StepSetIsEnabled = false;
        }

        public void StartChannel()
        {
            // TODO 1.打开启动配置界面 2.启动读取实时数据线程
            if (ShowSetView())
            {
                IsShowGrayScreen = Visibility.Collapsed;
                StartIsEnabled = false;
                StopIsEnabled = true;
                StepSetIsEnabled = true;
            }
        }

        public bool ShowSetView()
        {
            StepSettingView view = new StepSettingView(viewModel);
            if (view.ShowDialog() == true)
            {
                // TODO 启动线程，持续采集数据
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
