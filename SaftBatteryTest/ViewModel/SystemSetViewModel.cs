using SaftBatteryTest.Helper;
using SaftBatteryTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.ViewModel
{
    public class SystemSetViewModel
    {
        public SystemSetModel model;
        private SystemXmlHelper helper;

        public SystemSetViewModel(string Path)
        {
            helper = new SystemXmlHelper();
            InitConfig(Path);
        }

        private void InitConfig(string path)
        {
            if (path != null && path != "")
            {
                // 工程步骤更新
                model = helper.ReadSysParamFromXml(path);
            }
        }
    }
}
