using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaftBatteryTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private ChannelModel _body;
        public ChannelModel Body
        {
            get => _body;
            set
            {
                SetProperty<ChannelModel>(ref _body, value);
            }
        }

        public RelayCommand TestCommand { get; set; }

        public MainViewModel()
        {
            TestCommand = new RelayCommand(() => Test());
        }

        private void Test()
        {
            Body = new ChannelModel();
            Body.Title = "1231234";
        }
    }
}
