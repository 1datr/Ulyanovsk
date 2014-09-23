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
using System.Windows.Shapes;

namespace WpfStackerLibrary
{
    /// <summary>
    /// Логика взаимодействия для VNCWin.xaml
    /// </summary>
    public partial class VNCWin : Window
    {
        public String vncIP {get; set; }

        public VNCWin()
        {
            InitializeComponent();
        }

        public void ConnectVNC()
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (vncIP != "")
            {
                vncBOX.ServerAddress = vncIP;
                vncBOX.Connect();
            }
        }
    }
}
