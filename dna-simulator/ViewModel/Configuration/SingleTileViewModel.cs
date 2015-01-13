using dna_simulator.ViewModel.Atam;

namespace dna_simulator.ViewModel.Configuration
{
    public class SingleTileViewModel : ViewModelBase
    {
        private TileTypeVm _currentTileTypeVm;
        private ViewModelBase _currentEditorModel;
        private GlueEditorViewModel _glueEditorViewModel;

        public SingleTileViewModel(TileTypeVm currentTile)
        {
            // initialize properties
            CurrentTileTypeVm = currentTile;
            CurrentEditorModel = CurrentTileTypeVm;
            _glueEditorViewModel = new GlueEditorViewModel();
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

        public GlueEditorViewModel GlueEditorViewModel
        {
            get { return _glueEditorViewModel; }
            set
            {
                if (Equals(value, _glueEditorViewModel)) return;
                _glueEditorViewModel = value;
                RaisePropertyChanged();
            }
        }
    }
}