using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;

namespace dna_simulator.ViewModel.Configuration
{
    public class SingleTileViewModel : ViewModelBase
    {
        private IServiceBundle _serviceBundle;
        private IDataService _dataService;
        private IColorPickerService _colorPickerService;

        private TileTypeVm _currentTileTypeVm;
        private ViewModelBase _currentEditorModel;
        private GlueVmList _glueVmList;

        public SingleTileViewModel(IServiceBundle serviceBundle, TileTypeVm currentTile)
        {
            _serviceBundle = serviceBundle;
            _dataService = _serviceBundle.DataService;
            _colorPickerService = _serviceBundle.ColorPickerService;

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