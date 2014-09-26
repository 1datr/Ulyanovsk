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
using System.Collections.ObjectModel;



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
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            

                if (cells_occupied == null)
                cells_occupied = SDA.OccupiedCells(this.StackerID);                
            
                foreach (Button btn in rack_left.Children)
                {
                    set_style_of_cell(btn);
                    /*
                    if (cells_occupied.Exists(p => (p == Convert.ToInt32(btn.Content))))
                        btn.Style = Resources["RegCell_Occupied"] as Style;
                    else
                        btn.Style = Resources["RegCell"] as Style;*/
                }

                foreach (Button btn in rack_right.Children)
                {
                    set_style_of_cell(btn);
                    /*
                    if (cells_occupied.Exists(p => (p == Convert.ToInt32(btn.Content))))
                        btn.Style = Resources["RegCell_Occupied"] as Style;
                    else
                        btn.Style = Resources["RegCell"] as Style;*/
                }
            
        }

        private Dictionary<Int32, Button> GridPoints = new Dictionary<int, Button>();
 

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

        private bool TMode = false;

        public String Passw { get; set; }

        public bool SwitchMode()
        {
            if (TMode)
            {
                TMode = false;
                return true;
            }
            else
            {
                PasswWin w = new PasswWin();
                w.Passw = Passw;
                w.ShowDialog();
                if (w.DialogResult == true)
                {
                    TMode = true;
                    return true;
                }
                return false;
            }
        }


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

        public void SetParam(String propname, Object val, object oldval)
        {
            switch (propname)
            {
                case "Rows":
                    rack_left.ColumnDefinitions.Clear();
                    for (Int32 i = 0; i < Rows; i++)
                    {
                        rack_left.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    rack_right.ColumnDefinitions.Clear();
                    for (Int32 i = 0; i < Rows; i++)
                    {
                        rack_right.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    make_cells();
                    break;
                case "Floors":
                     
                    rack_left.RowDefinitions.Clear();
                    for (Int32 i = 0; i < Floors; i++)
                    {
                        rack_left.RowDefinitions.Add(new RowDefinition());
                    
                    }

                    rack_right.RowDefinitions.Clear();
                    for (Int32 i = 0; i < Floors; i++)
                    {
                        rack_right.RowDefinitions.Add(new RowDefinition());
                    }

                    make_cells();
                    break;
                case "StackerID":
                    cells_occupied = null;
                    set_cell_styles();
                    break;
                case "PointsEmptyLeft":
                    restruct_left();
                    set_cell_styles();
                    break;
                case "PointsEmptyRight":
                    restruct_right();
                    set_cell_styles();
                    break;
                case "Filter":
                    if (!DesignerProperties.GetIsInDesignMode(this))
                        Productlist = new ItemsChangeObservableCollection<Product>(SDA.GetAllProducts(Filter));
                    break;
                case "Poddons":
                    try
                    {
                       /* if (Poddons != null)
                            foreach (int p in Poddons)
                            {

                                Button b = GridPoints[p];
                                set_style_of_cell(b);
                            }*/
                        set_cell_styles();
                    }
                    catch (System.Exception ex)
                    { }
                    break;
            }
        }

        private static void DepParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StackerControl ctrl = (StackerControl)d;
            
            ctrl.SetParam(e.Property.Name, e.NewValue, e.OldValue);
             
        }

        // Dependency Property
        public static readonly DependencyProperty RowsDP = DependencyProperty.Register("Rows", typeof(Int32), typeof(StackerControl), new FrameworkPropertyMetadata(5, DepParamsChanged));
        // .NET Property wrapper
        [Description("Row count"), Category("Stacker")]
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
        public static readonly DependencyProperty FloorsDP = DependencyProperty.Register("Floors", typeof(Int32), typeof(StackerControl), new FrameworkPropertyMetadata(5, DepParamsChanged));
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
        public static readonly DependencyProperty EditCurrentDP = DependencyProperty.Register("EditCurrent", typeof(bool), typeof(StackerControl), new FrameworkPropertyMetadata(false));
        // .NET Property wrapper
        [Description("Can edit current cell"), Category("Stacker")]
        public bool EditCurrent
        {
            get
            {
                return (bool)GetValue(EditCurrentDP);
            }
            private set
            {
                SetValue(EditCurrentDP, value);

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                /*if (propertyName == "PointsEmptyRight")
                    restruct_right();
                if (propertyName == "PointsEmptyLeft")
                    restruct_left();*/
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Dependency Property
        public static readonly DependencyProperty PointsEmptyLeftDP = DependencyProperty.Register("PointsEmptyLeft", typeof(ObservableCollection<GridPoint>), typeof(StackerControl), new FrameworkPropertyMetadata(new ObservableCollection<GridPoint>(),DepParamsChanged));
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
        public static readonly DependencyProperty PointsEmptyRightDP = DependencyProperty.Register("PointsEmptyRight", typeof(ItemsChangeObservableCollection<GridPoint>), typeof(StackerControl), new FrameworkPropertyMetadata(new ItemsChangeObservableCollection<GridPoint>(), DepParamsChanged));
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
                for (Int32 x = 0; x < Rows; x++)
                {
                    for (Int32 y = Floors - 1; y >= 0; y--)
                    {
                        Point cp = new Point(x, y);
                        IEnumerable<GridPoint> points = PointsEmptyLeft.Where(p => ((p.X == cp.X) && (p.Y == cp.Y)));
                        if (points.Count() == 0)
                        {
                            Button b = new Button();
                            TextBlock t = new TextBlock();
                            b.Content = t;                            
                            b.GotFocus += new RoutedEventHandler(Cell_Click);
                            b.Style = Resources["RegCell"] as Style;
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
                for (Int32 x = 0; x < Rows; x++)
                {
                    for (Int32 y = 0; y < Floors; y++)
                    {
                        Point cp = new Point(x, y);
                        IEnumerable<GridPoint> pres = PointsEmptyRight.Where(p => ((p.X == cp.X) && (p.Y == cp.Y)));
                        if (pres.Count() == 0)
                        {
                            Button b = new Button();
                            TextBlock t = new TextBlock();
                            b.Content = t;    
                            b.GotFocus += new RoutedEventHandler(Cell_Click);
                            b.Style = Resources["RegCell"] as Style;
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
            if (PointsEmptyLeft == null) return;
            foreach (GridPoint p in PointsEmptyLeft)
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
                this.GridPoints.Clear();
                foreach (Button btn in rack_left.Children)
                {
                    btn.Content = c.ToString();
                    //btn.SetValue(Name, "cell_" + c.ToString());
                    btn.Name = "cell_" + c.ToString();
                    maxcell = c;
                    this.GridPoints.Add(c, btn);
                    c++;
                }

                foreach (Button btn in rack_right.Children)
                {
                    btn.Content = c.ToString();
                    
                    btn.Name = "cell_" + c.ToString();
                    maxcell = c;
                    this.GridPoints.Add(c, btn);
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
        // set style of the cell
        private void set_style_of_cell(Button b)
        {
            if (b == null) return;
            Int32 n = Convert.ToInt32(b.Content.ToString());
            String style_str = "RegCell";
            if (Poddons.Exists(p => (p==n))) 
                style_str = "PoddonCell";
            if (n == fSelectedCell) style_str = "CurrCell";

            if (cells_occupied.Exists(p => (p == n)))
            {
                style_str = style_str + "_Occupied";
            }
            Style s = Resources[style_str] as Style;
            if(b.Style!=s)
                b.Style = s;
        }

       

        private ItemsChangeObservableCollection<CellContent> ccc;
        private Button currbtn = null;
        void Cell_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)e.Source;
                        
            fSelectedCell = Convert.ToInt32(btn.Content);
            if(currbtn!=null)
                set_style_of_cell(currbtn);
            currbtn = btn;
            set_style_of_cell(currbtn);
            SetValue(SelCellStrProperty, "Ячейка №"+fSelectedCell);            
          
            List<CellContent> ccl = SDA.GetProductsByCell(fSelectedCell, StackerID);
          
            SetValue(SelectedCellContentDP, new ItemsChangeObservableCollection<CellContent>(ccl) );
            //throw new NotImplementedException();
            return;
        }
        // restruct right rack
        private void restruct_right()
        {
            foreach (GridPoint p in PointsEmptyRight)
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

        // Dependency Property
        public static readonly DependencyProperty PoddonsDP = DependencyProperty.Register("Poddons", typeof(List<int>), typeof(StackerControl), new FrameworkPropertyMetadata(new List<int>(),DepParamsChanged));
        // .NET Property wrapper
        [Description("Filter of products"), Category("Stacker")]
        public List<int> Poddons
        {
            get
            {
                return (List<int>)GetValue(PoddonsDP);
            }
            set { SetValue(PoddonsDP, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty WorkParamsDP = DependencyProperty.Register("WorkParams", typeof(StackerWorkData), typeof(StackerControl), new FrameworkPropertyMetadata(new StackerWorkData()));
        // .NET Property wrapper
        public StackerWorkData WorkParams
        {
            get
            {
                return (StackerWorkData)GetValue(WorkParamsDP);
            }
            private set { SetValue(WorkParamsDP, value); }
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
                
        /*
        // Property variables list
        // Dependency Property
        public static readonly DependencyProperty VarlistDP = DependencyProperty.Register("VarList", typeof(String), typeof(Dictionary<string, Variable>), new FrameworkPropertyMetadata(null));
       */

        private int fGroup = 0;

        // Содержимое текущей выделенной ячейки
        // Dependency Property
        public static readonly DependencyProperty FilterDP = DependencyProperty.Register("Filter", typeof(String), typeof(StackerControl), new FrameworkPropertyMetadata("",DepParamsChanged));
        [Description("Filter of products"), Category("Stacker data")]
        // .NET Property wrapper
        public String Filter
        {
            get
            {
                return (String)GetValue(FilterDP);
            }
            set
            {
                SetValue(FilterDP, value);
            }
        }

        // Содержимое текущей выделенной ячейки
        // Dependency Property
        public static readonly DependencyProperty ProductlistDP = DependencyProperty.Register("Productlist", typeof(ItemsChangeObservableCollection<Product>), typeof(StackerControl), new FrameworkPropertyMetadata(new ItemsChangeObservableCollection<Product>()));
        [Description("Full list of products filtered by ProdFilter"), Category("Stacker data")]
        // .NET Property wrapper
        public ItemsChangeObservableCollection<Product> Productlist
        {
            get
            {
                return (ItemsChangeObservableCollection<Product>)GetValue(ProductlistDP);
            }
            private set
            {
                SetValue(ProductlistDP, value);
            }
        }
        
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

        private Int32 OldCurrCell = -1;
        // Содержимое тележки
        // Dependency Property
        public static readonly DependencyProperty TelezhkaDP = DependencyProperty.Register("Telezhka", typeof(ItemsChangeObservableCollection<CellContent>), typeof(StackerControl), new FrameworkPropertyMetadata(new ItemsChangeObservableCollection<CellContent>()));
        [Description("List of products in selected cell"), Category("Stacker data")]
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
        
        
       
        public static readonly DependencyProperty StackerIDDP = DependencyProperty.Register("StackerID", typeof(Int32), typeof(StackerControl), new FrameworkPropertyMetadata(1, DepParamsChanged));
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
        // add som products to cell
        public void AddProduct(Int32 prod, Int32 count, Int32 cell=-2)
        {
            if (cell == -2) cell = fSelectedCell;
            SDA.AddProduct(prod, cell, count, this.StackerID);
            List<CellContent> ccl = SDA.GetProductsByCell(fSelectedCell, StackerID);

            SetValue(SelectedCellContentDP, new ItemsChangeObservableCollection<CellContent>(ccl));
            cells_occupied = null;
            set_cell_styles();
        }
        // take some products from cell
        public void TakeProduct(Int32 prod, Int32 count, Int32 cell = -2)
        {
            if (cell == -2) cell = fSelectedCell;
            SDA.TakeProduct(prod, cell, count, this.StackerID);
            List<CellContent> ccl = SDA.GetProductsByCell(fSelectedCell, StackerID);

            SetValue(SelectedCellContentDP, new ItemsChangeObservableCollection<CellContent>(ccl));
            cells_occupied = null;
            set_cell_styles();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
               
                SDA = new StackerDataAccess();
                this.SDA.OnDataAccessConnect += new OnDataAccessConnect(SDA_OnDataAccessConnect);
                Productlist = new ItemsChangeObservableCollection<Product>(SDA.GetAllProducts());

                // Detect telezhka contnet
               // this.fPointsEmptyLeft.CollectionChanged += new NotifyCollectionChangedEventHandler(fPointsEmptyLeft_CollectionChanged);
                this.PointsEmptyRight.CollectionChanged += new NotifyCollectionChangedEventHandler(fPointsEmptyRight_CollectionChanged);
            
                SetValue(TelezhkaDP, new ItemsChangeObservableCollection<CellContent>(SDA.GetProductsOnTelezhka(StackerID)));
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

        public void AddProduct(Int32 ProdId, Int32 Cellid, Int32 ProdCount, Int32 stackerId = 1)
        {
            try
            {
                var Res = (from Prod in objDataContext.Products where Prod.Id == ProdId select Prod).ToList<Product>();
                if (Res.Count() == 0) throw new Exception("Not exist product");
                var Product = Res[0];

                var Res_Cells = (from CC in objDataContext.CellContents where CC.Product.Id == ProdId && CC.CellID==Cellid && CC.StackerID == stackerId select CC).ToList<CellContent>();
                if (Res_Cells.Count() > 0)
                {
                    Res_Cells[0].Count += ProdCount;
                    Res_Cells[0].ChangeDate = DateTime.Now;
                }
                else
                {
                    CellContent ccitem = new CellContent();
                    ccitem.Product = Res[0];
                    ccitem.StackerID = stackerId;
                    ccitem.Count = ProdCount;
                    ccitem.CellID = Cellid;
                    ccitem.ChangeDate = DateTime.Now;
                    objDataContext.CellContents.AddObject(ccitem);
                }
                objDataContext.SaveChanges();

            }
            catch (System.Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        public void TakeProduct(Int32 ProdId, Int32 Cellid, Int32 ProdCount, Int32 stackerId = 1)
        {
            try
            {
                var Res = (from Prod in objDataContext.Products where Prod.Id == ProdId select Prod).ToList<Product>();
                if (Res.Count() == 0) throw new Exception("Not exist product");
                var Product = Res[0];

                var Res_Cells = (from CC in objDataContext.CellContents where CC.Product.Id == ProdId && CC.CellID == Cellid && CC.StackerID == stackerId select CC).ToList<CellContent>();
                if (Res_Cells[0].Count > ProdCount)
                {
                    Res_Cells[0].Count -= ProdCount;
                    Res_Cells[0].ChangeDate = DateTime.Now;
                }
                else
                {
                    if (Res_Cells[0].Count == ProdCount)
                    {
                        objDataContext.CellContents.DeleteObject(Res_Cells[0]);
                    }
                    else
                        throw new Exception("Нет такого количества продуктов в данной ячейке");
                }
                objDataContext.SaveChanges();

            }
            catch (System.Exception exc)
            {
                MessageBox.Show(exc.Message);
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

        public List<Product> GetAllProducts(String Filter="")
        {
            if (Filter == "")
                return objDataContext.Products.ToList<Product>();
            else
            { 
                var Res = (from Prod in objDataContext.Products where Prod.Name.Contains(Filter) select Prod).ToList<Product>();
                return Res;
            }
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
        public bool right { get; set; }
        public string FullName { get { return string.Format("{0} {1}", X, Y); } }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public GridPoint()
        {
            X = 0;
            Y = 0;
            right = false;
        }

        public GridPoint(int _x=0, int _y=0, bool _right=false)
        {
            X = _x; 
            Y = _y; 
            right = _right;
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

        
    }


  
}