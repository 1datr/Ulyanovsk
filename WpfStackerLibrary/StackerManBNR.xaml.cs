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
        public static readonly DependencyProperty WorkParamsDP = DependencyProperty.Register("WorkParams", typeof(StackerWorkData), typeof(StackerManBNR), new FrameworkPropertyMetadata(new StackerWorkData()));
        // .NET Property wrapper
        public StackerWorkData WorkParams
        {
            get
            {
                return (StackerWorkData)GetValue(WorkParamsDP);
            }
            private set { SetValue(WorkParamsDP, value); }
        }

        public void park()
        { 
        }

        public void take(object cellid)
        {

        }
        public void put(object cellid)
        {

        }
        public void transport(object cellfrom, object cellto)
        {

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

        private /*static*/ Service service;
        private /*static*/ Cpu cpu;
        private String ip = null;
        private int port = 0;

        private Int32 f_StackerID = 1;
        public Int32 StackerID
        {
            get { return f_StackerID; }
            set
            {
                f_StackerID = value;


            }
        }
        public String curr_error { get; set; }

        private void Connect_Service(String servname)
        {
            try
            {                

                ConnStatus = "PVI service connecting ...";

                if (service == null)
                {
                    service = new Service(servname);

                }

                service.Error += new PviEventHandler(ConnectionError);
                service.Connected += new PviEventHandler(Connect_CPU);
                if ((ip == null) && (port == 0))
                {
                    if (!service.IsConnected)
                        service.Connect();
                }
                else
                {
                    if (!service.IsConnected)
                        service.Connect(ip, port);
                }
                if (service.IsConnected) Connect_CPU();
            }
            catch (System.Exception ex)
            {
                curr_error = "Fatal error: " + ex.Message;
            }
        }

        private void ConnectionError(object sender, PviEventArgs e)
        {
            try
            {
                ConnStatus = String.Format("Error:{0} ({1})", e.ErrorText, e.ErrorCode);


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
            ConnStatus = "PVI service connected";
            ConnStatus = "PVI CPU connecting ...";
            try
            {
                if (cpu == null)
                    cpu = new Cpu(service, "Cpu" + f_StackerID.ToString());
            }
            catch (System.Exception exc)
            {
                ConnStatus = "Ошибка: " + exc.Message;
            }
            //cpu.Connection.DeviceType = DeviceType.Serial;
            cpu.Connection.DeviceType = DeviceType.TcpIp;
            /*   cpu.Connection.TcpIp.DestinationIpAddress = "127.0.0.1";
               cpu.Connection.TcpIp.DestinationPort = 11160;*/

            /*cpu.Connection.TcpIp.CpuParameterString = ConfigurationManager.AppSettings["CPUPARAMS_" + f_StackerID.ToString()];
            cpu.Connection.TcpIp.DeviceParameterString = ConfigurationManager.AppSettings["DEVPARAMS_" + f_StackerID.ToString()];
             */
            //  cpu.Connection.conn = ConfigurationManager.AppSettings["PLC_IP_" + f_StackerID.ToString()];

            cpu.Connection.Device.UpdateCpuParameters(ConfigurationManager.AppSettings["CPUPARAMS_" + f_StackerID.ToString()]);
            cpu.Connection.Device.UpdateDeviceParameters(ConfigurationManager.AppSettings["DEVPARAMS_" + f_StackerID.ToString()]);


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

          /*  AddVar("gModule1"); // 12 входов с датчиков
            AddVar("gModule2");  //
            AddVar("gModule3"); // 12 выходов на исполнительных устройств
            AddVar("gModule4"); // Шинный передатчик // только Module_OK
            AddVar("gModule8"); // Шинный приемник // только Module_OK
            AddVar("gModule9"); // Энкодер оси Y // ModuleOk, DI1, DI2, Encoder
            AddVar("gModule10"); // Энкодер оси X // ModuleOk, DI1, DI2, Encoder
            AddVar("gModule11"); // Энкодер оси Z // ModuleOk, DI1, DI2, Encoder

            AddVar("gModule12"); // 12 входов с датчиков
            AddVar("gModule13"); // 12 входов с датчиков
            */
            /*
            AddVar("gOPC.Input.ack");
            AddVar("gOPC.Input.driveack");
            AddVar("gOPC.Input.start");
            AddVar("gOPC.Input.src_cell");
            AddVar("gOPC.Input.dst_cell");
            AddVar("gOPC.Input.command");
            AddVar("gOPC.Input.power");*/

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

         private void ValueChanged(object sender, VariableEventArgs e)
        {
            Variable var = (Variable)sender;
            setvarinfo(var);
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
            
            ConnStatus = "PVI variables connected";
            // Console.WriteLine("Variable Connected Error=" + e.ErrorCode.ToString());
            if (Varlist.ContainsKey("gOPC.Input.power"))
                Varlist["gOPC.Input.power"].Value = true;
            //setvarinfo(var);

        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {           
            this.Connect_Service("srv" + SrvID.ToString());
            SrvID++;
            }
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

        public String VNCIP { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Connect_Service("srv" + SrvID.ToString());
        }

        private void btnSpec_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                VNCWin Vnc = new VNCWin();
                Vnc.vncIP = this.cpu.Connection.TcpIp.DestinationIpAddress;
                Vnc.Show();
            }
            catch (System.Exception exc)
            {
              //  MessageBox.Show("Fatal error: " + exc.Message, "Ошибка");
            } 
        }
    }
}
