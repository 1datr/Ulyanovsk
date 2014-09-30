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
using System.Collections;

namespace WpfStackerLibrary
{
    /// <summary>
    /// Логика взаимодействия для CommonCtrlKirishi.xaml
    /// </summary>
    public partial class CommonCtrlKirishi : UserControl, INotifyPropertyChanged
    {
        public CommonCtrlKirishi()
        {
            InitializeComponent();
            // Poddons changed
            fPoddons.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(fPoddons_CollectionChanged);
            // Left and Right emptypoints
            fPointsEmptyLeft.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(fPointsEmptyLeft_CollectionChanged);
            fPointsEmptyRight.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(fPointsEmptyRight_CollectionChanged);
            // Fixed points
            fFixedPoints.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(fFixedPoints_CollectionChanged);
        }

        void fFixedPoints_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            stacker1.FixedPoints = fFixedPoints;
        }

        void fPointsEmptyRight_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.stacker1.PointsEmptyRight = PointsEmptyRight;
        }

        void fPointsEmptyLeft_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.stacker1.PointsEmptyLeft = PointsEmptyLeft;
        }

        void fPoddons_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.stacker1.Poddons = fPoddons;
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

        private String fPassw = "";
        public String Passw
        {
            get { return fPassw; }
            set {
                fPassw = value;
                stacker1.Passw = fPassw;
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

        // Fixed points
        private ObservableCollection<GridPoint> fFixedPoints = new ObservableCollection<GridPoint>();
        //
        [Description("Rack points with fixed points"), Category("Stacker")]
        public ObservableCollection<GridPoint> FixedPoints
        {
            get
            {
                return fFixedPoints;
            }
            set
            {
                fFixedPoints = value;

            }
        }

        // Dependency Property
        private ObservableCollection<GridPoint> fPointsEmptyLeft = new ObservableCollection<GridPoint>();    
        // .NET Property wrapper
        [Description("Free points in left rack"), Category("Stacker")]
        public ObservableCollection<GridPoint> PointsEmptyLeft
        {
            get
            {
                return fPointsEmptyLeft;
            }
            set
            {
                fPointsEmptyLeft = value;

            }
        }

        // Dependency Property
        private ObservableCollection<GridPoint> fPointsEmptyRight = new ObservableCollection<GridPoint>();
        // .NET Property wrapper
        [Description("Free points in right rack"), Category("Stacker")]
        public ObservableCollection<GridPoint> PointsEmptyRight
        {
            get
            {
                return fPointsEmptyRight;
            }
            set
            {
                fPointsEmptyRight = value;

            }
        }

        private ObservableCollection<Int32> fPoddons = new ObservableCollection<Int32>();
        [Bindable(true), Description("List of poddons"), Category("Stacker")]
        public ObservableCollection<Int32> Poddons
        {
            get
            {
                return fPoddons;
            }
            set
            {
                fPoddons = value;
                this.stacker1.Poddons = fPoddons;
            }
        }
        /*
        public static DependencyProperty ValidationErrorsProperty =
           DependencyProperty.Register("ValidationErrors",
           typeof(object), typeof(CommonCtrlKirishi),
           new PropertyMetadata(null, OnValidationErrorsChanged
        ));
        */
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stacker1.AddProduct(Convert.ToInt32(CBProduct.SelectedValue.ToString()), Convert.ToInt32(PCount.Text));
            }
            catch(System.Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            TakeProdWin TPW = new TakeProdWin();
            TPW.ShowDialog();
            CellContent cc = DGVProducts.SelectedItem as CellContent;
            this.stacker1.TakeProduct(cc.Product.Id, TPW.COUNT);
            
            //DGR.DataContext
        }

        private bool tedit = false;

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            if (stacker1.SwitchMode())
            {
                tedit = !tedit;
                if (tedit)
                    BTNswitchmode.Content = "Обычное редактирование";
                else
                    BTNswitchmode.Content = "Тотальное редактирование";
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            
        }

        
    }
    
}
