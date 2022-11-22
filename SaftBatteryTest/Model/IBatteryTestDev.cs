using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Helper
{
    public interface IBatteryTestDev
    {
        bool Connect();
        bool DisConnect();
        void SendMsg(byte[] msg);
        byte[] ReceiveMsg();
        bool KeepAlive();
    }
}
