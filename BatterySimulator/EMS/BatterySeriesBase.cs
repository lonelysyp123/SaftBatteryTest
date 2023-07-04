using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BatterySimulator.EMS
{
    public class BatterySeriesBase
    {
        public double Voltage { get; set; }
        public double Current { get; set; }
        public List<BatteryBase> Batteries { get; set; }

        public int SeriesID { get; set; }
        public BatterySeriesBase(int ID)
        {
            SeriesID = ID;
            Batteries = new List<BatteryBase>();
            Voltage = 20;
            Current = 21;
        }

        public void Run()
        {
            Thread thread = new Thread(() => {
                while (true)
                {
                    Voltage = 21;
                    Current = 22;
                    Thread.Sleep(5000);
                    Voltage = 20;
                    Current = 21;
                    Thread.Sleep(5000);
                }
            });
            thread.Start();
        }
    }
}
