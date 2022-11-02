using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaftBatteryTest.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class AppStateViewModel : ObservableObject
    {
        private string _voltage = "侦测电压";
        public string Voltage
        {
            get => _voltage;
            set
            {
                SetProperty(ref _voltage, value);
            }
        }

        private string _mtvOnline = "MTV联机结果:";
        public string MTVOnline
        {
            get => _mtvOnline;
            set
            {
                SetProperty(ref _mtvOnline, value);
            }
        }

        private string _autoOnline = "自动联机结果:";
        public string AutoOnline
        {
            get => _autoOnline;
            set
            {
                SetProperty(ref _autoOnline, value);
            }
        }

        private string _userName = "[未登录]";
        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
            }
        }

        private string _currentTime = "00:00:00";
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                SetProperty(ref _currentTime, value);
            }
        }

        public AppStateViewModel()
        {
            Thread thread = new Thread(ShowTime);
            thread.IsBackground = true;
            thread.Start();
        }

        private bool IsRun = true;
        private void ShowTime()
        {
            while (IsRun)
            {
                Thread.Sleep(1000);
                CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            }
        }
    }
}
