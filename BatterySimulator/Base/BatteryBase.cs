using BatterySimulator.Interface;
using BatterySimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatterySimulator.Base
{
    public class BatteryBase : IBattery
    {
        public double Vol;//V
        public double Res;
        public double Elc;//A
        public double Temperature = 24;
        public double SOC;
        public double Capacity;//AH

        public BatteryBase(double vol, double elc, double capacity) 
        { 
            Vol = vol;
            Elc = elc;
            Res = Vol / Elc;
            Capacity = capacity;
        }

        public virtual void Charge(MyToken token, DateTime EndTime)
        {
            Console.WriteLine("BatteryBase Charge");
        }

        public virtual void Discharge(MyToken token, DateTime EndTime)
        {
            Console.WriteLine("BatteryBase Discharge");
        }

        public virtual void Standing(MyToken token, DateTime EndTime)
        {
            Console.WriteLine("BatteryBase Standing");
        }
    }
}
