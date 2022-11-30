using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class DevBase : ObservableObject
    {
        private string _communicationState;
        public string CommunicationState
        {
            get => _communicationState;
            set
            {
                SetProperty(ref _communicationState, value);
            }
        }

        public DevBase()
        {
            CommunicationState = "Disconnected";
        }
    }
}
