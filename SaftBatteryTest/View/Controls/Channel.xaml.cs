using SaftBatteryTest.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace SaftBatteryTest.View.Controls
{
    /// <summary>
    /// Channel.xaml 的交互逻辑
    /// </summary>
    public partial class Channel : UserControl
    {
        public Channel(ChannelModel model)
        {
            InitializeComponent();

            model = new ChannelModel();
            this.DataContext = model;
        }

        //public static readonly DependencyProperty MyTextProperty = 
        //    DependencyProperty.Register("MyText", typeof(string), typeof(Channel), new PropertyMetadata((string)null, new PropertyChangedCallback(MyTextChange)));

        //private static void MyTextChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    Channel control = d as Channel;
        //    if (control != null)
        //    {
        //        string oldT = (string)e.OldValue;
        //        string newT = (string)e.NewValue;
        //        control.UpdateProperty(newT);
        //    }
        //}

        //private void UpdateProperty(string newT)
        //{
        //    this.Title.Text = newT;
        //}

        //[Bindable(true)]
        //[Category("Appearence")]
        //public string MyText
        //{
        //    get
        //    {
        //        return (string)GetValue(MyTextProperty);
        //    }
        //    set
        //    {
        //        SetValue(MyTextProperty, value);
        //    }
        //}
    }
}
