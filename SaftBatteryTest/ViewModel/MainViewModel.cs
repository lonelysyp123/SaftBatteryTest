using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SaftBatteryTest.Model;
using SaftBatteryTest.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SaftBatteryTest.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private ObservableCollection<BatteryTestDev> _devList;
        public ObservableCollection<BatteryTestDev> DevList
        {
            get => _devList;
            set
            {
                SetProperty(ref _devList, value);
            }
        }

        public RelayCommand TestCommand { get; set; }
        public RelayCommand SetAutoOnlineCommand { get; set; }
        public RelayCommand OpenFileCommand { get; set; }
        public RelayCommand SetSavePathCommand { get; set; }
        public RelayCommand StepModifyCommand { get; set; }

        public MainViewModel()
        {
            TestCommand = new RelayCommand(() => Test());
            SetAutoOnlineCommand = new RelayCommand(SetAutoOnline);
            OpenFileCommand = new RelayCommand(OpenFile);
            SetSavePathCommand = new RelayCommand(SetSavePath);
            StepModifyCommand = new RelayCommand(StepModify);

            DevList = new ObservableCollection<BatteryTestDev>();
        }

        private void StepModify()
        {
            StepSettingView view = new StepSettingView();
            view.ShowDialog();
        }

        private void SetSavePath()
        {
            PathSettingView view = new PathSettingView();
            if (view.ShowDialog() == true)
            {
                //! TODO 保存这个地址到本地的配置文件
            }
        }

        private void OpenFile()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "数据文件|*.cds";
            file.ShowDialog();
            if (file.FileName != null && file.FileName != "")
            {
                //! 加载文档，根据规则生成数据
            }
        }

        private void SetAutoOnline()
        {
            AutoOnLineView view = new AutoOnLineView();
            view.ShowDialog();
        }

        private void Test()
        {
            DirectoryInfo directory = new DirectoryInfo("./Resource/Image");
            FileInfo[] files = directory.GetFiles("PC.png");

            BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(files[0].FullName, UriKind.Absolute);
                bi.EndInit();

                BatteryTestDev dev = new BatteryTestDev()
                {
                    Image = bi,
                    Address = "127.0.0.11111",
                    CommunicationState = "Connected"
                };

                DevList.Add(dev);
        }
    }
}
