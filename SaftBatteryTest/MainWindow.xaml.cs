using SaftBatteryTest.Model;
using SaftBatteryTest.View;
using SaftBatteryTest.Controls;
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
using MaterialDesignThemes.Wpf;

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
            StateContent.DataContext = viewmodel.AppStateVM;

            StateMenu.IsChecked = true;
            ShortcutMenu.IsChecked = true;
        }

        /// <summary>
        /// 根据设备的状态显示界面
        /// </summary>
        /// <param name="dev">设备对象</param>
        private void ShowDevContent(BatteryTestDev dev)
        {
            Body.Children.Clear();
            if (dev != null && dev.CommunicationState == "Connected")
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;

                for (int i = 0; i < dev.Channels.Count; i++)
                {
                    Channel channel1 = new Channel(dev.Channels[i]);

                    Border border1 = new Border();
                    border1.CornerRadius = new CornerRadius(10);
                    border1.Height = 180;
                    border1.Width = 200;
                    border1.Margin = new Thickness(7, 5, 7, 5);
                    border1.Child = channel1;

                    panel.Children.Add(border1);
                }

                Border border = new Border();
                border.BorderThickness = new Thickness(3);
                border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 218, 250, 240));
                border.CornerRadius = new CornerRadius(10);
                border.Height = 200;
                border.VerticalAlignment = VerticalAlignment.Top;
                border.Margin = new Thickness(7, 5, 7, 5);
                border.Child = panel;

                Body.Children.Add(border);
            }
        }

        private void ListBox_Selected(object sender, RoutedEventArgs e)
        {
            ListBoxItem lbi = e.Source as ListBoxItem;
            lbi.FontSize = 14;
            lbi.Background = new SolidColorBrush(Color.FromArgb(150, 76, 255, 0));
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CDS_Checked(object sender, RoutedEventArgs e)
        {
            if (CDSMenu.IsChecked)
            {
                MTVMenu.IsChecked = false;
            }
        }

        private void MTV_Checked(object sender, RoutedEventArgs e)
        {
            if (MTVMenu.IsChecked)
            {
                CDSMenu.IsChecked = false;
            }
        }

        private void AddIP_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.AddIP();
        }

        private void AddIPs_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.AddIPs();
        }

        private void DeleteIP_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.DeleteIP(ip);
        }

        private void DeleteAllIP_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.DeleteAllIP();
        }

        string ip = "";
        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var obj = sender as Grid;
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is StackPanel)
                {
                    for (int l = 0; l <= VisualTreeHelper.GetChildrenCount(child) - 1; l++)
                    {
                        var cld = VisualTreeHelper.GetChild(child, l);
                        if (cld is TextBlock)
                        {
                            ip = (cld as TextBlock).Text;
                        }
                    }
                }
            }
        }

        private void DevList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowDevContent((e.Source as ListBox).SelectedItem as BatteryTestDev);
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            var objs = viewmodel.DevList.Where(dev => dev.Address == ip).ToList();
            if (objs.Count == 1)
            {
                if (objs[0].CommunicationState == "Connected")
                {
                    if (objs[0].DevOffline())
                    {
                        if (objs[0].DevOnline())
                        {
                            objs[0].InitChannel();
                            ShowDevContent(objs[0]);
                        }
                    }
                }
                else
                {
                    if (objs[0].DevOnline())
                    {
                        objs[0].InitChannel();
                        ShowDevContent(objs[0]);
                    }
                }
            }
        }

        private void DisConnect_Click(object sender, RoutedEventArgs e)
        {
            var objs = viewmodel.DevList.Where(dev => dev.Address == ip).ToList();
            if (objs.Count == 1)
            {
                if (objs[0].DevOffline())
                {
                    ShowDevContent(objs[0]);
                }
            }
        }

        private void Shortcut_Checked(object sender, RoutedEventArgs e)
        {
            if (ShortcutMenu.IsChecked)
            {
                ShortcutView.Visibility = Visibility.Visible;
            }
            else
            {
                ShortcutView.Visibility = Visibility.Collapsed;
            }
            
        }

        private void State_Checked(object sender, RoutedEventArgs e)
        {
            if (StateMenu.IsChecked)
            {
                StateContent.Visibility = Visibility.Visible;
            }
            else
            {
                StateContent.Visibility = Visibility.Collapsed;
            }
        }

        private void Body_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Channel channel = e.Source as Channel;
            if (channel != null)
            {
                viewmodel.DevList[viewmodel.DevIndex].SelectChannel(((ChannelModel)channel.DataContext).ChannelBoxN);
            }
        }
    }
}
