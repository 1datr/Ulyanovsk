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
    /// Логика взаимодействия для PasswWin.xaml
    /// </summary>
    public partial class PasswWin : Window
    {
        public PasswWin()
        {
            InitializeComponent();
        }

        public String Passw { get; set; }
        private bool react = true;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
       
            if (TBPassw.Password == Passw)
                DialogResult = true;
            else
            {

                    MessageBox.Show("Неверный пароль");
                    react = false;

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TBPassw.Focus();
        }


        private void TBPassw_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                if (TBPassw.Password != "")
                {
                    // check the password
                    if (TBPassw.Password == Passw)
                        DialogResult = true;
                    else
                    {
                        if (react)
                        {
                            MessageBox.Show("Неверный пароль");
                            react = false;
                        }
                    }

                }                        
                
            }
        }

        private void TBPassw_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TBPassw_PasswordChanged(object sender, RoutedEventArgs e)
        {
            react = true;
        }
    }
}
