using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using SaftBatteryTest.Model;
using SaftBatteryTest.ViewModel;
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
        }

        public void InitStore(StoreModel storeModel)
        {
            PlotView plot = new PlotView();
            plot.Height = this.ActualHeight / 1;
            PlotModel model = new PlotModel();
            //model.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Maximum = 100, Minimum = 0, Title = "Y1" });
            //model.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Maximum = 100, Minimum = 0, Title = "X1" });

            var lineSend = new LineSeries() { Title = "电压", BrokenLineColor = OxyColors.Green };
            for (int i = 0; i < storeModel.VolCollect.Count; i++)
            {
                lineSend.Points.Add(new DataPoint((storeModel.VolCollect.Count - i) * 0.1, storeModel.VolCollect.Take()));
            }
            model.Series.Add(lineSend);

            plot.Model = model;
            Grid.SetRow(plot, 0);

            DataG.Children.Add(plot);
        }

        public void InitStep(StepSettingViewModel stepSettingViewModel)
        {

        }
    }
}
