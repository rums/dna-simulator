using System;
using System.Collections.Specialized;
using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
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
        private ObservableCollection<GlueVm> _glues; 

        public MultiTileViewModel(IServiceBundle serviceBundle)
        {
            _serviceBundle = serviceBundle;
            _dataService = _serviceBundle.DataService;
            var tileAssemblySystem = _dataService.TileAssemblySystem;

            CurrentTileAssemblySystemVm = new TileAssemblySystemVm
            {
                Temperature = tileAssemblySystem.Temperature,
                TileTypes = new ObservableCollection<TileTypeVm>(tileAssemblySystem.TileTypes.Values.Select(t => TileTypeVm.ToTileTypeVm(t, tileAssemblySystem))),
                Seed = TileTypeVm.ToTileTypeVm(tileAssemblySystem.Seed, tileAssemblySystem)
            };

            Glues = new ObservableCollection<GlueVm>(CurrentTileAssemblySystemVm.TileTypes.SelectMany(
                        t => t.TopEdges.Union(t.BottomEdges.Union(t.LeftEdges.Union(t.RightEdges))).ToList()).ToList());

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

        public ObservableCollection<GlueVm> Glues
        {
            get
            {
                return _glues;
                //return new ObservableCollection<GlueVm>(CurrentTileAssemblySystemVm.TileTypes.SelectMany(
                //      t => t.TopEdges.Union(t.BottomEdges.Union(t.LeftEdges.Union(t.RightEdges))).ToList()).ToList());
            }
            set
            {
                if (Equals(value, _glues)) return;
                _glues = value;
                RaisePropertyChanged();
            }
        }

        private void TileTypesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            RaisePropertyChanged("Glues");
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