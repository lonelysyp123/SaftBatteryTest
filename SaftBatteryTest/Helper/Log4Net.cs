using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Helper
{
    public static class Log4Net
    {
        public static ILog Log()
        {
            //log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            return LogManager.GetLogger("InfoLogger");
        }
    }
}
