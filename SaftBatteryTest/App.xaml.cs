using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SaftBatteryTest
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    
    public partial class App : Application
    {
        public static log4net.ILog OperationLog = log4net.LogManager.GetLogger("OperationLog");
        public static log4net.ILog ErrorLog = log4net.LogManager.GetLogger("ErrorLog");
    }
}
