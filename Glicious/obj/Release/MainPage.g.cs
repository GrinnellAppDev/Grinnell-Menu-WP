﻿#pragma checksum "C:\Users\Colin\documents\visual studio 2010\Projects\Glicious\Glicious\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C34176DCC09A3A53E0DE506872CF929C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Glicious {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock Glicious;
        
        internal System.Windows.Controls.TextBlock PgTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Image banner;
        
        internal Microsoft.Phone.Controls.DatePicker datePicker;
        
        internal System.Windows.Controls.Button bfastButton;
        
        internal System.Windows.Controls.Button lunchButton;
        
        internal System.Windows.Controls.Button dinnerButton;
        
        internal System.Windows.Controls.Button outtakesButton;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
        internal System.Windows.Media.GradientStop gradStart;
        
        internal System.Windows.Media.GradientStop gradStop;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Glicious;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.Glicious = ((System.Windows.Controls.TextBlock)(this.FindName("Glicious")));
            this.PgTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PgTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.banner = ((System.Windows.Controls.Image)(this.FindName("banner")));
            this.datePicker = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("datePicker")));
            this.bfastButton = ((System.Windows.Controls.Button)(this.FindName("bfastButton")));
            this.lunchButton = ((System.Windows.Controls.Button)(this.FindName("lunchButton")));
            this.dinnerButton = ((System.Windows.Controls.Button)(this.FindName("dinnerButton")));
            this.outtakesButton = ((System.Windows.Controls.Button)(this.FindName("outtakesButton")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
            this.gradStart = ((System.Windows.Media.GradientStop)(this.FindName("gradStart")));
            this.gradStop = ((System.Windows.Media.GradientStop)(this.FindName("gradStop")));
        }
    }
}

