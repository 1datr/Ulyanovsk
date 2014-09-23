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
    }


    public class StackerWorkData : INotifyPropertyChanged
        {
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

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, e);
            }
        }
}
