using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class StoreModel
    {
        public BlockingCollection<double> VolCollect;
        public BlockingCollection<double> ElcCollect;

        public StoreModel()
        {
            VolCollect = new BlockingCollection<double>();
            ElcCollect = new BlockingCollection<double>();
        }
    }
}
