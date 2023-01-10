using SaftBatteryTest.Controls;
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
        public DataAnalysisView(string obj)
        {
            InitializeComponent();

            AddData();
        }

        private void AddData()
        {
            DataControl data = new DataControl();

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
