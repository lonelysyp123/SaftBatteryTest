using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BatterySimulator.EMS
{
    public class BatteryTotalBase
    {
        public double Voltage { get; set; }
        public double Current { get; set; }
        public List<BatterySeriesBase> Series { get; set; }

        public int TotalID { get; set; }
        public BatteryTotalBase(int ID)
        {
            TotalID = ID;
            Series = new List<BatterySeriesBase>();
            Voltage = 30;
            Current = 31;
        }

        public BatteryTotalBase()
        {

        }

        public void Run()
        {
            Thread thread = new Thread(() => {
                while (true)
                {
                    Voltage = 31;
                    Current = 32;
                    Thread.Sleep(5000);
                    Voltage = 30;
                    Current = 31;
                    Thread.Sleep(5000);
                }
            });
            thread.Start();
        }
    }
}
