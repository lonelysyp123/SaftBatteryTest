using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class StepSafeModel
    {
        public double VoltageMin { get; set; }
        public double VoltageMax { get; set; }
        public double ElcRange { get; set; }
        public double TemperatureMax { get; set; }
        public double TemperatureMin { get; set; }
    }
}
