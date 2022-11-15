using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class DataRecordModel
    {
        public bool IsEnableOfSpan { get; set; }
        public double Span { get; set; }
        public bool IsEnableOfVolRange { get; set; }
        public double VolRange { get; set; }
        public bool IsEnableOfElcRange { get; set; }
        public double ElcRange { get; set; }
    }
}
