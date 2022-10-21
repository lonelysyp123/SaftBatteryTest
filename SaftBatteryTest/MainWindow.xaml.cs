using SaftBatteryTest.Model;
using SaftBatteryTest.View;
using SaftBatteryTest.View.Controls;
using SaftBatteryTest.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaftBatteryTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewmodel;
        public MainWindow()
        {
            InitializeComponent();

            viewmodel = new MainViewModel();
            this.DataContext = viewmodel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Channel channel1 = new Channel(viewmodel.Body);

            //Border border1 = new Border();
            ////border1.BorderThickness = new Thickness(3);
            ////border1.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 184, 0));
            //border1.CornerRadius = new CornerRadius(10);
            //border1.Height = 180;
            //border1.Width = 200;
            //border1.Margin = new Thickness(7, 5, 7, 5);
            //border1.Child = channel1;

            //StackPanel panel = new StackPanel();
            //panel.Orientation = Orientation.Horizontal;
            //panel.Children.Add(border1);

            //Border border = new Border();
            //border.BorderThickness = new Thickness(3);
            //border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 218, 250, 240));
            //border.CornerRadius = new CornerRadius(10);
            //border.Height = 200;
            //border.VerticalAlignment = VerticalAlignment.Top;
            //border.Margin = new Thickness(7, 5, 7, 5);
            //border.Child = panel;


            ////Channel channel = new Channel(new ChannelModel());
            ////Grid.SetRow(channel, 1);
            ////Grid.SetColumn(channel, 1);

            //Body.Children.Add(border);

            //PathSettingView view = new PathSettingView();
            //view.Show();
        }

        private void ListBox_Selected(object sender, RoutedEventArgs e)
        {
            ListBoxItem lbi = e.Source as ListBoxItem;
            lbi.FontSize = 14;
            lbi.Background = new SolidColorBrush(Color.FromArgb(150, 76, 255, 0));
        }
    }
}
