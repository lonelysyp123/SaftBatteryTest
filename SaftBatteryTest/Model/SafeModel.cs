using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class SafeModel
    {
        public double VolMax { get; set; }
        public double VolMin { get; set; }
        public double ElcRange { get; set; }
        public double TemperatureMax { get; set; }
        public double TemperatureMin { get; set; }

        public SafeModel()
        {

        }
    }
}
