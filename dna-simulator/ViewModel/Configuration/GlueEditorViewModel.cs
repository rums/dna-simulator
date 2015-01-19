using dna_simulator.Model;
using dna_simulator.ViewModel.Atam;

namespace dna_simulator.ViewModel.Configuration
{
    /* This class basically exists so we can use it with datatemplates */

    public class GlueEditorViewModel : ViewModelBase
    {
        private ObservableSet<GlueVm> _glues;

        public GlueEditorViewModel()
        {
            _glues = new ObservableSet<GlueVm>();
        }

        public ObservableSet<GlueVm> Glues
        {
            get { return _glues; }
            set
            {
                if (Equals(value, _glues)) return;
                _glues = value;
                RaisePropertyChanged();
            }
        }
    }
}