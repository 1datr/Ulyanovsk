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
            // PriemCells changed
            fPriemCells.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(fPriemCells_CollectionChanged);
            // Left and Right emptypoints
            fPointsEmptyLeft.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(fPointsEmptyLeft_CollectionChanged);
            fPointsEmptyRight.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(fPointsEmptyRight_CollectionChanged);
            // Fixed points
            fFixedPoints.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(fFixedPoints_CollectionChanged);
            
            
            
          //  this.SetBinding(ProductlistFull, b);
        }

  //      private Binding b = new Binding();

        private bool OnSourceUpdated(Object sender, DataTransferEventArgs args)
        {
            return true;
          // Handle event
        }

        public List<CellContent> FindByName(String namestr, List<Int32> stackers = null)
        {
            return this.stacker1.FindByName(namestr, stackers);
        }

        public void AddProduct(String pname)
        {
            if (pname != "")
            {
                stacker1.AddProduct(pname);
                ProductlistFull = stacker1.ProductlistFull;
            }
        }

        public void park() 
        {
            CmdQueue.park();
        }

        public void push(int cell)
        {
            CmdQueue.push(cell);
        }

        public void take(int cell)
        {
            CmdQueue.take(cell);
        }

        public void trans(int cell1, int cell2)
        {
            CmdQueue.trans(cell1, cell2);
        }

        public void SetParam(String propname, Object val, object oldval)
        {
            switch (propname)
            {
                case "StackerID":
                        Console.logfilepath = "stacker" + this.StackerID + "_log";
                    break;
                
            }
        }

        private static void DepParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommonCtrlKirishi ctrl = (CommonCtrlKirishi)d;

           ctrl.SetParam(e.Property.Name, e.NewValue, e.OldValue);

        }

        public void move_free_priem(Int32 cell)
        {
            List<Int32> freecells = stacker1.getfreecells();
            if (freecells.Count > 0)
            {
                CmdQueue.trans(cell, freecells[0]);
            }
            else
            {
                MessageBox.Show("Нет свободных приемных ячеек");
            }
        }
        // Список продуктов полный
        // Dependency Property
        public static readonly DependencyProperty ProductlistFullDP = DependencyProperty.Register("ProductlistFull", typeof(ItemsChangeObservableCollection<Product>), typeof(CommonCtrlKirishi), new FrameworkPropertyMetadata(null,DepParamsChanged));
        [Description("Full list of products filtered by ProdFilter"), Category("Stacker data")]
        // .NET Property wrapper
        public ItemsChangeObservableCollection<Product> ProductlistFull
        {
            get
            {
                return (ItemsChangeObservableCollection<Product>)GetValue(ProductlistFullDP);
            }
            set
            {
                SetValue(ProductlistFullDP, value);
            }
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

        void fPriemCells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.stacker1.PriemCells = fPriemCells;
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
        public static readonly DependencyProperty StackerIDDP = DependencyProperty.Register("StackerID", typeof(Int32), typeof(CommonCtrlKirishi), new FrameworkPropertyMetadata(1, DepParamsChanged));
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


        // Dependency Property
        public static readonly DependencyProperty IsEditableDP = DependencyProperty.Register("IsEditable", typeof(bool), typeof(CommonCtrlKirishi), new FrameworkPropertyMetadata(false));
        // .NET Property wrapper
        [Description("Is current cell editable"), Category("Stacker")]
        public bool IsEditable
        {
            get
            {
                return (bool)GetValue(IsEditableDP);
            }
            set
            {
                SetValue(IsEditableDP, value);

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

        private ObservableCollection<Int32> fPriemCells = new ObservableCollection<Int32>();
        [Bindable(true), Description("List of PriemCells"), Category("Stacker")]
        public ObservableCollection<Int32> PriemCells
        {
            get
            {
                return fPriemCells;
            }
            set
            {
                fPriemCells = value;
                this.stacker1.PriemCells = fPriemCells;
            }
        }
        
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

        public void refresh()
        {
            stacker1.refresh();
        }

        public void DeleteProduct(Object P)
        {
            Product p1 = P as Product;
            stacker1.DeleteProduct(p1);
            ProductlistFull = stacker1.ProductlistFull;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            TakeProdWin TPW = new TakeProdWin();
            TPW.ShowDialog();
            CellContent cc = DGVProducts.SelectedItem as CellContent;
            this.stacker1.TakeProduct(cc.Product.Id, TPW.COUNT);
            
            //DGR.DataContext
        }

        public void EditProduct(Object P, String str)
        {
            Product _P = P as Product;
            _P.Name = str;
            stacker1.EditProduct(_P);
            ProductlistFull = stacker1.ProductlistFull;
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

        private Binding b = new Binding();

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            Console.logfilepath = "stacker" + this.StackerID + "_log";

            this.ProductlistFull = stacker1.ProductlistFull;
            
            b.Source = stacker1;
            b.Path = new PropertyPath("EditCurrent");
            b.Mode = BindingMode.TwoWay;
            this.SetBinding(IsEditableDP, b);
        }

        private void stacker1_OnSelectCell(int cellno)
        {
            this.TabsCellContent.SelectedIndex = 0;
        }

        private void stacker1_OnSelectStacker()
        {
            this.TabsCellContent.SelectedIndex = 1;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 thecell = Convert.ToInt32(cell_to_select.Text);
                stacker1.SelectCell(thecell);
            }
            catch (System.Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            TakeProdWin TPW = new TakeProdWin();
            TPW.ShowDialog();
            CellContent cc = DGVProductsTel.SelectedItem as CellContent;
            this.stacker1.TakeProduct(cc.Product.Id, TPW.COUNT);

            //DGR.DataContext
        }

        private void btn_add_prod_tel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stacker1.AddProduct(Convert.ToInt32(CBProduct.SelectedValue.ToString()), Convert.ToInt32(PCountTel.Text), -1);
            }
            catch (System.Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btn_add_prod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stacker1.AddProduct(Convert.ToInt32(CBProduct.SelectedValue.ToString()), Convert.ToInt32(PCount.Text));
            }
            catch (System.Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void cell_to_select_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click_6(sender, e);
            }
        }

        
        
    }
    
}
