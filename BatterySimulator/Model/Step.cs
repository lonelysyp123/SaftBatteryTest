using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatterySimulator.Model
{
    public class Step
    {
        public int ID;
        public WorkMode Mode;
        public string GGS;
        public string MTV;
        public string RC;
        public string Param1;
        public string Param2;
        public DateTime StopTime;
        public double StopVoltage;
        public double StopElectric;
        public double StopTemperature;
        public double StopCap;
        public double StopEnergy;
        public double RSOC;
        public double Offset;
        public int NextStep;

        public Step() { }

        public Step(int id, UInt16[] ushorts)
        {
            ID = id;
            Mode = (WorkMode)ushorts[0];
            Param1 = ushorts[1].ToString();
            Param2 = ushorts[2].ToString();
            StopTime = ushort2DateTime(ushorts[3]);
            StopVoltage = ushorts[4];
            StopElectric = ushorts[5];
            StopTemperature = ushorts[6];
            StopCap = ushorts[7];
            StopEnergy = ushorts[8];
            NextStep = ushorts[9];
        }

        private DateTime ushort2DateTime(ushort mins)
        {
            int h = mins / 60;
            int m = mins % 60;
            DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, h, m, 0);
            return endTime;
        }
    }

    public enum WorkMode
    {
        Stand,
        Charge_CC,
        Discharge_CD,
        Stop,
        Goto,
    }
}
