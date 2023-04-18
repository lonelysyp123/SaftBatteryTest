using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatterySimulator.Base
{
    public class DevBase
    {
        
        protected BatteryBase[] Batteries;
        protected static readonly UInt16[] IntSwVersion = new UInt16[] { 10, 10 };
        protected static readonly UInt16 ExtSwVersion = 1;
        protected static readonly string ProjectCode = "Test01";           // ASSCII
        protected static readonly UInt16 Year = 2023;
        protected static readonly UInt16 Month = 4;
        protected static readonly UInt16 Day = 3;
        protected static readonly UInt16[] Sort = new UInt16[] { 0, 1 };
        protected static readonly UInt16 ChannelNums = 4;
        protected static readonly string Model = "V01";                 // ASSCII
        protected static readonly string ProductBarCode = "zxcvbn";       // ASSCII
        protected static readonly string ManufacturerName = "SYP";      // ASSCII
        protected static readonly UInt16 HardWareVersion_Major = 10;
        protected static readonly UInt16 HardWareVersion_Minor = 20;
        protected static readonly string PCBA_BarCode = "MCU";         // ASSCII

        public DevBase() 
        {
            Batteries = new BatteryBase[ChannelNums];
        }

        /// <summary>
        /// 启动设备
        /// </summary>
        /// <returns></returns>
        public virtual bool StartDev()
        {
            Console.WriteLine("StartDev");
            return true;
        }

        /// <summary>
        /// 暂停设备
        /// </summary>
        /// <returns></returns>
        public virtual bool StopDev()
        {
            Console.WriteLine("StopDev");
            return true;
        }
    }
}
