using System;
using System.Collections.ObjectModel;

namespace dna_simulator.ViewModel.Atam
{
    public class GlueVmList : ViewModelBase
    {
        private ObservableCollection<GlueVm> _glueVms;

        public GlueVmList()
        {
            GlueVms = new ObservableCollection<GlueVm>();
        }

        public ObservableCollection<GlueVm> GlueVms
        {
            get { return _glueVms; }
            set
            {
                if (Equals(value, _glueVms)) return;
                _glueVms = value;
                RaisePropertyChanged();
            }
        }
    }
}
