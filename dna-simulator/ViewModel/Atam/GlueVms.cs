using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace dna_simulator.ViewModel.Atam
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class GlueVms : ObservableCollection<GlueVm>
    {
        public GlueVms()
        {
        }

        public GlueVms(IEnumerable<GlueVm> glues) : base(glues)
        {
        }

        protected override void InsertItem(int index, GlueVm item)
        {
            if (base.Contains(item)) return;
            base.InsertItem(index, item);
        }
    }
}