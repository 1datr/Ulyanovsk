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
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography;


namespace SockTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            userControl11.Focus();
        }

        private void userControl11_ServerCutText(object sender, beeVNC.ServerCutTextEventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, e.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //userControl11.LoggingLevel = beeVNC.Logtype.None;
        }
        
        private void miCtrlAltDel_Click(object sender, RoutedEventArgs e)
        {
            userControl11.SendKeyCombination(beeVNC.KeyCombination.CtrlAltDel);
        }

        private void miCtrlAltEnd_Click(object sender, RoutedEventArgs e)
        {
            userControl11.SendKeyCombination(beeVNC.KeyCombination.CtrlAltEnd);
        }

        private void miAltTab_Click(object sender, RoutedEventArgs e)
        {
            userControl11.SendKeyCombination(beeVNC.KeyCombination.AltTab);
        }

        private void CtrlEsc_Click(object sender, RoutedEventArgs e)
        {
            userControl11.SendKeyCombination(beeVNC.KeyCombination.CtrlEsc);
        }

        private void miAltF4_Click(object sender, RoutedEventArgs e)
        {
            userControl11.SendKeyCombination(beeVNC.KeyCombination.AltF4);
        }

        private void miPrint_Click(object sender, RoutedEventArgs e)
        {
            userControl11.SendKeyCombination(beeVNC.KeyCombination.Print);
        }

        private void miNumLock_Click(object sender, RoutedEventArgs e)
        {
            userControl11.SendKeyCombination(beeVNC.KeyCombination.NumLock);
        }

        private void miCapsLock_Click(object sender, RoutedEventArgs e)
        {
            userControl11.SendKeyCombination(beeVNC.KeyCombination.CapsLock);
        }

        private void miScroll_Click(object sender, RoutedEventArgs e)
        {
            userControl11.SendKeyCombination(beeVNC.KeyCombination.Scroll);
        }

        private void miConnect_Click(object sender, RoutedEventArgs e)
        {
            userControl11.ServerAddress = txtServer.Text;
            userControl11.ServerPassword = txtPassword.Text;
            userControl11.ServerPort = Convert.ToInt32(txtPort.Text);
            userControl11.Connect();
        }

        private void miRefresh_Click(object sender, RoutedEventArgs e)
        {
            userControl11.UpdateScreen();
        }
    }
}
