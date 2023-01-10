using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaftBatteryTest.Controls
{
    /// <summary>
    /// DataControl.xaml 的交互逻辑
    /// </summary>
    public partial class DataControl : UserControl
    {
        public DataControl()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            PlotModel model = new PlotModel();
            model.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Maximum = 100, Minimum = 10, Title = "Y1" });
            plot.Model = model;
        }
    }
}
