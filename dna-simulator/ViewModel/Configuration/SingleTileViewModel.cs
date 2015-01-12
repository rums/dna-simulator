using dna_simulator.ViewModel.Atam;

namespace dna_simulator.ViewModel.Configuration
{
    public class SingleTileViewModel : ViewModelBase
    {
        private TileTypeVm _currentTileTypeVm;
        private ViewModelBase _currentEditorModel;
        private GlueVmList _glueVmList;

        public SingleTileViewModel(TileTypeVm currentTile)
        {
            // initialize properties
            CurrentTileTypeVm = currentTile;
            CurrentEditorModel = CurrentTileTypeVm;
            _glueVmList = new GlueVmList();
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

        public GlueVmList GlueVmList
        {
            get { return _glueVmList; }
            set
            {
                if (Equals(value, _glueVmList)) return;
                _glueVmList = value;
                RaisePropertyChanged();
            }
        }
    }
}