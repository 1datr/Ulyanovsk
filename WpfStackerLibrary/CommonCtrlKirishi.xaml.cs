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
using System.Collections.ObjectModel;

namespace WpfStackerLibrary
{
    /// <summary>
    /// Логика взаимодействия для CommonCtrlKirishi.xaml
    /// </summary>
    public partial class CommonCtrlKirishi : UserControl
    {
        public CommonCtrlKirishi()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            stacker1_man.kvit();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            stacker1_man.kvit_drives();
        }

        // Содержимое тележки
        // Dependency Property
        public static readonly DependencyProperty StackerIDDP = DependencyProperty.Register("StackerID", typeof(Int32), typeof(CommonCtrlKirishi), new FrameworkPropertyMetadata(1));
        [Description("Stacker ID"), Category("Stacker")]
        // .NET Property wrapper
        public Int32 StackerID
        {
            get
            {
                return (Int32)GetValue(StackerIDDP);
            }
            set
            {
                SetValue(StackerIDDP, value);               
            }
        }

        // Dependency Property
        public static readonly DependencyProperty RowsDP = DependencyProperty.Register("Rows", typeof(Int32), typeof(CommonCtrlKirishi), new FrameworkPropertyMetadata(5 ));
        // .NET Property wrapper
        public Int32 Rows
        {
            get
            {
                return (Int32)GetValue(RowsDP);
            }
            set
            {
                SetValue(RowsDP, value);

            }
        }

        // Dependency Property
        public static readonly DependencyProperty FloorsDP = DependencyProperty.Register("Floors", typeof(Int32), typeof(CommonCtrlKirishi), new FrameworkPropertyMetadata(5));
        // .NET Property wrapper
        [Description("Floor count"), Category("Stacker")]
        public Int32 Floors
        {
            get
            {
                return (Int32)GetValue(FloorsDP);
            }
            set
            {
                SetValue(FloorsDP, value);

            }
        }

        private static void OnCurrentTimePropertyChanged(DependencyObject source,
        DependencyPropertyChangedEventArgs e)
        {
            CommonCtrlKirishi control = source as CommonCtrlKirishi;
           // DateTime time = (DateTime)e.NewValue;
            // Put some update logic here...
        }
        // Dependency Property
        public static readonly DependencyProperty PointsEmptyLeftDP = DependencyProperty.Register("PointsEmptyLeft", typeof(ObservableCollection<GridPoint>), typeof(CommonCtrlKirishi), new FrameworkPropertyMetadata(new ObservableCollection<GridPoint>(), OnCurrentTimePropertyChanged));
        // .NET Property wrapper
        [Description("Free points in left rack"), Category("Stacker")]
        public ObservableCollection<GridPoint> PointsEmptyLeft
        {
            get
            {
                return (ObservableCollection<GridPoint>)GetValue(PointsEmptyLeftDP);
            }
            set
            {
                SetValue(PointsEmptyLeftDP, value);

            }
        }

        // Dependency Property
        public static readonly DependencyProperty PointsEmptyRightDP = DependencyProperty.Register("PointsEmptyRight", typeof(ItemsChangeObservableCollection<GridPoint>), typeof(CommonCtrlKirishi), new FrameworkPropertyMetadata(new ItemsChangeObservableCollection<GridPoint>()));
        // .NET Property wrapper
        [Description("Free points in right rack"), Category("Stacker")]
        public ItemsChangeObservableCollection<GridPoint> PointsEmptyRight
        {
            get
            {
                return (ItemsChangeObservableCollection<GridPoint>)GetValue(PointsEmptyRightDP);
            }
            set
            {
                SetValue(PointsEmptyRightDP, value);

            }
        }
    }
}
