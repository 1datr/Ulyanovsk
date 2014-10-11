using System;
using System.Runtime;
using System.Configuration;
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
using System.Collections.Specialized;
using System.Collections;
using System.Data;
using System.Threading.Tasks;
using BR.AN.PviServices;

namespace WpfStackerLibrary
{
    /// <summary>
    /// Логика взаимодействия для StackerManBNR.xaml
    /// </summary>
    public partial class StackerManBNR : UserControl, IStackerMan, INotifyPropertyChanged 
    {
        /*
        private Visibility Visibility {
            get { }
            set { }
        }*/
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static Int32 SrvID = 1;

        // Dependency Property
        public static readonly DependencyProperty WorkParamsDP = DependencyProperty.Register("WorkParams", typeof(StackerWorkData), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        public StackerWorkData WorkParams
        {
            get
            {
                return (StackerWorkData)GetValue(WorkParamsDP);
            }
            private set { 
                SetValue(WorkParamsDP, value); 
            }
        }

        [Description("Stacker state"), Category("Stacker")]
        // Dependency Property
        public static readonly DependencyProperty StackerStateDP = DependencyProperty.Register("StackerState", typeof(String), typeof(StackerManBNR), new FrameworkPropertyMetadata(""));
        // .NET Property wrapper
        public String StackerState
        {
            get
            {
                return (String)GetValue(StackerStateDP);
            }
            set { SetValue(StackerStateDP, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty PowerBtnCaptionDP = DependencyProperty.Register("PowerBtnCaption", typeof(String), typeof(StackerManBNR), new FrameworkPropertyMetadata("OFF"));
        // .NET Property wrapper
        public String PowerBtnCaption
        {
            get
            {
                return (String)GetValue(PowerBtnCaptionDP);
            }
            private set { SetValue(PowerBtnCaptionDP, value); }
        }

        private bool PowerChangedOuter = false; // power изменено по внешнему сигналу
        // Dependency Property
        public static readonly DependencyProperty PowerDP = DependencyProperty.Register("Power", typeof(bool), typeof(StackerManBNR), new FrameworkPropertyMetadata(false));
        // .NET Property wrapper
        public bool Power
        {
            get
            {
                return (bool)GetValue(PowerDP);
            }
            set { 
                SetValue(PowerDP, value);
            }
        }

        public void SetParam(String propname, Object val, object oldval)
        {
           try
           {
               
               switch (propname)
                    {                
                
                    case "CurrCmd":
                            if (CmdReady)
                            { 
                        
                            StackerCommand cmd = val as StackerCommand;
                            Message = "Началось выполнение команды \""+CurrCmd.ToString()+"\"";
                            WorkParams.cmd = cmd;
                            //switch
                                switch(cmd.CmdName)
                                {
                                    case "park" : this.park(); break;
                                    case "take" : this.take(cmd.Op1); break;
                                    case "push": this.put(cmd.Op2); break;
                                    case "trans": this.transport(cmd.Op1, cmd.Op2); break;
                                }
                        
                            }
                            
                            break;
                    }
            }
            catch (System.Exception ex)
            {

            }
           
        }

        private static void DepParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StackerManBNR ctrl = (StackerManBNR)d;

            ctrl.SetParam(e.Property.Name, e.NewValue, e.OldValue);

        }

        // Dependency Property Module1
        public static readonly DependencyProperty Module1DP = DependencyProperty.Register("Module1", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module1"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module1
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module1DP);
            }
            set { SetValue(Module1DP, value); }
        }

        // Dependency Property Module2
        public static readonly DependencyProperty Module2DP = DependencyProperty.Register("Module2", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module2"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module2
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module2DP);
            }
            set { SetValue(Module2DP, value); }
        }

        // Dependency Property Module3
        public static readonly DependencyProperty Module3DP = DependencyProperty.Register("Module3", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module3"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module3
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module3DP);
            }
            set { SetValue(Module3DP, value); }
        }

        // Dependency Property Module4
        public static readonly DependencyProperty Module4DP = DependencyProperty.Register("Module4", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module4"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module4
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module4DP);
            }
            set { SetValue(Module4DP, value); }
        }

        // Dependency Property Module5
        public static readonly DependencyProperty Module5DP = DependencyProperty.Register("Module5", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module5"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module5
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module5DP);
            }
            set { SetValue(Module5DP, value); }
        }

        // Dependency Property Module6
        public static readonly DependencyProperty Module6DP = DependencyProperty.Register("Module6", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module6"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module6
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module6DP);
            }
            set { SetValue(Module6DP, value); }
        }

        // Dependency Property Module7
        public static readonly DependencyProperty Module7DP = DependencyProperty.Register("Module7", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module7"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module7
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module7DP);
            }
            set { SetValue(Module7DP, value); }
        }

        // Dependency Property Module8
        public static readonly DependencyProperty Module8DP = DependencyProperty.Register("Module8", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module8"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module8
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module8DP);
            }
            set { SetValue(Module8DP, value); }
        }

        // Dependency Property Module9
        public static readonly DependencyProperty Module9DP = DependencyProperty.Register("Module9", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module9"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module9
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module9DP);
            }
            set { SetValue(Module9DP, value); }
        }

        // Dependency Property Module10
        public static readonly DependencyProperty Module10DP = DependencyProperty.Register("Module10", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module10"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module10
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module10DP);
            }
            set { SetValue(Module10DP, value); }
        }

        // Dependency Property Module11
        public static readonly DependencyProperty Module11DP = DependencyProperty.Register("Module11", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module11"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module11
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module11DP);
            }
            set { SetValue(Module11DP, value); }
        }

        // Dependency Property Module12
        public static readonly DependencyProperty Module12DP = DependencyProperty.Register("Module12", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module12"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module12
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module12DP);
            }
            set { SetValue(Module12DP, value); }
        }

        // Dependency Property Module13
        public static readonly DependencyProperty Module13DP = DependencyProperty.Register("Module13", typeof(ItemsChangeObservableCollection<ModuleDigit>), typeof(StackerManBNR), new FrameworkPropertyMetadata(null));
        // .NET Property wrapper
        [Description("Module13"), Category("Stacker")]
        public ItemsChangeObservableCollection<ModuleDigit> Module13
        {
            get
            {
                return (ItemsChangeObservableCollection<ModuleDigit>)GetValue(Module13DP);
            }
            set { SetValue(Module13DP, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty dst_cell_DP = DependencyProperty.Register("dst_cell", typeof(Int32), typeof(StackerManBNR), new FrameworkPropertyMetadata(-1));
        // .NET Property wrapper
        [Description("Stacker parameters"), Category("Stacker")]
        public Int32 dst_cell
        {
            get
            {
                return (Int32)GetValue(dst_cell_DP);
            }
            set { SetValue(dst_cell_DP, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty src_cell_DP = DependencyProperty.Register("src_cell", typeof(Int32), typeof(StackerManBNR), new FrameworkPropertyMetadata(-1));
        // .NET Property wrapper
        [Description("Stacker parameters"), Category("Stacker")]
        public Int32 src_cell
        {
            get
            {
                return (Int32)GetValue(src_cell_DP);
            }
            set { SetValue(src_cell_DP, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty CurrCmdDP = DependencyProperty.Register("CurrCmd", typeof(StackerCommand), typeof(StackerManBNR), new FrameworkPropertyMetadata(null, DepParamsChanged));
        // .NET Property wrapper
        [Description("Current command"), Category("Stacker")]
        public StackerCommand CurrCmd
        {
            get
            {
                return (StackerCommand)GetValue(CurrCmdDP);
            }
            set { SetValue(CurrCmdDP, value); }
        }

        private bool cmdExecuting = false;
        public void park()
        {
            cmdExecuting = true;
            Varlist["gOPC.Input.command"].Value = 0;
            Varlist["gOPC.Input.start"].Value = true;
        }

        public void take(object cell)
        {
            cmdExecuting = true;
            Varlist["gOPC.Input.command"].Value = 1;
            Varlist["gOPC.Input.src_cell"].Value = Convert.ToInt32(cell.ToString());
            WorkParams.Cell = Convert.ToInt32(cell.ToString()); 
            Varlist["gOPC.Input.start"].Value = true;
            
        }
        public void put(object cell)
        {
            cmdExecuting = true;
            Varlist["gOPC.Input.command"].Value = 2;
            Varlist["gOPC.Input.dst_cell"].Value = Convert.ToInt32(cell.ToString());
            WorkParams.Cell = Convert.ToInt32(cell.ToString()); 
            Varlist["gOPC.Input.start"].Value = true;
        }
        public void transport(object cellfrom, object cellto)
        {
            cmdExecuting = true;
            Varlist["gOPC.Input.command"].Value = 3;
            Varlist["gOPC.Input.src_cell"].Value = Convert.ToInt32(cellfrom.ToString());
            Varlist["gOPC.Input.dst_cell"].Value = Convert.ToInt32(cellto.ToString());
            Varlist["gOPC.Input.start"].Value = true;
        }

        public void kvit()
        {
            if (Varlist.ContainsKey("gOPC.Input.ack"))
            Varlist["gOPC.Input.ack"].Value = true;
        }
        public void kvit_drives()
        {
            int maxcnt = 12;
            int i = 0;
            if (Varlist.ContainsKey("gOPC.Output.drivestatus"))
                while ((VarVal("gOPC.Output.drivestatus").ToString() != "0") && (i < maxcnt))
                {
                    Varlist["gOPC.Input.driveack"].Value = true;
                    i++;
                    System.Threading.Thread.Sleep(100);
                }
        }

        // Dependency Property
        public static readonly DependencyProperty PLCModeDP = DependencyProperty.Register("PLCMode", typeof(String), typeof(StackerManBNR), new FrameworkPropertyMetadata("None"));
        // .NET Property wrapper
        public String PLCMode
        {
            get
            {
                return (String)GetValue(PLCModeDP);
            }
            private set { SetValue(PLCModeDP, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty ErrorDP = DependencyProperty.Register("Error", typeof(String), typeof(StackerManBNR), new FrameworkPropertyMetadata("ОК"));
        // .NET Property wrapper
        public String Error
        {
            get
            {
                return (String)GetValue(ErrorDP);
            }
            private set { SetValue(ErrorDP, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty DriveErrorDP = DependencyProperty.Register("DriveError", typeof(String), typeof(StackerManBNR), new FrameworkPropertyMetadata(""));
        // .NET Property wrapper
        public String DriveError
        {
            get
            {
                return (String)GetValue(DriveErrorDP);
            }
            private set { SetValue(DriveErrorDP, value); }
        }

        public StackerManBNR()
        {
            InitializeComponent();
        }

        public Int32 PosX {
            get {
                if(Varlist.ContainsKey("gOPC.Output.Xpos"))
                    return Convert.ToInt32(Varlist["gOPC.Output.Xpos"].Value.ToString());
                return 0;
                }
        }

        public Int32 PosY
        {
            get
            {
                if (Varlist.ContainsKey("gOPC.Output.Ypos"))
                    return Convert.ToInt32(Varlist["gOPC.Output.Ypos"].Value.ToString());
                return 0;
                
            }
        }

        public Int32 PosZ
        {
            get
            {
                if (Varlist.ContainsKey("gOPC.Output.Zpos"))
                    return Convert.ToInt32(Varlist["gOPC.Output.Zpos"].Value.ToString());
                return 0;
            }
        }

        public static readonly DependencyProperty TaraLoadedDP = DependencyProperty.Register("TaraLoaded", typeof(bool), typeof(StackerManBNR), new FrameworkPropertyMetadata(false));
        [Description("Poddon is loaded"), Category("Stacker")]
        // .NET Property wrapper
        public bool TaraLoaded
        {
            get
            {
                return (bool)GetValue(TaraLoadedDP);
            }
            private set
            {
                SetValue(TaraLoadedDP, value);
            }
        }
               
        private /*static*/ Service service;
        private /*static*/ Cpu cpu;
        private String ip = null;
        private int port = 0;

        public static readonly DependencyProperty StackerIDDP = DependencyProperty.Register("StackerID", typeof(Int32), typeof(StackerManBNR), new FrameworkPropertyMetadata(1));
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

        public String curr_error { get; set; }

        private void Connect_Service(String servname)
        {
            try
            {
                Error = "OK";
                ConnStatus = "PVI service connecting ...";

                if (service == null)
                {
                    service = new Service(servname);

                }

                service.Error += new PviEventHandler(ConnectionError);
                service.Connected += new PviEventHandler(Connect_CPU);
             /*   if ((ip == null) && (port == 0))
                {
                    if (!service.IsConnected)
                        service.Connect();
                }
                else
                {*/
                    if (!service.IsConnected)
                        service.Connect(ip, port);
              //  }
               // if (service.IsConnected) Connect_CPU();
            }
            catch (System.Exception ex)
            {
                ConnStatus = Error = "Fatal error: " + ex.Message;
            }
        }

        private void ConnectionError(object sender, PviEventArgs e)
        {
            try
            {
                ConnStatus = String.Format("Error:{0} ({1})", e.ErrorText, e.ErrorCode);

                Error = ConnStatus;
                //out_cpu_state();
            /*    if (!service.IsConnected) 
                    service.Connect();*/
                /*
                if(cpu!=null)
                         if (!cpu.IsConnected) 
                             
                             cpu.Connect();*/
            }
            catch (System.Exception ex)
            {
                curr_error = "Fatal error: " + ex.Message;
            }
            //Application.Exit();
        }

        private void Connect_CPU()
        {
            Error = "OK";
            ConnStatus = "PVI service connected";
            ConnStatus = "PVI CPU connecting ...";
            try
            {
                if (cpu == null)
                    cpu = new Cpu(service, "Cpu" + StackerID.ToString());
            }
            catch (System.Exception exc)
            {
                ConnStatus = "Ошибка: " + exc.Message;
            }
            //cpu.Connection.DeviceType = DeviceType.Serial;
            cpu.Connection.DeviceType = DeviceType.TcpIp;
            
            cpu.Connection.Device.UpdateCpuParameters(ConfigurationManager.AppSettings["CPUPARAMS_" + StackerID.ToString()]);
            cpu.Connection.Device.UpdateDeviceParameters(ConfigurationManager.AppSettings["DEVPARAMS_" + StackerID.ToString()]);


            //cpu.Connection.Serial.Channel = 1;
            cpu.Error += new PviEventHandler(ConnectionError);
            cpu.Connected += new PviEventHandler(Connect_Vars);

            cpu.Connect();
        }

        private void Connect_CPU(object sender, PviEventArgs e)
        {
            Connect_CPU();
        }



        private void Connect_Vars(object sender, PviEventArgs e)
        {
            Error = "OK";
            ConnStatus = "PVI CPU connected";
            ConnStatus = "PVI variables connecting ...";

            AddVar("gOPC.Output.Xpos");
            AddVar("gOPC.Output.Ypos");
            AddVar("gOPC.Output.Zpos");
            AddVar("gOPC.Output.load");
            
            AddVar("gOPC.Output.status");
            AddVar("gOPC.Output.drivestatus");
            AddVar("gOPC.Output.power");
            AddVar("gOPC.Output.Mode");

            AddVar("gModule1"); // 12 входов с датчиков
            AddVar("gModule2");  //
            AddVar("gModule3"); // 12 выходов на исполнительных устройств
            AddVar("gModule4"); // Шинный передатчик // только Module_OK
            AddVar("gModule8"); // Шинный приемник // только Module_OK
            AddVar("gModule9"); // Энкодер оси Y // ModuleOk, DI1, DI2, Encoder
            AddVar("gModule10"); // Энкодер оси X // ModuleOk, DI1, DI2, Encoder
            AddVar("gModule11"); // Энкодер оси Z // ModuleOk, DI1, DI2, Encoder

            AddVar("gModule12"); // 12 входов с датчиков
            AddVar("gModule13"); // 12 входов с датчиков
            
            
            AddVar("gOPC.Input.ack");
            AddVar("gOPC.Input.driveack");
            AddVar("gOPC.Input.start");
            AddVar("gOPC.Input.src_cell");
            AddVar("gOPC.Input.dst_cell");
            AddVar("gOPC.Input.command");
            AddVar("gOPC.Input.power");

        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            
                
        }

        

        // Dependency Property
        public static readonly DependencyProperty VarlistDP = DependencyProperty.Register("VarList", typeof(ItemsChangeObservableCollection<VarInfo>), typeof(StackerManBNR), new FrameworkPropertyMetadata(new ItemsChangeObservableCollection<VarInfo>()));
        // .NET Property wrapper
        public ItemsChangeObservableCollection<VarInfo> VarList
        {
            get {
                return (ItemsChangeObservableCollection<VarInfo>)GetValue(VarlistDP);
            }
            private set
            {
                SetValue(VarlistDP, value);
            }
        }

        private Dictionary<string, Variable> Varlist = new Dictionary<string, Variable>();

        // Dependency Property
        public static readonly DependencyProperty ConnStatusDP = DependencyProperty.Register("ConnStatus", typeof(String), typeof(StackerManBNR), new FrameworkPropertyMetadata(""));
        // .NET Property wrapper
        public String ConnStatus
        {
            get { 
                return (String)GetValue(ConnStatusDP); 
            }
            private set { SetValue(ConnStatusDP, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty MessageDP = DependencyProperty.Register("Message", typeof(String), typeof(StackerManBNR), new FrameworkPropertyMetadata(""));
        // .NET Property wrapper
        public String Message
        {
            get
            {
                return (String)GetValue(MessageDP);
            }
            private set { SetValue(MessageDP, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty CmdReadyDP = DependencyProperty.Register("CmdReady", typeof(bool), typeof(StackerManBNR), new FrameworkPropertyMetadata(false));
        // .NET Property wrapper
        public bool CmdReady
        {
            get
            {
                return (bool)GetValue(CmdReadyDP);
            }
            private set { SetValue(CmdReadyDP, value); }
        }

        public static readonly DependencyProperty PowerBtnBrushDP = DependencyProperty.Register("PowerBtnBrush", typeof(Style), typeof(StackerManBNR), new FrameworkPropertyMetadata(new Style()));
        // .NET Property wrapper
        public Style PowerBtnBrush
        {
            get
            {
                return (Style)GetValue(PowerBtnBrushDP);
            }
            private set { SetValue(PowerBtnBrushDP, value); }
        }

        // Get variable value 
        private object VarVal(String Varname)
        {
            return Varlist[Varname].Value;
        }

        private Int32 mode;
        private void ValueChanged(object sender, VariableEventArgs e)
        {
           // Error = "";
            Variable var = (Variable)sender;
            setvarinfo(var);

            switch (var.Name)
            {
                case "gOPC.Output.Mode":
                    mode = Convert.ToInt32(VarVal("gOPC.Output.Mode").ToString());
                  //  gb_commands.Enabled = (mode == 0);
                    String[] mode_capts = { "ПА", "наладка/шаговый", "наладка/ручной" };
                    
                    SetValue(PLCModeDP,mode_capts[mode]);
                    break;

                case "gOPC.Output.status":
                    cmdExecuting = false;
                    Int32 status = Convert.ToInt32(VarVal("gOPC.Output.status").ToString());
                        switch (status)
                        {
                            case 0:
                                if(Power)
                                    StackerState = "Штабелер готов к выполнению команды";
                                else
                                    StackerState = "Штабелер выключен";
                                ConnStatus = StackerState;
                                Error = "OK";
                                CmdReady = true;
                                break;
                            case 1: StackerState = "Штабелер находиться в состоянии выполнения команды";
                                ConnStatus = StackerState;
                                Error = "OK";
                                CmdReady = false;
                                break;
                            case 2: StackerState = "Штабелер не готов выполненять команду";
                                ConnStatus = StackerState;
                                CmdReady = false;
                                Error = "OK";
                                break;
                            default:
                                {
                                    CmdReady = false;
                                    StackerState = "Ошибка " + status.ToString();
                                    String ertext = "Неизвестная ошибка";
                                    if(StackerErrors.Contains(status.ToString()))
                                        ertext = StackerErrors[status.ToString()].ToString();
                                    Error = "Ошибка: " + ertext + " (" + status.ToString() + ")";
                                    ConnStatus = Error;
                                    //ShowStatus("Ошибка " + status.ToString() + ". " + stacker_error_text(status)); break;
                                    break;
                                }
                        }
                    break;
                case "gOPC.Output.drivestatus":
                    if (VarVal("gOPC.Output.drivestatus").ToString() == "0")
                        DriveError = "";
                    else {
                        String errstr = "";
                        if (DriveErrors.Contains(VarVal("gOPC.Output.drivestatus").ToString()))
                            errstr = DriveErrors[VarVal("gOPC.Output.drivestatus").ToString()].ToString();
                        DriveError = "Ошибка привода (" + VarVal("gOPC.Output.drivestatus") + ") "+errstr;
                        ConnStatus = "Ошибка привода";
                    }

                    break;                
                case "gOPC.Output.power":
                    Power = Convert.ToBoolean(VarVal("gOPC.Output.power").ToString());
                /*    PowerChangedOuter = true;

                    PowerBtnBrush = (Style)this.PowerBtnBrushes[power.ToString()];
                    PowerBtnCaption = PowerBtnCaptions[power.ToString().ToLower()].ToString();*/
                    break;
                case "gOPC.Output.load":
                    try
                    {
                        TaraLoaded = Convert.ToBoolean(VarVal("gOPC.Output.load").ToString());
                        switch (CurrCmd.CmdName)
                        {
                            case "trans":
                                {
                                    if (WorkParams.Cell != CurrCmd.Op1)
                                        WorkParams.Cell = CurrCmd.Op1;
                                    else
                                        if (WorkParams.Cell == CurrCmd.Op1)
                                            WorkParams.Cell = CurrCmd.Op2;
                                        else
                                            WorkParams.Cell = -1;
                                };break;
                            default:
                                {
                                    WorkParams.Cell = -1;
                                };break;
                        }
                    }
                    catch (System.Exception ex)
                    { 
                    
                    }
                    break;
                case "gOPC.Output.Xpos":
                    WorkParams.X = Convert.ToInt32(VarVal(var.Name));
                    break;
                case "gOPC.Output.Ypos":
                    WorkParams.Y = Convert.ToInt32(VarVal(var.Name));
                    break;
                case "gOPC.Output.Zpos":
                    WorkParams.Z = Convert.ToInt32(VarVal(var.Name));
                    break;
                case "gOPC.Input.src_cell" :
                    src_cell = Convert.ToInt32(VarVal(var.Name)); 
                    break;
                case "gOPC.Input.dst_cell":
                    dst_cell = Convert.ToInt32(VarVal(var.Name)); 
                    break;
            }
        }

        

        private void setvarinfo(Variable var)
        {
            ItemsChangeObservableCollection<VarInfo> vil = (ItemsChangeObservableCollection<VarInfo>)GetValue(VarlistDP);
            IEnumerable<VarInfo> vi = vil.Where(p => (p.name == var.Name));
            if (vi.Count() > 0)
            {
                vi.ElementAt(0).Value = var.Value.ToString();
            }
            SetValue(VarlistDP, vil);
        }

        private void AddVar(String Varname)
        {
          //  Error = "";
            if (Varlist.ContainsKey(Varname)) return;
            Varlist.Add(Varname, new Variable(cpu, Varname));
            Varlist[Varname].Active = true;
            Varlist[Varname].ValueChanged += new VariableEventHandler(ValueChanged);
            Varlist[Varname].Connected += new PviEventHandler(variables_Connected);

            ItemsChangeObservableCollection<VarInfo> vil = (ItemsChangeObservableCollection<VarInfo>)GetValue(VarlistDP);
            vil.Add(new VarInfo() { name = Varname, Value = "" });
            SetValue(VarlistDP, vil);

            Varlist[Varname].Connect();
            
        }

        private void variables_Connected(object sender, PviEventArgs e)
        {
           // Error = "";
            ConnStatus = "PVI variables connected";
            // Console.WriteLine("Variable Connected Error=" + e.ErrorCode.ToString());
            if (Varlist.ContainsKey("gOPC.Input.power"))
                Varlist["gOPC.Input.power"].Value = true;
            //setvarinfo(var);

        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            WorkParams = new StackerWorkData();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {           
            this.Connect_Service("srv" + SrvID.ToString());
            SrvID++;
            }
            Module1 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module2 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module3 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module4 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module5 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module6 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module7 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module8 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module9 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module10 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module11 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module12 = new ItemsChangeObservableCollection<ModuleDigit>();
            Module13 = new ItemsChangeObservableCollection<ModuleDigit>();
        }

        public class VarInfo : INotifyPropertyChanged
        {
            private string _name;
            public string name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("name"));
                }
            }

            private string _value;
            public string Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    _value = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Value"));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, e);
            }
        }
        // IP VNC 
        public String VNCIP { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cpu.Disconnect();
            service.Disconnect();
            this.Connect_Service("srv" + SrvID.ToString());
        }

        

        private void btnSpec_Click(object sender, RoutedEventArgs e)
        {
            Vnc vnc_wnd = new Vnc();
            try
            {
                if (cpu == null)
                {
                    cpu = new Cpu(service, "Cpu" + StackerID.ToString());

                    //cpu.Connection.DeviceType = DeviceType.Serial;
                    cpu.Connection.DeviceType = DeviceType.TcpIp;


                    cpu.Connection.Device.UpdateCpuParameters(ConfigurationManager.AppSettings["CPUPARAMS_" + StackerID.ToString()]);
                    cpu.Connection.Device.UpdateDeviceParameters(ConfigurationManager.AppSettings["DEVPARAMS_" + StackerID.ToString()]);
                }
                vnc_wnd.IP = cpu.Connection.TcpIp.DestinationIpAddress;
                vnc_wnd.Show();

            }
            catch (System.Exception exc)
            {
               MessageBox.Show("Fatal error: " + exc.Message, "Ошибка");
               vnc_wnd.Close();
            } 
        }

        private bool power = false;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Power)
                {
                    if (Varlist["gOPC.Input.power"].Value == false)
                    {
                        Varlist["gOPC.Input.power"].Value = true;
                        System.Threading.Thread.Sleep(1000);
                        Varlist["gOPC.Input.power"].Value = false;
                    }
                    else
                        Varlist["gOPC.Input.power"].Value = false;

                }
                else
                {
                    if (Varlist["gOPC.Input.power"].Value == true)
                    {
                        Varlist["gOPC.Input.power"].Value = false;
                        System.Threading.Thread.Sleep(1000);
                        Varlist["gOPC.Input.power"].Value = true;
                    }
                    else
                        Varlist["gOPC.Input.power"].Value = true;
                }
            }
            catch (System.Exception exc)
            { }
        }

        
       
    }

    public class ErrorInfo
    {
        public Int32 Number { get; set; }
        public String Text { get; set; }

        public ErrorInfo(Int32 number = 0, String _text="")
        {
            Number = number;
            Text = _text;
        }
    }
}
