using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private string _channelBoxN = "0-0";
        public string ChannelBoxN
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

        public ChannelModel()
        {

        }
    }
}
