using System.Collections.ObjectModel;

namespace dna_simulator.ViewModel.Atam
{
    public class TileAssemblySystemVm : ViewModelBase
    {
        private ObservableCollection<TileTypeVm> _tileTypes;
        private TileTypeVm _seed;
        private int _temperature;

        public ObservableCollection<TileTypeVm> TileTypes
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
                RaisePropertyChanged();
            }
        }
    }
}
