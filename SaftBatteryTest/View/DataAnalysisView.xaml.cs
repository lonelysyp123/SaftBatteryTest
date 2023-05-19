using SaftBatteryTest.Controls;
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
using System.Windows.Shapes;

namespace SaftBatteryTest.View
{
    /// <summary>
    /// DataAnalysisView.xaml 的交互逻辑
    /// </summary>
    public partial class DataAnalysisView : Window
    {
        private DataAnalysisViewModel viewmodel;

        public DataAnalysisView(StoreModel storeModel, StepSettingViewModel stepSettingViewModel)
        {
            InitializeComponent();

            viewmodel = new DataAnalysisViewModel(storeModel, stepSettingViewModel);
            this.DataContext = viewmodel;

        }
    }
}
