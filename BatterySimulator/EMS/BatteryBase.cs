using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BatterySimulator.EMS
{
    public class BatteryBase
    {
        public double Voltage { get; set; }
        public double Current { get; set; }

        public int BatteryID { get; set; }
        public BatteryBase(int ID)
        {
            BatteryID = ID;
            Voltage = 10;
            Current = 11;
        }

        public void Run()
        {
            Thread thread = new Thread(() => {
                while (true)
                {
                    Voltage = 11;
                    Current = 12;
                    Thread.Sleep(5000);
                    Voltage = 10;
                    Current = 11;
                    Thread.Sleep(5000);
                }
            });
            thread.Start();
        }
    }
}
