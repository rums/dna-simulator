﻿#pragma checksum "D:\dev\workspace\Visual Studio 2013\Projects\dna-simulator\dna-simulator\View\ColorPicker.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "07C9B842637D56CFE7FF34A58E3F1422"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using InnerProduct;
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


namespace dna_simulator.View {
    
    
    public partial class ColorPicker : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.UserControl ColorPickerWindow;
        
        internal InnerProduct.ColorPickr ColorPickr;
        
        internal System.Windows.Controls.Button Ok;
        
        internal System.Windows.Controls.Button Cancel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/dna-simulator;component/View/ColorPicker.xaml", System.UriKind.Relative));
            this.ColorPickerWindow = ((System.Windows.Controls.UserControl)(this.FindName("ColorPickerWindow")));
            this.ColorPickr = ((InnerProduct.ColorPickr)(this.FindName("ColorPickr")));
            this.Ok = ((System.Windows.Controls.Button)(this.FindName("Ok")));
            this.Cancel = ((System.Windows.Controls.Button)(this.FindName("Cancel")));
        }
    }
}

