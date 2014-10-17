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
    /// 
    public interface IStackerMan 
    {
      //  StackerWorkData WorkParams { get; }
        void park();
        void take(object cellid);
        void put(object cellid);
        void transport(object cellfrom, object cellto);
        void kvit();
        void kvit_drives();
    }


    public class StackerWorkData : DependencyObject, INotifyPropertyChanged 
        {

            public StackerWorkData(Int32 _x=0, Int32 _y=0, Int32 _z=0, StackerCommand _cmd=null)
            {
                X = _x;
                Y = _y;
                Z = _z;
                cmd = _cmd;
            }

            public StackerWorkData()
            {
               
            }

            public static readonly DependencyProperty X_DP =
                DependencyProperty.Register("X", typeof(Int32), typeof(StackerWorkData), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnMyPropertyChanged)));
            
            public Int32 X
            {
                get { return (Int32)GetValue(X_DP); }
                set { SetValue(X_DP, value); }
            }

            public static readonly DependencyProperty Y_DP =
                 DependencyProperty.Register("Y", typeof(Int32), typeof(StackerWorkData), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnMyPropertyChanged)));

            public Int32 Y
            {
                get { return (Int32)GetValue(Y_DP); }
                set { SetValue(Y_DP, value); }
            }


            public static readonly DependencyProperty Z_DP =
                 DependencyProperty.Register("Z", typeof(Int32), typeof(StackerWorkData), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnMyPropertyChanged)));

            public Int32 Z
            {
                get { return (Int32)GetValue(Z_DP); }
                set { SetValue(Z_DP, value); }
            }

            public static readonly DependencyProperty Cell_DP =
                 DependencyProperty.Register("Cell", typeof(Int32), typeof(StackerWorkData), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnMyPropertyChanged)));

            public Int32 Cell
            {
                get { return (Int32)GetValue(Cell_DP); }
                set { SetValue(Cell_DP, value); }
            }

            public static readonly DependencyProperty cmd_DP =
                 DependencyProperty.Register("cmd", typeof(StackerCommand), typeof(StackerWorkData), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMyPropertyChanged)));
            
            public StackerCommand cmd
            {
                get { return (StackerCommand)GetValue(cmd_DP); }
                set { SetValue(cmd_DP, value); }
            }

            public override String ToString()
            {

                return X.ToString() + "," + Y.ToString() + "," + Z.ToString() + "," + cmd.ToString();
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string prop)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
                   
                }
            }

            public static void OnMyPropertyChanged(DependencyObject dObject, DependencyPropertyChangedEventArgs e)
            {
                
            }

        }

    public class ModuleDigit : INotifyPropertyChanged 
        {
            private bool _mode_int;
            public bool Mode_int
            {
                get
                {
                    return _mode_int;
                }
                set
                {
                    _mode_int = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("mode_int"));
                }
            }               

            private Int32 _intval;
            public Int32 IntVal
            {
                get
                {
                    return _intval;
                }
                set
                {
                    _intval = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IntVal"));
                }
            }

            private bool _boolval;
            public bool BoolVal
            {
                get
                {
                    return _boolval;
                }
                set
                {
                    _boolval = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BoolVal"));
                }
            }

            public Visibility Vis_Bool {
                get
                {
                    if (!_mode_int) return Visibility.Visible;
                    else return Visibility.Hidden;
                }
            }

            public Visibility Vis_Int
            {
                get
                {
                    if (_mode_int) return Visibility.Visible;
                    else return Visibility.Hidden;
                }
            }

            public object Value
            {
                get {
                    if (Mode_int)
                        return _intval;
                    else
                        return _boolval;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, e);
            }
        }

}
