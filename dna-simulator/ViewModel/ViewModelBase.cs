﻿using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using dna_simulator.Annotations;

namespace dna_simulator.ViewModel
{
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        /// <summary>
        /// This gives us the ReSharper option to transform an autoproperty into a property with change notification
        /// Also leverages .net 4.5 callermembername attribute
        /// </summary>
        /// <param name="property">name of the property</param>
        [NotifyPropertyChangedInvocator]
        protected override void RaisePropertyChanged([CallerMemberName]string property = "")
        {
            base.RaisePropertyChanged(property);
        }
    }
}
