using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class AppStateModel : ObservableObject
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

        private string _userName = "[Admin]";
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

        public AppStateModel()
        {
            Thread thread = new Thread(ShowTime);
            thread.IsBackground = true;
            thread.Start();
        }

        private void ShowTime()
        {
            while (true)
            {
                Thread.Sleep(1000);
                CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            }
        }
    }
}
