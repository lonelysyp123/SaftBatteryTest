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
        void Charge(DateTime EndTime);

        /// <summary>
        /// 放电
        /// </summary>
        void Discharge(DateTime EndTime);

        /// <summary>
        /// 静置
        /// </summary>
        void Standing(DateTime EndTime);
    }
}
