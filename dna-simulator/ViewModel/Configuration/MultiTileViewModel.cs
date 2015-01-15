using dna_simulator.Model;
using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace dna_simulator.ViewModel.Configuration
{
    public class MultiTileViewModel : ViewModelBase
    {
        private IDataService _dataService;

        private TileAssemblySystemVm _currentTileAssemblySystemVm;

        private ObservableSet<GlueVm> _glues; 

        public MultiTileViewModel(IServiceBundle serviceBundle)
        {
            _dataService = serviceBundle.DataService;
            var tileAssemblySystem = _dataService.TileAssemblySystem;

            CurrentTileAssemblySystemVm = new TileAssemblySystemVm
            {
                Temperature = tileAssemblySystem.Temperature,
                TileTypes = new ObservableCollection<TileTypeVm>(tileAssemblySystem.TileTypes.Values.Select(t => TileTypeVm.ToTileTypeVm(t, tileAssemblySystem))),
                Seed = TileTypeVm.ToTileTypeVm(tileAssemblySystem.Seed, tileAssemblySystem)
            };

            Glues = new ObservableSet<GlueVm>(CurrentTileAssemblySystemVm.TileTypes.SelectMany(
                        t => t.TopGlues.Union(t.BottomGlues.Union(t.LeftGlues.Union(t.RightGlues))).ToList()).ToList());

            // Register event handlers
            _dataService.PropertyChanged += DataServiceOnPropertyChanged;
        }

        public TileAssemblySystemVm CurrentTileAssemblySystemVm
        {
            get { return _currentTileAssemblySystemVm; }
            set
            {
                if (Equals(value, _currentTileAssemblySystemVm)) return;
                _currentTileAssemblySystemVm = value;
                RaisePropertyChanged();
            }
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


        private void DataServiceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TileAssemblySystem":
                    RaisePropertyChanged("CurrentTileAssemblySystemVm");
                    RaisePropertyChanged("Glues");
                    break;
            }
        }
    }
}