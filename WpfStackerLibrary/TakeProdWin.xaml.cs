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
    /// Логика взаимодействия для TakeProdWin.xaml
    /// </summary>
    public partial class TakeProdWin : Window
    {
        public TakeProdWin()
        {
            InitializeComponent();
        }
        public Int32 COUNT;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            
            DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 _count = 0;
                _count = Convert.ToInt32(TBCount.Text);
                if (_count <= 0)
                {
                    MessageBox.Show("Введите число больше нуля");
                }

                COUNT = _count;
                DialogResult = true;
            }
            catch (System.Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
