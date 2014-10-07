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
using System.Windows.Markup;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;


namespace WpfStackerLibrary
{
    // тип события при клике по кнопке ячейки
    public delegate void OnSelectCell(int cellno);
    // тип события при клике по квадрату штабелера
    public delegate void OnSelectStacker();
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class StackerControl : UserControl, INotifyPropertyChanged 
    {
        public StackerControl()
        {
            InitializeComponent();
            this.fPriemCells.CollectionChanged += new NotifyCollectionChangedEventHandler(fPriemCells_CollectionChanged);
            fPointsEmptyLeft.CollectionChanged += new NotifyCollectionChangedEventHandler(fPointsEmptyLeft_CollectionChanged);
            fPointsEmptyRight.CollectionChanged += new NotifyCollectionChangedEventHandler(fPointsEmptyRight_CollectionChanged);
            fFixedPoints.CollectionChanged += new NotifyCollectionChangedEventHandler(fFixedPoints_CollectionChanged);
        }

        void fFixedPoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            build_matr();
            renum();
        }

        private OnSelectCell OnSelectCell_hndlr;
        public event OnSelectCell OnSelectCell {
            add { lock (this) { OnSelectCell_hndlr += value; } }
            remove { lock (this) { OnSelectCell_hndlr -= value; } }
        }
        private OnSelectStacker OnSelectStacker_hndlr;
        public event OnSelectStacker OnSelectStacker
        {
            add { lock (this) { OnSelectStacker_hndlr += value; } }
            remove { lock (this) { OnSelectStacker_hndlr -= value; } }
        }

        void fPointsEmptyRight_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            restruct_right();
        }

        void fPointsEmptyLeft_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            restruct_left();
        }

        void fPriemCells_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            set_cell_styles();
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
                }

                foreach (Button btn in rack_right.Children)
                {
                    set_style_of_cell(btn);              
                }
            
        }

        private Dictionary<Int32, Button> GridPoints = new Dictionary<int, Button>();
 

        int SDA_OnDataAccessConnect()
        {                       
            return 0;
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

        private bool fRackLeft = true;
        private bool fRackRight = true;        

        // Dependency Property
        public static readonly DependencyProperty TModeDP = DependencyProperty.Register("TMode", typeof(bool), typeof(StackerControl), new FrameworkPropertyMetadata(false));
        // .NET Property wrapper
        [Description("Can edit current cell"), Category("Stacker")]
        public bool TMode
        {
            get
            {
                return (bool)GetValue(TModeDP);
            }
            private set
            {
                SetValue(TModeDP, value);

            }
        }

        public String Passw { get; set; }

        private void set_edit_mode()
        {
            if (fSelectedCell == -1) EditCurrent = false;
            if (TMode)
                {
                    EditCurrent = true;
                    return;
                }
            if(fSelectedCell==-1) EditCurrent = false;
            else
            {
                if(PriemCells.Contains(fSelectedCell)) EditCurrent=true;
                else EditCurrent = false;
            }
        }

        public bool SwitchMode()
        {
            
            if (TMode)
            {
                TMode = false;
                set_edit_mode();
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
                    EditCurrent = true;
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
                    build_matr();
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
                    build_matr();
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
                case "FilterFull":
                    if (!DesignerProperties.GetIsInDesignMode(this))
                        ProductlistFull = new ItemsChangeObservableCollection<Product>(SDA.GetAllProducts(FilterFull));
                    break;
                case "PriemCells":
                    try
                    {
                       /* if (PriemCells != null)
                            foreach (int p in PriemCells)
                            {

                                Button b = GridPoints[p];
                                set_style_of_cell(b);
                            }*/
                        set_cell_styles();
                    }
                    catch (System.Exception ex)
                    { }
                    break;
                case "CellMenu":
                    try
                    {                        
                        foreach (Button b in rack_left.Children)
                        {
                            b.ContextMenu = CellMenu;
                        }

                        foreach (Button b in rack_right.Children)
                        {
                            b.ContextMenu = CellMenu;
                        }
                    }
                    catch (System.Exception ex)
                    { }
                    break;
                case "StackerMenu":
                    stacker_left_panel.ContextMenu = StackerMenu;
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
                build_matr();
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
        public ObservableCollection<GridPoint> fPointsEmptyLeft = new System.Collections.ObjectModel.ObservableCollection<GridPoint>();
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
        public ObservableCollection<GridPoint> fPointsEmptyRight = new System.Collections.ObjectModel.ObservableCollection<GridPoint>();
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
        // One cell menu
        [Description("Queue of commands"), Category("Stacker")]
        public ContextMenu CellMenu     
        {         
            get {

                return (ContextMenu)this.GetValue(CellMenuDP); 
            }
            set { this.SetValue(CellMenuDP, value); }     
        }     

        public static readonly DependencyProperty CellMenuDP = 
            DependencyProperty.Register("CellMenu",
            typeof(ContextMenu), typeof(StackerControl),
            new FrameworkPropertyMetadata(null, DepParamsChanged) 
            );
        // Stacker panel menu
        [Description("Queue of commands"), Category("Stacker")]
        public ContextMenu StackerMenu
        {
            get
            {

                return (ContextMenu)this.GetValue(StackerMenuDP);
            }
            set { this.SetValue(StackerMenuDP, value); }
        }

        public static readonly DependencyProperty StackerMenuDP =
            DependencyProperty.Register("StackerMenu",
            typeof(ContextMenu), typeof(StackerControl),
            new FrameworkPropertyMetadata(null, DepParamsChanged)
            ); 

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
            if (PointsEmptyLeft == null)
                PointsEmptyLeft = new ObservableCollection<GridPoint>();
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
                            b.MouseRightButtonUp += new MouseButtonEventHandler(b_MouseRightButtonUp);
                            b.Style = Resources["RegCell"] as Style;
                            b.ContextMenu = CellMenu;
                            rack_left.Children.Add(b);
                            Grid.SetColumn(b, x);
                            Grid.SetRow(b, y);
                            c++;
                        }
                    }
                }
            }
            if (PointsEmptyRight == null)
                PointsEmptyRight = new ObservableCollection<GridPoint>();
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
                            b.MouseRightButtonUp += new MouseButtonEventHandler(b_MouseRightButtonUp);
                            b.Style = Resources["RegCell"] as Style;
                            b.ContextMenu = CellMenu;
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

        void b_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button b = (sender as Button);
         
            // context menu dropdown
            b.Focus();
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

        public void SelectCell(Int32 cell)
        {
            try
            {
                GridPoints[cell].Focus();
            }
            catch (System.Exception exc)
            { 
            }
        }

        private Int32 cell_right = 0;
        private Int32 maxcell = 0;

        private int[,] Matr_Left;
        private int[,] Matr_Right;

        private void build_matr()
        {
            try
            {
            
            Matr_Left = new int[this.Floors, this.Rows];
            Matr_Right = new int[this.Floors, this.Rows];

            int c = 0;
            int x = 0;
            int y = 0;

            for (x = 0; x < Rows; x++)
            {
                for (y = 0; y < Floors; y++)
                {
                    Matr_Left[y, x] = -2;
                    Matr_Right[y, x] = -2;
                }
            }

            x = 0;
            y = 0;
            
            bool stop = false;
            // left walking
            y = Floors - 1;
            while (!stop)
            {
                bool move = true;
                if (Matr_Left[y, x] > -2)
                { }
                else
                {
                    
                    IEnumerable<GridPoint> Empty = fPointsEmptyLeft.Where(p => ((p.X == x) && (p.Y == y)));
                    if (Empty.Count() > 0) // spend the cell
                    {
                        Matr_Left[y, x] = -1;
                    }
                    else
                    {
                        List<GridPoint> GP = fFixedPoints.Where(p => ((p.Cell == c) && (p.right == true))).ToList<GridPoint>();
                        if (GP.Count() > 0) // point found - set it in matr
                        {
                            move = false;
                            if ((GP[0] as GridPoint).right)
                            {
                                Matr_Right[(GP[0] as GridPoint).Y, (GP[0] as GridPoint).X] = c;
                            }
                            else
                            {
                                Matr_Left[(GP[0] as GridPoint).Y, (GP[0] as GridPoint).X] = c;
                            }
                        }
                        else
                        {
                            Matr_Left[y, x] = c;
                        }
                        c++;
                    }
                }
                if (move)
                {
                    y--;
                    if (y < 0)
                    {
                        y = Floors - 1;
                        x++;
                        if (x >= Rows) 
                            stop = true;
                    }
                }
            }                           

            stop = false;
            // right walking
            x = 0;
            y = 0;
            while (!stop)
            {
                bool move = true;
                if (Matr_Right[y, x] > -2)
                { }
                else
                {

                    IEnumerable<GridPoint> Empty = fPointsEmptyRight.Where(p => ((p.X == x) && (p.Y == y)));
                    if (Empty.Count() > 0) // spend the cell
                    {
                        Matr_Right[y, x] = -1;
                    }
                    else
                    {
                        List<GridPoint> GP = fFixedPoints.Where(p => ((p.Cell == c) && (p.right == false))).ToList<GridPoint>();
                        if (GP.Count() > 0) // point found - set it in matr
                        {
                            move = false;
                            if ((GP[0] as GridPoint).right)
                            {
                                Matr_Right[(GP[0] as GridPoint).Y, (GP[0] as GridPoint).X] = c;
                            }
                            else
                            {
                                Matr_Left[(GP[0] as GridPoint).Y, (GP[0] as GridPoint).X] = c;
                            }
                        }
                        else
                        {
                            Matr_Right[y, x] = c;
                        }
                        c++;
                    }
                }
                if (move)
                {
                    y++;
                    if (y >= Floors)
                    {
                        y = 0;
                        x++;
                        if (x >= Rows) stop = true;
                    }
                }
            }
            }
            catch (System.Exception exc)
            {

            }

        }

        public List<Int32> getfreecells(bool priem = true)
        {
            List<Int32> L = new List<int>();
            if (priem)
            {
                foreach (Int32 cell in PriemCells)
                {
                    if (!this.cells_occupied.Contains(cell))
                        L.Add(cell);
                }
            }
            else
            {
                
                foreach (KeyValuePair<int, Button> kvp in GridPoints)  
                {
                    if (!this.cells_occupied.Contains(kvp.Key))
                        L.Add(kvp.Key);
                }
            }
            return L;
        }

        public static readonly DependencyProperty TaraLoadedDP = DependencyProperty.Register("TaraLoaded", typeof(bool), typeof(StackerControl), new FrameworkPropertyMetadata(false, DepParamsChanged));
        [Description("Poddon is loaded"), Category("Stacker")]
        // .NET Property wrapper
        public bool TaraLoaded
        {
            get
            {
                return (bool)GetValue(TaraLoadedDP);
            }
            set
            {
                SetValue(TaraLoadedDP, value);
            }
        }

        private void renum()
        {
            if ((Matr_Left == null) || (Matr_Right == null))
                build_matr();
            if (fRackLeft)
            {
                Int32 c = 0;
                this.GridPoints.Clear();
                foreach (Button btn in rack_left.Children)
                {
                    c = Matr_Left[ Grid.GetRow(btn), Grid.GetColumn(btn)];
                    btn.Content = c.ToString();
                    //btn.SetValue(Name, "cell_" + c.ToString());
                  //  btn.Name = "cell_" + c.ToString();
                    maxcell = c;
                    if (!this.GridPoints.ContainsKey(c))
                        this.GridPoints.Add(c, btn);
                    
                }

                foreach (Button btn in rack_right.Children)
                {
                    c = Matr_Right[Grid.GetRow(btn), Grid.GetColumn(btn)];
                    btn.Content = c.ToString();
                    
                //    btn.Name = "cell_" + c.ToString();
                    maxcell = c;
                    if(!this.GridPoints.ContainsKey(c))
                        this.GridPoints.Add(c, btn);
                   
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
            try
            {
                if (b == null) return;
                Int32 n = Convert.ToInt32(b.Content.ToString());
                String style_str = "RegCell";
                if (PriemCells == null) PriemCells = new ObservableCollection<Int32>();
                if (PriemCells.Where(p => (p == n)).Count() > 0)
                    style_str = "PoddonCell";
                if (n == fSelectedCell) style_str = "CurrCell";

                if (cells_occupied.Exists(p => (p == n)))
                {
                    style_str = style_str + "_Occupied";
                }
                Style s = Resources[style_str] as Style;
                if (b.Style != s)
                    b.Style = s;
            }
            catch (System.Exception exc)
            { }
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
            set_edit_mode();
            List<CellContent> ccl = SDA.GetProductsByCell(fSelectedCell, StackerID);          
            SetValue(SelectedCellContentDP, new ItemsChangeObservableCollection<CellContent>(ccl) );
            //throw new NotImplementedException();
            if (this.OnSelectCell_hndlr != null)
                this.OnSelectCell_hndlr(fSelectedCell);
            return;
        }
        // restruct right rack
        private void restruct_right()
        {
            if (PointsEmptyRight == null) return;
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

        // .NET Property wrapper
        private ObservableCollection<Int32> fPriemCells = new ObservableCollection<Int32>();
        
        [Bindable(true), Description("List of PriemCells"), Category("Stacker")]
        public ObservableCollection<Int32> PriemCells
        {
            get
            {
                return fPriemCells;
            }
            set { 
                fPriemCells = value;
                set_cell_styles();
            }
        }
        /*
        private static readonly DependencyPropertyKey PriemCellsPropertyKey = 
        DependencyProperty.RegisterReadOnly(
          "PriemCells",
          typeof(List<Int32>),
          typeof(StackerControl),
          new FrameworkPropertyMetadata(new List<Int32>())
        );
        public static readonly DependencyProperty PriemCellsProperty = PriemCellsPropertyKey.DependencyProperty;
        [Description("Filter of products"), Category("Stacker")]
        public List<Int32> PriemCells
        {
            get { return (List<Int32>)GetValue(PriemCellsProperty); }
            set { SetValue(PriemCellsProperty,value); }
        }
        */

        // Dependency Property
        public static readonly DependencyProperty WorkParamsDP = DependencyProperty.Register("WorkParams", typeof(StackerWorkData), typeof(StackerControl), new FrameworkPropertyMetadata(new StackerWorkData()));
        // .NET Property wrapper
        [Description("Stacker parameters"), Category("Stacker")]
        public StackerWorkData WorkParams
        {
            get
            {
                return (StackerWorkData)GetValue(WorkParamsDP);
            }
            set { SetValue(WorkParamsDP, value); }
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

        // Список продуктов фильтрованный
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

        // Список продуктов полный
        // Dependency Property
        public static readonly DependencyProperty ProductlistFullDP = DependencyProperty.Register("ProductlistFull", typeof(ItemsChangeObservableCollection<Product>), typeof(StackerControl), new FrameworkPropertyMetadata(new ItemsChangeObservableCollection<Product>()));
        [Description("Full list of products filtered by ProdFilter"), Category("Stacker data")]
        // .NET Property wrapper
        public ItemsChangeObservableCollection<Product> ProductlistFull
        {
            get
            {
                return (ItemsChangeObservableCollection<Product>)GetValue(ProductlistFullDP);
            }
            private set
            {
                SetValue(ProductlistFullDP, value);
            }
        }

        public void EditProduct(Product P)
        {
            SDA.EditProduct(P);
            ProductlistFull = new ItemsChangeObservableCollection<Product>(SDA.GetAllProducts(FilterFull));
            Productlist = new ItemsChangeObservableCollection<Product>(SDA.GetAllProducts(Filter));
        }

        public void AddProduct(String pname)
        {
            Product p = new Product();
            p.Name = pname;
            SDA.CreateProduct(p);
            ProductlistFull = new ItemsChangeObservableCollection<Product>(SDA.GetAllProducts(FilterFull));
            Productlist = new ItemsChangeObservableCollection<Product>(SDA.GetAllProducts(Filter));
        }

        public void DeleteProduct(Product _p)
        {
            SDA.DeleteProduct(_p);
            refresh();
        }

        public void refresh()
        {
            ProductlistFull = new ItemsChangeObservableCollection<Product>(SDA.GetAllProducts(FilterFull));
            Productlist = new ItemsChangeObservableCollection<Product>(SDA.GetAllProducts(Filter));
            cells_occupied = null;
            set_cell_styles();
            List<CellContent> ccl = SDA.GetProductsByCell(fSelectedCell, StackerID);
            SetValue(SelectedCellContentDP, new ItemsChangeObservableCollection<CellContent>(ccl));
            SetValue(TelezhkaDP, new ItemsChangeObservableCollection<CellContent>(SDA.GetProductsOnTelezhka(StackerID)));        
        }

        // Содержимое текущей выделенной ячейки
        // Dependency Property
        public static readonly DependencyProperty FilterFullDP = DependencyProperty.Register("FilterFull", typeof(String), typeof(StackerControl), new FrameworkPropertyMetadata("", DepParamsChanged));
        [Description("Filter of products"), Category("Stacker data")]
        // .NET Property wrapper
        public String FilterFull
        {
            get
            {
                return (String)GetValue(FilterFullDP);
            }
            set
            {
                SetValue(FilterFullDP, value);
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
        // Поиск по штабелерам
        private String fFindFilterName = "";
        public String FindFilterName
        {
            get
            { 
                return fFindFilterName; 
            }
            set
            { 
                fFindFilterName = value;
            
            }
        }
        /*
        private List<Int32> fFindFilterStackerNumbers = null;
        public List<Int32> FindFilterStackerNumbers { get; set; }

        // Dependency Property
        public static readonly DependencyProperty FindByNameResDP = DependencyProperty.Register("FindByNameRes", typeof(List<CellContent>), typeof(StackerControl), new FrameworkPropertyMetadata(null, DepParamsChanged));
        [Description("Finds products in cells by name"), Category("Stacker data")]
        // .NET Property wrapper
        public List<CellContent> FindByNameRes
        {
            get
            {
                return (List<CellContent>)GetValue(FindByNameResDP);
            }
            set
            {
                SetValue(FindByNameResDP, value);
            }
        }
        */
        public List<CellContent> FindByName(String namestr, List<Int32> stackers=null)
        {
            return SDA.SearchCC(namestr, stackers);
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
                ProductlistFull = new ItemsChangeObservableCollection<Product>(SDA.GetAllProducts(this.FilterFull));
                // Detect telezhka contnet
               // this.fPointsEmptyLeft.CollectionChanged += new NotifyCollectionChangedEventHandler(fPointsEmptyLeft_CollectionChanged);
               // this.PointsEmptyRight.CollectionChanged += new NotifyCollectionChangedEventHandler(fPointsEmptyRight_CollectionChanged);
            
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

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.OnSelectStacker_hndlr != null)
                this.OnSelectStacker_hndlr();
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

        

        public List<CellContent> SearchCC(String str, List<Int32> stackers=null)
        {
            if (stackers == null)
            {
                var Res = (from CC in objDataContext.CellContents where CC.Product.Name.Contains(str) select CC).ToList<CellContent>();
                return Res;
            }
            else
            {
                var Res = (from CC in objDataContext.CellContents where CC.Product.Name.Contains(str) && stackers.Contains(CC.StackerID) select CC).ToList<CellContent>();
                return Res;
            }
        }

        public void SaveChanges()
        {            
            objDataContext.SaveChanges();
        }

        public void DeleteProduct(Product _p)
        {
            var Res_cc = (from CC in objDataContext.CellContents where CC.Product.Id == _p.Id select CC).ToList<CellContent>();
            foreach (CellContent cc in Res_cc)
            {
                objDataContext.CellContents.DeleteObject(cc);
            }

            var Res = (from Prod in objDataContext.Products where Prod.Id == _p.Id select Prod).ToList<Product>();
            if (Res.Count() == 0) throw new Exception("Not exist product");

            objDataContext.Products.DeleteObject(Res[0]);
            objDataContext.SaveChanges();
            
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

        public void EditProduct(Product P)
        {
            var Res = (objDataContext.Products.Where(p=>(p.Id == P.Id))).ToList<Product>();
            if (Res.Count() > 0)
                Res[0].Name = P.Name;
            this.SaveChanges();
        }

        public void SelectCell(int cell)
        {

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
            objDataContext.Products.AddObject(objProduct);            
            objDataContext.SaveChanges();
        }

        public List<Product> GetAllProducts(String Filter="")
        {
            if (Filter == "")
            {
                objDataContext.Refresh(System.Data.Objects.RefreshMode.ClientWins, objDataContext.Products);
                return objDataContext.Products.ToList<Product>();
            }
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
        public int Cell { get; set; }
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
    
    public class BoolToVisibleOrHidden : IValueConverter
    {
        #region Constructors
        /// <summary>
        /// The default constructor
        /// </summary>
        public BoolToVisibleOrHidden() { }
        #endregion

        #region Properties
        public bool Collapse { get; set; }
        public bool Reverse { get; set; }
        #endregion

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool bValue = (bool)value;

            if (bValue != Reverse)
            {
                return Visibility.Visible;
            }
            else
            {
                if (Collapse)
                    return Visibility.Collapsed;
                else
                    return Visibility.Hidden;
            }
        }        

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            if (visibility == Visibility.Visible)
                return !Reverse;
            else
                return Reverse;
        }
        #endregion
    }
   
    public class MyRadioButton : RadioButton
{
    public object RadioValue
    {
        get { return (object)GetValue(RadioValueProperty); }
        set { SetValue(RadioValueProperty, value); }
    }

    /* Using a DependencyProperty as the backing store for RadioValue.
       This enables animation, styling, binding, etc...*/
    public static readonly DependencyProperty RadioValueProperty =
        DependencyProperty.Register(
            "RadioValue", 
            typeof(object), 
            typeof(MyRadioButton), 
            new UIPropertyMetadata(null));

    public object RadioBinding
    {
        get { return (object)GetValue(RadioBindingProperty); }
        set { SetValue(RadioBindingProperty, value); }
    }

    /* Using a DependencyProperty as the backing store for RadioBinding.
       This enables animation, styling, binding, etc...*/
    public static readonly DependencyProperty RadioBindingProperty =
        DependencyProperty.Register(
            "RadioBinding", 
            typeof(object), 
            typeof(MyRadioButton), 
            new FrameworkPropertyMetadata(
                null, 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                OnRadioBindingChanged));

    private static void OnRadioBindingChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        MyRadioButton rb = (MyRadioButton)d;
        if (rb.RadioValue.Equals(e.NewValue))
            rb.SetCurrentValue(RadioButton.IsCheckedProperty, true);
    }

    protected override void OnChecked(RoutedEventArgs e)
    {
        base.OnChecked(e);
        SetCurrentValue(RadioBindingProperty, RadioValue);
    }
}
}