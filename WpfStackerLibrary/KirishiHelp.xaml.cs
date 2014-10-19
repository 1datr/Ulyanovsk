using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace WpfStackerLibrary
{
    /// <summary>
    /// Логика взаимодействия для KirishiHelp.xaml
    /// </summary>
    public partial class KirishiHelp : UserControl
    {
        public KirishiHelp()
        {
            InitializeComponent();
        }

        [Description("Stacker State from Stackerman"), Category("Styles")]
        public static readonly DependencyProperty StylePriemDP = DependencyProperty.Register("StylePriem", typeof(Style), typeof(KirishiHelp), new FrameworkPropertyMetadata(null));

        // .NET Property wrapper
        public Style StylePriem
        {
            get { return (Style)GetValue(StylePriemDP); }
            set { SetValue(StylePriemDP, value); }
        }

        [Description("Stacker State from Stackerman"), Category("Styles")]
        public static readonly DependencyProperty StyleRegDP = DependencyProperty.Register("StyleReg", typeof(Style), typeof(KirishiHelp), new FrameworkPropertyMetadata(null));

        // .NET Property wrapper
        public Style StyleReg
        {
            get { return (Style)GetValue(StyleRegDP); }
            set { SetValue(StyleRegDP, value); }
        }

        [Description("Stacker State from Stackerman"), Category("Styles")]
        public static readonly DependencyProperty StyleFullDP = DependencyProperty.Register("StyleFull", typeof(Style), typeof(KirishiHelp), new FrameworkPropertyMetadata(null));

        // .NET Property wrapper
        public Style StyleFull
        {
            get { return (Style)GetValue(StyleFullDP); }
            set { SetValue(StyleFullDP, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            About aboutwin = new About();
            aboutwin.ShowDialog();
        }
    }
}
