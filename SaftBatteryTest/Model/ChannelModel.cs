using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class ChannelModel : ObservableObject
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                SetProperty(ref _title, value);
            }
        }
        public ChannelModel()
        {
            Title = "code:";
        }
    }
}
