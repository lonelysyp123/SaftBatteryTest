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
    /// AddIPsView.xaml 的交互逻辑
    /// </summary>
    public partial class AddIPsView : Window
    {
        public AddIPsView()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (IP1.P1 == IP2.P1 && IP1.P2 == IP2.P2 && IP1.P3 == IP2.P3)
            {
                int beforeN = 0;
                int.TryParse(IP1.P4.Text, out beforeN);
                int afterN = 0;
                int.TryParse(IP2.P4.Text, out afterN);
                if (afterN < beforeN)
                {
                    MessageBox.Show("IP地址输入有误!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("IP地址输入有误!");
                return;
            }
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
