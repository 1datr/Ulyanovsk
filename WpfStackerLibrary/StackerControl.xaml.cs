using System;
using System.Runtime;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class StackerControl : UserControl, INotifyPropertyChanged 
    {
        public StackerControl()
        {
            InitializeComponent();
            
        }

        [Description("Occupied cell style6"), Category("Stacker")]
        public Style CellOccupied { get; set; }
        [Description("Free cell style"), Category("Stacker")]
        public Style CellFree { get; set; }

        void change_button_style(Button btn)
        {
            int cell_no = Convert.ToInt32(btn.Content);
            var cells = SDA.GetProductsByCell(cell_no);
            if (cells.Count > 0)
            {
                btn.Style = CellOccupied;   
            }
            else 
            {
                btn.Style = CellFree;
            }

        }

        private List<Int32> cells_occupied_last;
        private List<Int32> cells_occupied;
        //private IEnumerable<ProductCountRec> cell_counts;

        void set_cell_styles()
        {
          // try {

                if (cells_occupied == null)
                cells_occupied = SDA.OccupiedCells(this.fStackerID);                
            
                foreach (Button btn in rack_left.Children)
                {
                    if (cells_occupied.Exists(p => (p == Convert.ToInt32(btn.Content))))
                        btn.Style = CellOccupied;
                    else
                        btn.Style = CellFree;
                }

                foreach (Button btn in rack_right.Children)
                {
                    if (cells_occupied.Exists(p => (p == Convert.ToInt32(btn.Content))))
                        btn.Style = CellOccupied;
                    else
                        btn.Style = CellFree;
                }
            
        }


        private IStackerMan fStackerman;
        [Description("Stackerman object"), Category("Stacker")]
        public IStackerMan StackerMan {
            get { return fStackerman; }
            set { 
                fStackerman = value;
                if (fStackerman != null)
                { 
                
                }
            }
        }

        int SDA_OnDataAccessConnect()
        {
            
           
            return 0;
            //throw new NotImplementedException();
        }

        private void fPointsEmptyLeft_CollectionChanged(Object sender,	NotifyCollectionChangedEventArgs e)
        {
            restruct_left();
        }

        private void fPointsEmptyRight_CollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            restruct_right();
        }

        private bool fRackLeft = true;
        private bool fRackRight = true;

        private StackerDataAccess SDA;

        public bool RackLeft {
            get { return fRackLeft; }
            set {
                fRackLeft = value;
                restruct_left();
            }
        }

       // private ModelStackerContainer ModelCon = new ModelStackerContainer();

        public bool RackRight
        {
            get { return fRackRight; }
            set { fRackRight = value; }
        }

        
        private Int32 fRows;
        [Description("Row count"), Category("Stacker")]
        public Int32 Rows
        {
            get { return fRows; }
            set
            {
                fRows = value;
                rack_left.ColumnDefinitions.Clear();
                for (Int32 i = 0; i < fRows; i++)
                {
                    rack_left.ColumnDefinitions.Add(new ColumnDefinition());
                }

                rack_right.ColumnDefinitions.Clear();
                for (Int32 i = 0; i < fRows; i++)
                {
                    rack_right.ColumnDefinitions.Add(new ColumnDefinition());
                }

                make_cells();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                if (propertyName == "PointsEmptyRight")
                    restruct_right();
                if (propertyName == "PointsEmptyLeft")
                    restruct_left();
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ItemsChangeObservableCollection<GridPoint> fPointsEmptyLeft = new ItemsChangeObservableCollection<GridPoint>();
        [Description("Points of left rack grid must be empty"), Category("Stacker")]
        public ItemsChangeObservableCollection<GridPoint> PointsEmptyLeft
        {
            get
            {
                return fPointsEmptyLeft;
            }
            set
            {
                fPointsEmptyLeft = value;
                OnPropertyChanged("PointsEmptyLeft");
                
            }
        }

        private ItemsChangeObservableCollection<GridPoint> fPointsEmptyRight = new ItemsChangeObservableCollection<GridPoint>();
        [Description("Points of right rack grid must be empty"), Category("Stacker")]
        public ItemsChangeObservableCollection<GridPoint> PointsEmptyRight
        {
            get
            {
                return fPointsEmptyRight;
            }
            set
            {
                fPointsEmptyRight = value;
                OnPropertyChanged("PointsEmptyRight");
            }
        }                   
      
        public List<string> AvailableItems     
        {         
            get { 
                if(AvailableItemsProperty==null)
                    this.SetValue(AvailableItemsProperty, new List<Point>());
                return (List<string>)this.GetValue(AvailableItemsProperty); 
            }         
            set { this.SetValue(AvailableItemsProperty, value); }     
        }     

        public static readonly DependencyProperty AvailableItemsProperty = 
            DependencyProperty.Register("AvailableItems", 
            typeof(List<string>), typeof(StackerControl), 
            new FrameworkPropertyMetadata(OnAvailableItemsChanged) 
            {
                BindsTwoWayByDefault =true
            });       

    

        public static void OnAvailableItemsChanged(
               DependencyObject sender, 
               DependencyPropertyChangedEventArgs e)
        {
            // Breakpoint here to see if the new value is being set
            var newValue = e.NewValue;
           // Debugger.Break();
        }

        private void make_cells()
        {
            Int32 c = 0;

            if (fRackLeft)
            {
                rack_left.Children.Clear();
                for (Int32 x = 0; x < fRows; x++)
                {
                    for (Int32 y = fFloors - 1; y >= 0; y--)
                    {
                        Point cp = new Point(x, y);
                        IEnumerable<GridPoint> points = fPointsEmptyLeft.Where(p => ((p.X == cp.X) && (p.Y == cp.Y)));
                        if (points.Count() == 0)
                        {
                            Button b = new Button();
                            b.GotFocus += new RoutedEventHandler(Cell_Click);
                            rack_left.Children.Add(b);
                            Grid.SetColumn(b, x);
                            Grid.SetRow(b, y);
                            c++;
                        }
                    }
                }
            }
       
            if (fRackRight)
            {
                rack_right.Children.Clear();
                for (Int32 x = 0; x < fRows; x++)
                {
                    for (Int32 y = 0; y < fFloors; y++)
                    {
                        Point cp = new Point(x, y);
                        IEnumerable<GridPoint> pres = fPointsEmptyRight.Where(p => ((p.X == cp.X) && (p.Y == cp.Y)));
                        if (pres.Count() == 0)
                        {
                            Button b = new Button();
                            b.GotFocus += new RoutedEventHandler(Cell_Click);
                            rack_right.Children.Add(b);
                            Grid.SetColumn(b, x);
                            Grid.SetRow(b, y);
                            c++;
                        }
                    }
                }
            }
            renum();
        }
        // restruct left rack
        private void restruct_left()
        {
            foreach (GridPoint p in fPointsEmptyLeft)
            {
                try
                {
                    IEnumerable<UIElement> itemsInCell = rack_left.Children.Cast<UIElement>().Where(c => (Grid.GetRow(c) == p.Y && Grid.GetColumn(c) == p.X));
                    foreach (UIElement btn in itemsInCell)
                    {                       
                        rack_left.Children.Remove(btn);
                    }
                    renum();
                }
                catch(System.Exception ex)
                {
                    renum();
                }
            }
        }

        private Int32 cell_right = 0;
        private Int32 maxcell = 0;
        private void renum()
        {
            if (fRackLeft)
            {
                Int32 c = 0;
                foreach (Button btn in rack_left.Children)
                {
                    btn.Content = c.ToString();
                    
                    btn.Name = "cell_" + c.ToString();
                    maxcell = c;
                    c++;
                }

                foreach (Button btn in rack_right.Children)
                {
                    btn.Content = c.ToString();
                    
                    btn.Name = "cell_" + c.ToString();
                    maxcell = c;
                    c++;
                }
            }
            // set cells if occupied 
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                set_cell_styles();
            }
        }

        private Int32 fSelectedCell = -1;
        public String SelCellStr {
            get {
                String str = GetValue(SelCellStrProperty).ToString();
                if (str == "")
                {
                    str = "Не выбранна ячейка";
                    SetValue(SelCellStrProperty, str);
                }
                return str;
            }
            private set { SetValue(SelCellStrProperty, value); }
        }

        public static readonly DependencyProperty SelCellStrProperty =
            DependencyProperty.Register("SelCellStr",
            typeof(string), typeof(StackerControl),
            new FrameworkPropertyMetadata( "Не выбрана ячейка"));

        private ItemsChangeObservableCollection<CellContent> ccc;
        void Cell_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)e.Source;
            fSelectedCell = Convert.ToInt32(btn.Content);
            SetValue(SelCellStrProperty, "Ячейка №"+fSelectedCell);            
          
            List<CellContent> ccl = SDA.GetProductsByCell(fSelectedCell, fStackerID);
          
            SetValue(SelectedCellContentDP, new ItemsChangeObservableCollection<CellContent>(ccl) );
            //throw new NotImplementedException();
            return;
        }
        // restruct right rack
        private void restruct_right()
        {
            foreach (GridPoint p in fPointsEmptyRight)
            {
                try
                {
                    IEnumerable<UIElement> itemsInCell = rack_right.Children.Cast<UIElement>().Where(c => (Grid.GetRow(c) == p.Y && Grid.GetColumn(c) == p.X));
                    foreach (UIElement btn in itemsInCell)
                    {
                        rack_right.Children.Remove(btn);
                    }
                    renum();
                }
                catch (System.Exception ex)
                {
                    renum();
                }
            }

        }
     
        private void restruct()
        {

            this.restruct_left();

            this.restruct_right();
           
        }

        public Style CellStyle;
        public Style CellOccupiedStyle;
        public Style CellUnloadStyle;
        public Style CellLoadStyle;

        private Int32 fFloors;
        [Description("Floor count"), Category("Stacker")]
        public Int32 Floors
        {
            get { return fFloors; }
            set
            {
                fFloors = value;
                rack_left.RowDefinitions.Clear();
                for (Int32 i = 0; i < fFloors; i++)
                {
                    rack_left.RowDefinitions.Add(new RowDefinition());
                    
                }

                rack_right.RowDefinitions.Clear();
                for (Int32 i = 0; i < fFloors; i++)
                {
                    rack_right.RowDefinitions.Add(new RowDefinition());
                }

                make_cells();
            }
        }

        
        /*
        // Property variables list
        // Dependency Property
        public static readonly DependencyProperty VarlistDP = DependencyProperty.Register("VarList", typeof(String), typeof(Dictionary<string, Variable>), new FrameworkPropertyMetadata(null));
       */

        private int fGroup = 0;
        
        
        
        // Содержимое текущей выделенной ячейки
        // Dependency Property
        public static readonly DependencyProperty SelectedCellContentDP = DependencyProperty.Register("SelectedCellContent", typeof(ItemsChangeObservableCollection<CellContent>), typeof(StackerControl), new FrameworkPropertyMetadata(new ItemsChangeObservableCollection<CellContent>()));
        [Description("List of products in selected cell"), Category("Stacker")]
        // .NET Property wrapper
        public ItemsChangeObservableCollection<CellContent> SelectedCellContent
        {
            get
            {
                return (ItemsChangeObservableCollection<CellContent>)GetValue(SelectedCellContentDP); 
            }
            private set
            {
                SetValue(SelectedCellContentDP, value);
            }
        }
        // Содержимое тележки
        // Dependency Property
        public static readonly DependencyProperty TelezhkaDP = DependencyProperty.Register("Telezhka", typeof(ItemsChangeObservableCollection<CellContent>), typeof(StackerControl), new FrameworkPropertyMetadata(new ItemsChangeObservableCollection<CellContent>()));
        [Description("List of products in selected cell"), Category("Stacker")]
        // .NET Property wrapper
        public ItemsChangeObservableCollection<CellContent> Telezhka
        {
            get
            {
                return (ItemsChangeObservableCollection<CellContent>)GetValue(TelezhkaDP);
            }
            private set
            {
                SetValue(TelezhkaDP, value);
                if (value != null)
                {
                  //  stacker_rect.Fill = 0xFFBFDBBF;
                }
                else
                {
                 //   stacker_rect.Fill = new Brus

                }
            }
        }

        

        

       

        Int32 fStackerID = 1;
        public Int32 StackerID {
            get { return fStackerID; }
            set { 
                fStackerID = value;
                
              
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                SDA = new StackerDataAccess();
                this.SDA.OnDataAccessConnect += new OnDataAccessConnect(SDA_OnDataAccessConnect);

                // Detect telezhka contnet
                this.fPointsEmptyLeft.CollectionChanged += new NotifyCollectionChangedEventHandler(fPointsEmptyLeft_CollectionChanged);
                this.fPointsEmptyRight.CollectionChanged += new NotifyCollectionChangedEventHandler(fPointsEmptyRight_CollectionChanged);
            
                SetValue(TelezhkaDP, new ItemsChangeObservableCollection<CellContent>(SDA.GetProductsOnTelezhka(fStackerID)));
            }
            else
            {
                restruct_left();
                restruct_right();
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }

    public delegate int OnDataAccessConnect();   

    public class StackerDataAccess
    {
        private bool inited = false;
        ModelStackerContainer objDataContext;
        public StackerDataAccess()
        {
            objDataContext = new ModelStackerContainer();
            if (this.OnDataAccessConnect_hndlr != null)
                if (!inited)
                {
                    this.OnDataAccessConnect_hndlr();
                    inited = true;
                }
        }

        private OnDataAccessConnect OnDataAccessConnect_hndlr;
        public event OnDataAccessConnect OnDataAccessConnect
        {
            add
            {
                lock (this)
                {
                    OnDataAccessConnect_hndlr += value;
                    if (this.OnDataAccessConnect_hndlr != null)
                        if (!inited)
                        {
                            this.OnDataAccessConnect_hndlr();
                            inited = true;
                        }
                }
            }
            remove { lock (this) { OnDataAccessConnect_hndlr -= value; } }
        }

        public void CreateProduct(Product objProduct)
        {
            objDataContext.AddToProducts(objProduct);
            objDataContext.SaveChanges();
        }

        public List<Product> GetAllProducts()
        {
            return objDataContext.Products.ToList<Product>();
        }
        // содержимое ячейки
        public List<CellContent> GetProductsByCell(Int32 cell_id, Int32 stacker_id=1)
        { 
            var Res = (from CellCont in objDataContext.CellContents
                       from Prod in objDataContext.Products
                       where CellCont.Product.Id == Prod.Id && CellCont.CellID == cell_id && CellCont.StackerID == stacker_id
                       select CellCont).ToList<CellContent>();
            return Res;
        }
        // продукты на тележке
        public List<CellContent> GetProductsOnTelezhka(Int32 stacker_id = 1)
        {
            var Res = (from CellCont in objDataContext.CellContents
                       from Prod in objDataContext.Products
                       where CellCont.Product.Id == Prod.Id && CellCont.CellID == -1 && CellCont.StackerID == stacker_id
                       select CellCont).ToList<CellContent>();
            return Res;
        }

        private Int32 fStackerID = 1;
        public Int32 StackerID {
            get { return fStackerID; }
            set { fStackerID = value; }
        }

        public List<Int32> OccupiedCells(Int32 stackerid = 1)
        {
            var Res = (from CellCont in objDataContext.CellContents
                       where CellCont.StackerID == stackerid
                       select CellCont.CellID).Distinct().ToList<Int32>();
            return Res;          
        }

       
        /*public List<Department> GetAllDepartments()
        {
            return objDataContext.Department.ToList<Department>();
        }

        public List<Employee> GetAllEmployeeBeDeptName(string DeptName)
        {
            var Res = (from Emp in objDataContext.Employee
                       from Dept in objDataContext.Department
                       where Emp.DeptNo == Dept.DeptNo && Dept.Dname == DeptName
                       select Emp).ToList<Employee>();
            return Res;
        }*/
    }

   

    public class GridPoint : INotifyPropertyChanged
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string FullName { get { return string.Format("{0} {1}", X, Y); } }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }

    


    public class ProductCountRec
    {


        public int Cell { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return string.Format("Cell: {0}, Count: {1}", Cell, Count);
        }

    }   

    /// <summary>
    ///     This class adds the ability to refresh the list when any property of
    ///     the objects changes in the list which implements the INotifyPropertyChanged. 
    /// </summary>
    /// <typeparam name="T">
    public class ItemsChangeObservableCollection<T> :
           ObservableCollection<T> where T : INotifyPropertyChanged
    {
        private bool notnotify = false;


        public ItemsChangeObservableCollection()
            : base()
        {

        }

        public ItemsChangeObservableCollection(List<T> l): base(l)
        { 
            
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (notnotify) return;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                RegisterPropertyChanged(e.NewItems);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                UnRegisterPropertyChanged(e.OldItems);
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                UnRegisterPropertyChanged(e.OldItems);
                RegisterPropertyChanged(e.NewItems);
            }

            base.OnCollectionChanged(e);
        }

        protected override void ClearItems()
        {
            UnRegisterPropertyChanged(this);
            base.ClearItems();
        }

        private void RegisterPropertyChanged(IList items)
        {
            foreach (INotifyPropertyChanged item in items)
            {
                if (item != null)
                {
                    item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
        }

        private void UnRegisterPropertyChanged(IList items)
        {
            foreach (INotifyPropertyChanged item in items)
            {
                if (item != null)
                {
                    item.PropertyChanged -= new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void FromList(List<T> L)
        {
            
          //  notnotify = true;
            this.Clear();
            
            foreach (T item in L)
            {
                this.Add(item);
            }
          //  notnotify = false;
        }
    }


  
}