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
using System.Windows.Forms.Integration;
using VncSharp;

namespace WpfStackerLibrary
{
    /// <summary>
    /// Логика взаимодействия для Vnc.xaml
    /// </summary>
    public partial class Vnc : Window
    {
        private RemoteDesktop RD = new RemoteDesktop();

        public Vnc()
        {
            InitializeComponent();
        }

        public Int32 Port 
        {
            set {
                RD.VncPort = value;
            }
            get {
                return RD.VncPort;
            }
        }

        private String fIP = "";
        public String IP 
        {
            set {
                fIP = value;
                
                windowsFormsHost1.Child = RD;
                RD.Connect(fIP);
            }
            get {
                return fIP;
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            RD.VncPort = 5900;
        }
    }
}
