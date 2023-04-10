using BatterySimulator.Base;
using BatterySimulator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BatterySimulator
{
    public class TestBattery : BatteryBase
    {
        public bool IsQuit = false;
        public TestBattery(double vol, double elc, double capacity)
            :base(vol, elc, capacity) 
        {
            
        }

        // 电池充电50s
        public override void Charge(DateTime EndTime)
        {
            Console.WriteLine("TestBattery Charge");
            DateTime startTime = DateTime.Now;
            while(DateTime.Now < EndTime)
            {
                if(IsQuit)
                {
                    break;
                }
                
                if(CurrVol < Vol) 
                {
                    CurrCapacity = CurrCapacity + 0.001;
                    CurrVol = CurrVol + 0.001;
                }
                Thread.Sleep(1000);
            }
        }

        public override void Discharge(DateTime EndTime)
        {
            Console.WriteLine("TestBattery Discharge");
            DateTime startTime = DateTime.Now;
            while (DateTime.Now < EndTime)
            {
                if (IsQuit)
                {
                    break;
                }

                if (CurrVol > 9.0)
                {
                    CurrCapacity = CurrCapacity - 0.001;
                    CurrVol = CurrVol - 0.001;
                }
                Thread.Sleep(1000);
            }
        }

        public override void Standing(DateTime EndTime)
        {
            Console.WriteLine("TestBattery Standing");
            while (DateTime.Now < EndTime)
            {
                if (IsQuit)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
