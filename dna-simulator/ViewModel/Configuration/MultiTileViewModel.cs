using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace dna_simulator.ViewModel.Configuration
{
    public class MultiTileViewModel : ViewModelBase
    {
        private IServiceBundle _serviceBundle;
        private IDataService _dataService;

        private TileAssemblySystemVm _currentTileAssemblySystemVm;

        public MultiTileViewModel(IServiceBundle serviceBundle)
        {
            _serviceBundle = serviceBundle;
            _dataService = _serviceBundle.DataService;
            var tileAssemblySystem = _dataService.TileAssemblySystem;

            // Initialize TileAssemblySystemVm
            CurrentTileAssemblySystemVm = new TileAssemblySystemVm
            {
                Temperature = tileAssemblySystem.Temperature,
                TileTypes = new ObservableCollection<TileTypeVm>(tileAssemblySystem.TileTypes.Values.Select(t => TileTypeVm.ToTileTypeVm(t, tileAssemblySystem))),
                Seed = TileTypeVm.ToTileTypeVm(tileAssemblySystem.Seed, tileAssemblySystem)
            };

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

        private void DataServiceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TileAssemblySystem":
                    // Note: This code may cause bugs; for example, SingleTileViewModel.CurrentTileTypeVm will reference the old object.
                    //CurrentTileAssemblySystemVm.TileTypes = new ObservableCollection<TileTypeVm>(_dataService
                    //    .TileAssemblySystem
                    //    .TileTypes
                    //    .Values
                    //    .Select(t => TileTypeVm.ToTileTypeVm(t, _dataService.TileAssemblySystem)));
                    RaisePropertyChanged("CurrentTileAssemblySystemVm");
                    break;
            }
        }
    }
}