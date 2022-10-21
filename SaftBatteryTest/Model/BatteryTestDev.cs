using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SaftBatteryTest.Model
{
    public class BatteryTestDev
    {
        public List<ChannelModel> Channels { get; set; }
        public BitmapSource Image { get; set; }
        public string Address { get; set; }
        public string CommunicationState { get; set; }

        public BatteryTestDev()
        {

        }
    }
}
