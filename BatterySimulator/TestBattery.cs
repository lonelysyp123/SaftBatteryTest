using BatterySimulator.Base;
using BatterySimulator.Interface;
using BatterySimulator.Model;
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
        public TestBattery(double vol, double elc, double capacity)
            :base(vol, elc, capacity) 
        {
            
        }

        public override string ToString()
        {
            return "模拟电池，电压：" + Vol.ToString() + " 电流：" + Elc.ToString() + " 容量：" + Capacity.ToString();
        }

        // 电池充电
        public override void Charge(MyToken token, DateTime EndTime)
        {
            Console.WriteLine("TestBattery Charge");
            DateTime startTime = DateTime.Now;
            while(DateTime.Now < EndTime)
            {
                if(token.IsCancel)
                {
                    break;
                }

                while(token.IsPause)
                {
                    Thread.Sleep(200);
                }
                
                if(Vol < 12.0) 
                {
                    Capacity = Capacity + 0.001;
                    Vol = Vol + 0.001;
                }
                Thread.Sleep(1000);
            }
        }

        public override void Discharge(MyToken token, DateTime EndTime)
        {
            Console.WriteLine("TestBattery Discharge");
            DateTime startTime = DateTime.Now;
            while (DateTime.Now < EndTime)
            {
                if (token.IsCancel)
                {
                    break;
                }

                while (token.IsPause)
                {
                    Thread.Sleep(200);
                }

                if (Vol > 9.0)
                {
                    Capacity = Capacity - 0.001;
                    Vol = Vol - 0.001;
                }
                Thread.Sleep(1000);
            }
        }

        public override void Standing(MyToken token, DateTime EndTime)
        {
            Console.WriteLine("TestBattery Standing");
            while (DateTime.Now < EndTime)
            {
                if (token.IsCancel)
                {
                    break;
                }

                Thread.Sleep(1000);
            }
        }
    }
}
