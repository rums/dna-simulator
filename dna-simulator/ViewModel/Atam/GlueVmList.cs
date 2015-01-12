using System.Collections.ObjectModel;

namespace dna_simulator.ViewModel.Atam
{
    /* This class is just here to give DataTemplates in XAML a unique type to bind to.
     * Maybe there's a better way? */
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