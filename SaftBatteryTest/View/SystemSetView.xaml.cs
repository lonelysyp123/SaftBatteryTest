﻿using SaftBatteryTest.ViewModel;
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
    /// SystemSetView.xaml 的交互逻辑
    /// </summary>
    public partial class SystemSetView : Window
    {
        public SystemSetView(SystemSetViewModel viewmodel)
        {
            InitializeComponent();

            this.DataContext = viewmodel;
        }
    }
}
