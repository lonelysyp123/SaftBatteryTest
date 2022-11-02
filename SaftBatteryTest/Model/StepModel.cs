using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class StepModel
    {
        public int ID { get; set; }
        public WorkMode Mode { get; set; }
        public string GGS { get; set; }
        public string MTV { get; set; }
        public string RC { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public double StopTime { get; set; }
        public double StopVoltage { get; set; }
        public double StopElectric { get; set; }
        public double StopT { get; set; }
        public double StopCap { get; set; }
        public double StopEnergy { get; set; }
        public double RSOC { get; set; }
        public double Offset { get; set; }
        public string NextStep { get; set; }

        public StepModel()
        {

        }
    }

    public enum WorkMode
    {
        静置,
        恒流充电CC,
        恒压充电CV,
        恒流恒压充电CCCV,
        浮充充电FV,
        恒功充电CP,
        恒流放电CD,
        恒功放电CP,
        恒电阻放电,
        停止,
        跳转,
        RC模式,
    }
}
