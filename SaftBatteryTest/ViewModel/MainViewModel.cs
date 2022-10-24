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

        public MainViewModel()
        {
            TestCommand = new RelayCommand(() => Test());
            SetAutoOnlineCommand = new RelayCommand(SetAutoOnline);
            OpenFileCommand = new RelayCommand(OpenFile);

            DevList = new ObservableCollection<BatteryTestDev>();
        }

        private void OpenFile()
        {
            
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
