using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DragEventArgs = Microsoft.Windows.DragEventArgs;

namespace dna_simulator.Converters
{
    public class DragEventArgsToAttachedGluesVmConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var attachedGluesVm = parameter as AttachedGluesVm;
            var args = value as DragEventArgs;
            if (attachedGluesVm == null) return DependencyProperty.UnsetValue;
            if (args == null) return DependencyProperty.UnsetValue;
            var itemDragEventArgs = args.Data.GetData(typeof(ItemDragEventArgs)) as ItemDragEventArgs;
            if (itemDragEventArgs == null) return DependencyProperty.UnsetValue;
            var selectionCollection = itemDragEventArgs.Data as SelectionCollection;
            if (selectionCollection == null) return DependencyProperty.UnsetValue;
            var glue = selectionCollection[0].Item as GlueVm;
            return new AttachedGluesVm { FocusedGlue = glue, FocusedTile = attachedGluesVm.FocusedTile, FocusedEdge = attachedGluesVm.FocusedEdge };
        }
    }
}