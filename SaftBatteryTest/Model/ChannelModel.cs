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

        public ChannelModel()
        {

        }
    }
}
