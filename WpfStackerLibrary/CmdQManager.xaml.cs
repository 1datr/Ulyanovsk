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
                                CurrCmd = CmdQueue[0];
                            //switch
                    
                        break;
                    case "CmdQueue": {
                        if (CmdReady)
                            if (CmdQueue.Count > 0)
                                CurrCmd = CmdQueue[0];
                        } break;
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
                                            // Действие завершилось
                                            CmdQueue.RemoveAt(0);
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
                                            if (CmdQueue.Count > 0)
                                                CurrCmd = CmdQueue[0];
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
        public static readonly DependencyProperty CmdQueueDP = DependencyProperty.Register("CmdQueue", typeof(ItemsChangeObservableCollection<StackerCommand>), typeof(CmdQManager), new FrameworkPropertyMetadata(new ItemsChangeObservableCollection<StackerCommand>(), DepParamsChanged));
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
            private set { SetValue(CurrCmdDP, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty CmdReadyDP = DependencyProperty.Register("CmdReady", typeof(bool), typeof(CmdQManager), new FrameworkPropertyMetadata(false));
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CmdQueue.Add(new StackerCommand("park"));
            if (CmdReady)
                if (CmdQueue.Count > 0)
                    CurrCmd = CmdQueue[0];
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 p1 = Convert.ToInt32(this.TB_Take.Text);
                CmdQueue.Add(new StackerCommand("take", p1));
                if (CmdReady)
                    if (CmdQueue.Count > 0)
                        CurrCmd = CmdQueue[0];
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
                CmdQueue.Add(new StackerCommand("push", -1, p2));
                if (CmdReady)
                    if (CmdQueue.Count > 0)
                        CurrCmd = CmdQueue[0];
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
                CmdQueue.Add(new StackerCommand("trans", p1, p2));
                if (CmdReady)
                    if (CmdQueue.Count > 0)
                        CurrCmd = CmdQueue[0];
            }
            catch (System.Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
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

        void mi_Click_put(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (Button)contextMenu.PlacementTarget;

            var CellId = Convert.ToInt32(item.Content.ToString());

            this.CmdQueue.Add(new StackerCommand("push",-1,CellId));

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

            this.CmdQueue.Add(new StackerCommand("take", CellId));
        }

        void mi_Click_park(object sender, RoutedEventArgs e)
        {
            this.CmdQueue.Add(new StackerCommand("park"));
        }
    }

    public class StackerCommand : INotifyPropertyChanged
    { 
        private Int32 _op1 = -1;
        public Int32 Op1 { get { return _op1; } set { _op1 = value; } }

        private Int32 _op2 = -1;
        public Int32 Op2 { get { return _op2; } set { _op2 = value; } }

        private String f_cmdname = "park";
        public String CmdName { get { return f_cmdname; } set { f_cmdname = value; } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public StackerCommand()
        { 
        
        }

        public StackerCommand(String opname="park", Int32 op1_=-1, Int32 op2_=-1)
        {
            f_cmdname = opname;
            _op1 = op1_;
            _op2 = op2_;
        }
    }
}
