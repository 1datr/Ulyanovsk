﻿#pragma checksum "..\..\StackerControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3F2DF5222940376360301BF7B0BA0A2D"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.1022
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace WpfStackerLibrary {
    
    
    /// <summary>
    /// StackerControl
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class StackerControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\StackerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition stacker_left_panel;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\StackerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition stacker_right_panel;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\StackerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border stacker_base;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\StackerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid rack_left;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\StackerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid rack_right;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfStackerLibrary;component/stackercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\StackerControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\StackerControl.xaml"
            ((WpfStackerLibrary.StackerControl)(target)).Initialized += new System.EventHandler(this.UserControl_Initialized);
            
            #line default
            #line hidden
            
            #line 7 "..\..\StackerControl.xaml"
            ((WpfStackerLibrary.StackerControl)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded_1);
            
            #line default
            #line hidden
            return;
            case 2:
            this.stacker_left_panel = ((System.Windows.Controls.RowDefinition)(target));
            return;
            case 3:
            this.stacker_right_panel = ((System.Windows.Controls.RowDefinition)(target));
            return;
            case 4:
            this.stacker_base = ((System.Windows.Controls.Border)(target));
            return;
            case 5:
            this.rack_left = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.rack_right = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

