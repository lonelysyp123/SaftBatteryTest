using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BatterySimulator.Model
{
    public class MyToken
    {
        public bool IsCancel = false;
        public bool IsPause = false;
        public MyToken() { }

        public void Cancel()
        {
            IsCancel = true;
        }

        public void Pause()
        {
            IsPause = true;
        }

        public void Continue()
        {
            IsPause = false;
        }
    }
}
