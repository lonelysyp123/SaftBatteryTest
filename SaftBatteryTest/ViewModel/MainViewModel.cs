using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SaftBatteryTest.Helper;
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

        public RelayCommand SetAutoOnlineCommand { get; set; }
        public RelayCommand OpenFileCommand { get; set; }
        public RelayCommand SetSavePathCommand { get; set; }
        public RelayCommand StepModifyCommand { get; set; }
        public RelayCommand AddIPCommand { get; set; }
        public RelayCommand AddIPsCommand { get; set; }

        public AppStateViewModel AppState { get; set; }

        public MainViewModel()
        {
            SetAutoOnlineCommand = new RelayCommand(SetAutoOnline);
            OpenFileCommand = new RelayCommand(OpenFile);
            SetSavePathCommand = new RelayCommand(SetSavePath);
            StepModifyCommand = new RelayCommand(StepModify);
            AddIPCommand = new RelayCommand(AddIP);
            AddIPsCommand = new RelayCommand(AddIPs);

            DevList = new ObservableCollection<BatteryTestDev>();
            AppState = new AppStateViewModel();

            //Log4Net.Log().Info("123");
            //Log4Net.Log().Error("321");
        }

        private void AddIPs()
        {
            AddIPsView view = new AddIPsView();
            //! 根据配置文件来设置网段
            view.IP1.SetAddressText("192.168.0.1");
            view.IP2.SetAddressText("192.168.0.3");
            if (view.ShowDialog() == true)
            {
                string ip = view.IP1.AddressText;
                //! 判断该IP是否存在
                var objs = DevList.Where(dev => dev.Address == ip).ToList();
                if (objs.Count == 0)
                {
                    //! 界面上新增IP
                    AddIPInView(ip);
                    //! TODO 配置文件中新增IP
                }
            }
        }

        private void AddIP()
        {
            AddIPView view = new AddIPView();
            if (view.ShowDialog() == true)
            {
                string ip = view.IPText.AddressText;
                //! 判断该IP是否存在
                var objs = DevList.Where(dev => dev.Address == ip).ToList();
                if (objs.Count == 0)
                {
                    //! 界面上新增IP
                    AddIPInView(ip);
                    //! TODO 配置文件中新增IP
                }
            }
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

        private void AddIPInView(string ip)
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
                Address = ip,
                CommunicationState = "Disconnected"
            };

            DevList.Add(dev);
        }
    }
}
