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

namespace WpfStackerLibrary
{
    /// <summary>
    /// Логика взаимодействия для CmdQManager.xaml
    /// </summary>
    public partial class CmdQManager : UserControl
    {
        public CmdQManager()
        {
            InitializeComponent();
        }

        
        // One cell menu
        [Description("Queue of commands"), Category("Stacker")]
        public ContextMenu CellMenu
        {
            get
            {

                return (ContextMenu)this.GetValue(CellMenuDP);
            }
            set { this.SetValue(CellMenuDP, value); }
        }

        public static readonly DependencyProperty CurrCmdIdxDP =
            DependencyProperty.Register("CurrCmdIdx",
            typeof(Int32), typeof(CmdQManager),
            new FrameworkPropertyMetadata(-1, DepParamsChanged)
            );
        // Stacker panel menu
        [Description("Index in commands queue"), Category("Stacker")]
        public Int32 CurrCmdIdx
        {
            get
            {

                return (Int32)this.GetValue(CurrCmdIdxDP);
            }
            set {
                try
                {
                    Int32 oldval = CurrCmdIdx;

                    this.SetValue(CurrCmdIdxDP, value);

                    if ((oldval > -1) && (oldval < CmdQueue.Count))
                    {
                        CmdQueue[oldval].Selected = false;
                        
                    }
                    
                    CurrCmd = CmdQueue[CurrCmdIdx];


                    CmdQueue[CurrCmdIdx].Selected = true;
                   

                    DGCmdlist.ItemsSource = DGCmdlist.ItemsSource;
                }
                catch (System.Exception exc)
                { 
                
                }
            }
        }

        // Dependency Property
        public static readonly DependencyProperty MessageDP = DependencyProperty.Register("Message", typeof(String), typeof(CmdQManager), new FrameworkPropertyMetadata(""));
        // .NET Property wrapper
        public String Message
        {
            get
            {
                return (String)GetValue(MessageDP);
            }
            private set { SetValue(MessageDP, value); }
        }

        public static readonly DependencyProperty IsWorkDP =
            DependencyProperty.Register("IsWork",
            typeof(bool), typeof(CmdQManager),
            new FrameworkPropertyMetadata(false, DepParamsChanged)
            );
        // Stacker panel menu
        [Description("Index in commands queue"), Category("Stacker")]
        public bool IsWork
        {
            get
            {

                return (bool)this.GetValue(IsWorkDP);
            }
            set
            {
                try
                {                   

                    this.SetValue(IsWorkDP, value);
                    if (value)
                    {
                        
                        exe_command();
                    }
                    else
                    {
                        CurrCmd = null;
                    }

                }
                catch (System.Exception exc)
                {

                }
            }
        }

        public static readonly DependencyProperty CycleDP =
            DependencyProperty.Register("Cycle",
            typeof(bool), typeof(CmdQManager),
            new FrameworkPropertyMetadata(false, DepParamsChanged)
            );
        // Stacker panel menu
        [Description("Index in commands queue"), Category("Stacker")]
        public bool Cycle
        {
            get
            {

                return (bool)this.GetValue(CycleDP);
            }
            set
            {
                try
                {

                    this.SetValue(CycleDP, value);
                   
                }
                catch (System.Exception exc)
                {

                }
            }
        }

        private void next()
        {
            //CurrCmd = null;
            try {
                if (Cycle)
                {
                    if (CmdQueue.Count == 0)
                    {
                        CurrCmd = null;
                    }
                    else
                    {
                        CurrCmdIdx++;
                        if (CurrCmdIdx == CmdQueue.Count)
                            CurrCmdIdx = 0;
                        
                    }
                }
                else
                {
                    CmdQueue.RemoveAt(CurrCmdIdx);
                    
                }
                exe_command();
            }
            catch (System.Exception exc)
            { }
        }

        private void exe_command()
        {
            if (CmdQueue.Count > CurrCmdIdx)
            {
                CurrCmdIdx = CurrCmdIdx;
                CurrCmd = CmdQueue[CurrCmdIdx];
            }
            else
                CurrCmd = null;
            DGCmdlist.ItemsSource = DGCmdlist.ItemsSource;
        }

        private void prev()
        { 
        
        }

        private void stop()
        { 
        
        }

        private void addcommand(StackerCommand scmd)
        {
            this.CmdQueue.Add(scmd);
            if (CmdQueue.Count == 1)
            {
                
                CurrCmdIdx = 0;
                exe_command();
            }
            
        }

        public void park()
        {
            addcommand(new StackerCommand("park"));
           
        }

        public void push(int c)
        {
            addcommand(new StackerCommand("push", -1, c));
        
        }

        public void take(int c)
        {
            addcommand(new StackerCommand("take", c));
            
        }

        public void trans(int cell_from, int cell_to)
        {
            addcommand(new StackerCommand("trans", cell_from, cell_to));
           
        }

        public static readonly DependencyProperty CellMenuDP =
            DependencyProperty.Register("CellMenu",
            typeof(ContextMenu), typeof(CmdQManager),
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
            typeof(ContextMenu), typeof(CmdQManager),
            new FrameworkPropertyMetadata(null, DepParamsChanged)
            ); 


        public void SetParam(String propname, Object val, object oldval)
        {
            try
            {

                switch (propname)
                {
                    case "CmdReady":

                        if (CmdReady)
                        {
                            if (CmdQueue.Count > 0)
                                this.IsWork = true;
                           // next();
                        }
                        else
                            this.IsWork = false;
                            //switch
                    
                        break;
                    case "StackerState": {
                            switch (oldval.ToString())
                            {
                                case "Штабелер готов к выполнению команды":
                                    switch (val.ToString())
                                    {                                       
                                        case "Штабелер находиться в состоянии выполнения команды":



                                            break;
                                        case "Штабелер не готов выполненять команду":
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case "Штабелер находиться в состоянии выполнения команды":

                                    switch (val.ToString())
                                    {
                                        case "Штабелер готов к выполнению команды":
                                            // Действие завершилось - переходим к следующему
                                            next();
                                            break;
                                        case "Штабелер находиться в состоянии выполнения команды":



                                            break;
                                        case "Штабелер не готов выполненять команду":
                                            break;
                                        default:
                                            break;
                                    }

                                    break;
                                case "Штабелер не готов выполненять команду":
                                    switch (val.ToString())
                                    {
                                        case "Штабелер готов к выполнению команды":
                                            // Выполнение команды завершилось

                                            exe_command();
                                                                                            
                                            break;
                                        case "Штабелер находиться в состоянии выполнения команды":



                                            break;                                        
                                        default:
                                            break;
                                    }

                                    break;
                                default:
                                    switch (val.ToString())
                                    {
                                        case "Штабелер готов к выполнению команды":
                                            exe_command();
                                            break;
                                        case "Штабелер находиться в состоянии выполнения команды":



                                            break;
                                        case "Штабелер не готов выполненять команду":
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                            }
                        } break;
                }

            }
            catch (System.Exception ex)
                    { }
        }

        private static void DepParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CmdQManager ctrl = (CmdQManager)d;

            ctrl.SetParam(e.Property.Name, e.NewValue, e.OldValue);

        }

        // Dependency Property
        public static readonly DependencyProperty CmdQueueDP = DependencyProperty.Register("CmdQueue", typeof(ItemsChangeObservableCollection<StackerCommand>), typeof(CmdQManager), new FrameworkPropertyMetadata(null, DepParamsChanged));
        // .NET Property wrapper
        [Description("Queue of commands"), Category("Stacker")]
        public ItemsChangeObservableCollection<StackerCommand> CmdQueue
        {
            get
            {
                return (ItemsChangeObservableCollection<StackerCommand>)GetValue(CmdQueueDP);
            }
            private set { 
                SetValue(CmdQueueDP, value);
             
                }
        }

        // Dependency Property
        public static readonly DependencyProperty CurrCmdDP = DependencyProperty.Register("CurrCmd", typeof(StackerCommand), typeof(CmdQManager), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Filter of products"), Category("Stacker")]
        public StackerCommand CurrCmd
        {
            get
            {
                return (StackerCommand)GetValue(CurrCmdDP);
            }
            private set { 
                SetValue(CurrCmdDP, value);
                
            }
        }

        public void kvit()
        {
            if (Cycle)
            {
                Cycle = false;
                next();
                Cycle = true;
            }
            else
            {
                next();
            
            }
        }

        // Dependency Property
        public static readonly DependencyProperty CmdReadyDP = DependencyProperty.Register("CmdReady", typeof(bool), typeof(CmdQManager), new FrameworkPropertyMetadata(false,DepParamsChanged));
        // .NET Property wrapper
        [Description("Filter of products"), Category("Stacker")]
        public bool CmdReady
        {
            get
            {
                return (bool)GetValue(CmdReadyDP);
            }
            set { SetValue(CmdReadyDP, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty PowerDP = DependencyProperty.Register("Power", typeof(bool), typeof(CmdQManager), new FrameworkPropertyMetadata(false, DepParamsChanged));
        // .NET Property wrapper
        [Description("Power of stackerman"), Category("Stacker")]
        public bool Power
        {
            get
            {
                return (bool)GetValue(PowerDP);
            }
            set { SetValue(PowerDP, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addcommand(new StackerCommand("park"));
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 p1 = Convert.ToInt32(this.TB_Take.Text);
                addcommand(new StackerCommand("take", p1));
               
            }
            catch (System.Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        // Dependency Property
        public static readonly DependencyProperty StackerStateDP = DependencyProperty.Register("StackerState", typeof(String), typeof(CmdQManager), new FrameworkPropertyMetadata("",DepParamsChanged));
        // .NET Property wrapper
        [Description("Stacker State from Stackerman"), Category("Stacker")]
        public String StackerState
        {
            get
            {
                return (String)GetValue(StackerStateDP);
            }
            set { SetValue(StackerStateDP, value); }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 p2 = Convert.ToInt32(this.TB_Push.Text);
                addcommand(new StackerCommand("push", -1, p2));
              
            }
            catch (System.Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 p1 = Convert.ToInt32(this.TB_Trans1.Text);
                Int32 p2 = Convert.ToInt32(this.TB_Trans2.Text);
                addcommand(new StackerCommand("trans", p1, p2));
               
            }
            catch (System.Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            this.CmdQueue = new ItemsChangeObservableCollection<StackerCommand>();
            CmdQueue.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CmdQueue_CollectionChanged);

            ContextMenu m = new ContextMenu();

            MenuItem mi = new MenuItem();
            mi.Header = "Взять из";
            mi.Click += new RoutedEventHandler(mi_Click_take);
            m.Items.Add(mi);

            mi = new MenuItem();
            mi.Header = "Положить в";
            mi.Click += new RoutedEventHandler(mi_Click_put);
            m.Items.Add(mi);

            CellMenu = m;

            ContextMenu ms = new ContextMenu();

            mi = new MenuItem();
            mi.Header = "Парковать";
            mi.Click += new RoutedEventHandler(mi_Click_park);
            ms.Items.Add(mi);

            StackerMenu = ms;
        }

        void CmdQueue_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {

                if (CmdReady)
                    if (CmdQueue.Count > 0)
                    { 
                        // Нет выполняемых команд
                        if (CurrCmd == null)
                        {
                            CurrCmdIdx = 0;
                        }
                        else if (CmdQueue[CurrCmdIdx] != CurrCmd)
                        {
                            CurrCmdIdx = 0;
                        }
                    }
                
            }
            catch (System.Exception exc)
            {

            }
            
        }

        void mi_Click_put(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (Button)contextMenu.PlacementTarget;

            var CellId = Convert.ToInt32(item.Content.ToString());

            addcommand(new StackerCommand("push",-1,CellId));

        }

        void mi_Click_take(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (Button)contextMenu.PlacementTarget;

            var CellId = Convert.ToInt32(item.Content.ToString());

            addcommand(new StackerCommand("take", CellId));
        }

        void mi_Click_park(object sender, RoutedEventArgs e)
        {
            addcommand(new StackerCommand("park"));
        }

        private void TB_Take_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mi_Click_take(sender, e);
            }
        }

        private void TB_Push_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mi_Click_put(sender, e);
            }
        }

        private void TB_Trans2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click_3(sender, e);
            }
        }

        
        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void btn_next_cmd_Click(object sender, RoutedEventArgs e)
        {
            if (CurrCmdIdx < CmdQueue.Count - 1)
            {
                CurrCmdIdx++;
                exe_command();
            }
        }

        private void btn_prev_cmd_Click(object sender, RoutedEventArgs e)
        {
            if (CurrCmdIdx > 0)
            {
                CurrCmdIdx--;
                exe_command();
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (DGCmdlist.SelectedIndex > -1)
            {
                if(DGCmdlist.SelectedIndex == this.CurrCmdIdx)
                    next();
               
                else
                    CmdQueue.RemoveAt(DGCmdlist.SelectedIndex);
            }

        }

        private void DGCmdlist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrCmdIdx = DGCmdlist.SelectedIndex;
        }
    }

    public class StackerCommand : DependencyObject, INotifyPropertyChanged
    {         

        public static void OnMyPropertyChanged(DependencyObject dObject, DependencyPropertyChangedEventArgs e)
        {
            //MessageBox.Show(e.NewValue.ToString());
        }

        public static readonly DependencyProperty op1_dp =  DependencyProperty.Register("Op1", typeof(Int32), typeof(StackerCommand), new FrameworkPropertyMetadata(-1, new PropertyChangedCallback(OnMyPropertyChanged)));

        // .NET Property wrapper
        public Int32 Op1
        {
            get { return (Int32)GetValue(op1_dp); }
            set { SetValue(op1_dp, value); }
        }

        public static readonly DependencyProperty op2_dp = DependencyProperty.Register("Op2", typeof(Int32), typeof(StackerCommand), new FrameworkPropertyMetadata(-1, new PropertyChangedCallback(OnMyPropertyChanged)));

        // .NET Property wrapper
        public Int32 Op2
        {
            get { return (Int32)GetValue(op2_dp); }
            set { SetValue(op2_dp, value); }
        }

        public static readonly DependencyProperty cmdname_dp = DependencyProperty.Register("CmdName", typeof(String), typeof(StackerCommand), new FrameworkPropertyMetadata("park", new PropertyChangedCallback(OnMyPropertyChanged)));

        // .NET Property wrapper
        public String CmdName
        {
            get { return (String)GetValue( cmdname_dp); }
            set { SetValue(cmdname_dp, value); }
        }

        public static readonly DependencyProperty Selected_dp = DependencyProperty.Register("Selected", typeof(bool), typeof(StackerCommand), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnMyPropertyChanged)));

        // .NET Property wrapper
        public bool Selected
        {
            get { return (bool)GetValue(Selected_dp); }
            set { SetValue(Selected_dp, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
                if (prop == "Selected")
                { 
                
                }
            }
        }

        public StackerCommand()
        { 
        
        }

        public override String ToString()
        {
            String str = "";
            switch (CmdName)
            { 
                case "park": str = "Парковать";
                                break;
                case "push": str = "Положить в ячейку "+Op2.ToString();
                                break;
                case "take": str = "Взять из ячейки " + Op1.ToString();
                                break;
                case "trans": str = "Переместить из ячейки " + Op1.ToString() +" в ячейку " +Op2.ToString();
                                break;
            }
            return str;
        }

        public StackerCommand(String opname="park", Int32 op1_=-1, Int32 op2_=-1)
        {
            CmdName = opname;
            Op1 = op1_;
            Op2 = op2_;
        }
    }
}
