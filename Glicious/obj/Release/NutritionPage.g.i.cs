﻿#pragma checksum "C:\Users\Colin\documents\visual studio 2010\Projects\Glicious\Glicious\NutritionPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E8AF861745AB79E155C475109198CB1C"
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
    
    
    public partial class NutritionPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock Glicious;
        
        internal System.Windows.Controls.TextBlock PgTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock nutrTxt;
        
        internal System.Windows.Controls.TextBlock nutrVals;
        
        internal System.Windows.Controls.Image image1;
        
        internal System.Windows.Controls.TextBlock dishName;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Glicious;component/NutritionPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.Glicious = ((System.Windows.Controls.TextBlock)(this.FindName("Glicious")));
            this.PgTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PgTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.nutrTxt = ((System.Windows.Controls.TextBlock)(this.FindName("nutrTxt")));
            this.nutrVals = ((System.Windows.Controls.TextBlock)(this.FindName("nutrVals")));
            this.image1 = ((System.Windows.Controls.Image)(this.FindName("image1")));
            this.dishName = ((System.Windows.Controls.TextBlock)(this.FindName("dishName")));
            this.gradStart = ((System.Windows.Media.GradientStop)(this.FindName("gradStart")));
            this.gradStop = ((System.Windows.Media.GradientStop)(this.FindName("gradStop")));
        }
    }
}

