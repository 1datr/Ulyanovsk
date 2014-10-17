﻿using System;
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


    public class StackerWorkData : /*DependencyObject,*/ INotifyPropertyChanged 
        {
        /*
            public static readonly DependencyProperty X_DP =
                DependencyProperty.Register("X", typeof(Int32), typeof
                    (StackerWorkData), new UIPropertyMetadata(0));
            
            public Int32 X
            {
                get { return (Int32)GetValue(X_DP); }
                set { SetValue(X_DP, value); }
            }

            public static readonly DependencyProperty Y_DP =
                 DependencyProperty.Register("Y", typeof(Int32), typeof
                     (StackerWorkData), new UIPropertyMetadata(0));

            public Int32 Y
            {
                get { return (Int32)GetValue(Y_DP); }
                set { SetValue(Y_DP, value); }
            }


            public static readonly DependencyProperty Z_DP =
                 DependencyProperty.Register("Z", typeof(Int32), typeof
                     (StackerWorkData), new UIPropertyMetadata(0));

            public Int32 Z
            {
                get { return (Int32)GetValue(Z_DP); }
                set { SetValue(Z_DP, value); }
            }

            public static readonly DependencyProperty Cell_DP =
                 DependencyProperty.Register("Cell", typeof(Int32), typeof
                     (StackerWorkData), new UIPropertyMetadata(0));

            public Int32 Cell
            {
                get { return (Int32)GetValue(Cell_DP); }
                set { SetValue(Cell_DP, value); }
            }

            public static readonly DependencyProperty cmd_DP =
                 DependencyProperty.Register("cmd", typeof(StackerCommand), typeof
                     (StackerWorkData), new UIPropertyMetadata(null));
            
            public StackerCommand cmd
            {
                get { return (StackerCommand)GetValue(cmd_DP); }
                set { SetValue(cmd_DP, value); }
            }
        */
            private Int32 _X;
            public Int32 X
            {
                get
                {
                    return _X;
                }
                set
                {
                    _X = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("X"));
                }
            }

            private Int32 _Y;
            public Int32 Y
            {
                get
                {
                    return _Y;
                }
                set
                {
                    _Y = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Y"));
                }
            }


            private Int32 _Z;
            public Int32 Z
            {
                get
                {
                    return _Z;
                }
                set
                {
                    _Z = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Z"));
                }
            }

            private Int32 _cell;
            public Int32 Cell
            {
                get
                {
                    return _cell;
                }
                set
                {
                    _cell = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Cell"));
                }
            }

            private StackerCommand _cmd;
            public StackerCommand cmd
            {
                get
                {
                    return _cmd;
                }
                set
                {
                    _cmd = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("cmd"));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, e);
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
