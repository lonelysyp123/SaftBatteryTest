using BatterySimulator.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatterySimulator.Interface
{
    public interface IDev
    {
        /// <summary>
        /// 放入电池
        /// </summary>
        /// <param name="Ero">错误信息</param>
        /// <param name="ChannelIndex">通道号</param>
        /// <param name="Battery">电池实体</param>
        /// <returns></returns>
        bool InBattery(ref string Ero, int ChannelIndex, BatteryBase Battery);

        /// <summary>
        /// 取出电池
        /// </summary>
        /// <param name="Ero">错误信息</param>
        /// <param name="ChannelIndex">通道号</param>
        /// <returns></returns>
        bool OutBattery(ref string Ero, int ChannelIndex);

        /// <summary>
        /// 运行任务
        /// </summary>
        /// <param name="ChannelIndex">通道号</param>
        void Run(int ChannelIndex);
    }
}
