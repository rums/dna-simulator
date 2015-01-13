using dna_simulator.ViewModel.Atam;

namespace dna_simulator.ViewModel.Configuration
{
    public class SingleTileViewModel : ViewModelBase
    {
        private TileTypeVm _currentTileTypeVm;
        private ViewModelBase _currentEditorModel;
        private GlueVms _glueVms;

        public SingleTileViewModel(TileTypeVm currentTile)
        {
            // initialize properties
            CurrentTileTypeVm = currentTile;
            CurrentEditorModel = CurrentTileTypeVm;
            _glueVms = new GlueVms();
        }

        public TileTypeVm CurrentTileTypeVm
        {
            get { return _currentTileTypeVm; }
            set
            {
                if (Equals(value, _currentTileTypeVm)) return;
                _currentTileTypeVm = value;
                RaisePropertyChanged();
            }
        }

        public ViewModelBase CurrentEditorModel
        {
            get { return _currentEditorModel; }
            set
            {
                if (Equals(value, _currentEditorModel)) return;
                _currentEditorModel = value;
                RaisePropertyChanged();
            }
        }

        public GlueVms GlueVms
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