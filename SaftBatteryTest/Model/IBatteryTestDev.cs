using SaftBatteryTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Helper
{
    public interface IBatteryTestDev
    {
        bool DevOnline();                                           // 设备上线
        bool DevOffline();                                          // 设备下线

        string ReadIntSwVersion();                                  // 读取软件版本号
        int ReadChNums();                                           // 读取通道数

        double ReadCurrVol(int ChIndex);                            // 读取实时电压
        double ReadCurrElc(int ChIndex);                            // 读取实时电流

        void WriteWorkMode(int ChIndex, int ID, WorkMode Mode);     // 写入工作模式
        void WriteNextStep(int ChIndex, int ID, int NextS);         // 写入下一工步号
    }
}
