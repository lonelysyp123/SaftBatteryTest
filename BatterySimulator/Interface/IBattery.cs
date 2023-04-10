using BatterySimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatterySimulator.Interface
{
    public interface IBattery
    {
        /// <summary>
        /// 充电
        /// </summary>
        void Charge(MyToken token, DateTime EndTime);

        /// <summary>
        /// 放电
        /// </summary>
        void Discharge(MyToken token, DateTime EndTime);

        /// <summary>
        /// 静置
        /// </summary>
        void Standing(MyToken token, DateTime EndTime);
    }
}
