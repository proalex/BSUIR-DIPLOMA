﻿#pragma checksum "..\..\..\Windows\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1A58E8FDEE41D347720B6BA340BD86FF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Client;
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


namespace Client {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label infrastructureLoadingRate;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView generatorsControl;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label2;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView tasksConrtol;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button changeUserButton;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox profileControl;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label3;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button chooseProfileButton;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button createNewProfileButton;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button reportListButton;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button deleteProfileButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Client;component/windows/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\Windows\MainWindow.xaml"
            ((Client.MainWindow)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.infrastructureLoadingRate = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.generatorsControl = ((System.Windows.Controls.ListView)(target));
            return;
            case 5:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.label2 = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.tasksConrtol = ((System.Windows.Controls.ListView)(target));
            return;
            case 8:
            this.changeUserButton = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\Windows\MainWindow.xaml"
            this.changeUserButton.Click += new System.Windows.RoutedEventHandler(this.changeUserButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.profileControl = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 10:
            this.label3 = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.chooseProfileButton = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\Windows\MainWindow.xaml"
            this.chooseProfileButton.Click += new System.Windows.RoutedEventHandler(this.chooseProfileButton_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.createNewProfileButton = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\Windows\MainWindow.xaml"
            this.createNewProfileButton.Click += new System.Windows.RoutedEventHandler(this.createNewProfileButton_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.reportListButton = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\Windows\MainWindow.xaml"
            this.reportListButton.Click += new System.Windows.RoutedEventHandler(this.reportListButton_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.deleteProfileButton = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\Windows\MainWindow.xaml"
            this.deleteProfileButton.Click += new System.Windows.RoutedEventHandler(this.deleteProfileButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

