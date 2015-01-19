using dna_simulator.Model;
using dna_simulator.Services;

namespace dna_simulator.ViewModel.Atam
{
    public class TileAssemblySystemVm : ViewModelBase
    {
        private ObservableSet<TileTypeVm> _tileTypes;
        private TileTypeVm _seed;
        private int _temperature;

        // viewmodel specific
        private IDataService _dataService;

        public TileAssemblySystemVm(IDataService dataService)
        {
            _dataService = dataService;
        }

        public ObservableSet<TileTypeVm> TileTypes
        {
            get { return _tileTypes; }
            set
            {
                if (Equals(value, _tileTypes)) return;
                _tileTypes = value;
                RaisePropertyChanged();
            }
        }

        public TileTypeVm Seed
        {
            get { return _seed; }
            set
            {
                if (Equals(value, _seed)) return;
                _seed = value;
                _dataService.SetSeed(TileTypeVm.ToTileType(value));
                RaisePropertyChanged();
            }
        }

        public int Temperature
        {
            get { return _temperature; }
            set
            {
                if (value == _temperature) return;
                _temperature = value;
                _dataService.SetTemperature(value);
                RaisePropertyChanged();
            }
        }
    }
}