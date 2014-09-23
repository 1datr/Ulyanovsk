﻿using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Runtime;
using System.Configuration;
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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;
using System.Data;
using System.Threading.Tasks;

namespace WpfStackerLibrary
{
    /// <summary>
    /// Логика взаимодействия для Console.xaml
    /// </summary>
    public partial class Console : UserControl, INotifyPropertyChanged
    {
        public Console()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                if (propertyName == "StringSrc1")
                {
                   
                }
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static void InnerTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Console ctrl = (Console)d;
            ctrl.newline( e.NewValue.ToString());
        }

        // Dependency Property
        public static readonly DependencyProperty StringSrc1DP = DependencyProperty.Register("StringSrc1", typeof(String), typeof(Console), new FrameworkPropertyMetadata("", InnerTextChanged));
        // .NET Property wrapper
        public String StringSrc1
        {
            get
            {
                return (String)GetValue(StringSrc1DP);
            }
            set 
            {
                SetValue(StringSrc1DP, value);
                
            }
        }

        public void newline(String str)
        {
            tbConsole.Text = "["+DateTime.Now.ToString()+"] " + (String)GetValue(StringSrc1DP) + "\n" + tbConsole.Text;
            
        }

        // Dependency Property
        public static readonly DependencyProperty StringSrc2DP = DependencyProperty.Register("StringSrc2", typeof(String), typeof(Console), new FrameworkPropertyMetadata("", InnerTextChanged));
        // .NET Property wrapper
        public String StringSrc2
        {
            get
            {
                return (String)GetValue(StringSrc2DP);
            }
            set
            {
                SetValue(StringSrc2DP, value);

            }
        }

        private void tbConsole_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            String str = (String)GetValue(StringSrc1DP);
            newline(str);
        }
    }
}
