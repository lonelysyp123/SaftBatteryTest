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
        private StoreModel storeModel;
        private StepSettingViewModel stepSettingViewModel;

        public DataAnalysisView(StoreModel storeModel, StepSettingViewModel stepSettingViewModel)
        {
            InitializeComponent();

            viewmodel = new DataAnalysisViewModel();
            this.DataContext = viewmodel;
            this.storeModel = storeModel;
            this.stepSettingViewModel = stepSettingViewModel;

            AddDataPage();
        }

        private void AddDataPage()
        {
            DataControl data = new DataControl();
            data.InitStore(storeModel);
            data.InitStep(stepSettingViewModel);

            TabItem item = new TabItem();
            item.Header = "123123.cds";
            item.Style = null;
            item.Content = data;
            DataTab.Items.Add(item);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("打开文件");
        }
    }
}
