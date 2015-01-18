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
            //TileTypes.CollectionChanged += TileTypesVmOnCollectionChanged;
        }

        //private void TileTypesVmOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    var tileAssemblySystem = _dataService.GetTileAssemblySystem();
        //    if (e.NewItems != null)
        //        foreach (TileTypeVm item in e.NewItems)
        //        {
        //            _dataService.AddTile(TileTypeVm.ToTileType(item));
        //            var tileVm = tileAssemblySystem.TileTypes.First(t => item.Key == t.Label);
        //            item.Value.PropertyChanged += tileVm.TileTypeOnPropertyChanged;
        //            item.Value.TopGlues.CollectionChanged += OnGluesOnCollectionChanged(tileVm.TopGlues);
        //            item.Value.BottomGlues.CollectionChanged += OnGluesOnCollectionChanged(tileVm.BottomGlues);
        //            item.Value.LeftGlues.CollectionChanged += OnGluesOnCollectionChanged(tileVm.LeftGlues);
        //            item.Value.RightGlues.CollectionChanged += OnGluesOnCollectionChanged(tileVm.RightGlues);
        //        }
        //    if (e.OldItems != null)
        //        foreach (KeyValuePair<string, TileType> item in e.OldItems)
        //        {
        //            CurrentTileAssemblySystemVm.TileTypes.Remove(TileTypeVm.ToTileTypeVm(item.Value, _dataService.Glues,
        //                _dataService.TileAssemblySystem));
        //            var tileVm = CurrentTileAssemblySystemVm.TileTypes.First(t => item.Key == t.Label);
        //            item.Value.PropertyChanged -= tileVm.TileTypeOnPropertyChanged;
        //            item.Value.TopGlues.CollectionChanged -= OnGluesOnCollectionChanged(tileVm.TopGlues);
        //            item.Value.BottomGlues.CollectionChanged -= OnGluesOnCollectionChanged(tileVm.BottomGlues);
        //            item.Value.LeftGlues.CollectionChanged -= OnGluesOnCollectionChanged(tileVm.LeftGlues);
        //            item.Value.RightGlues.CollectionChanged -= OnGluesOnCollectionChanged(tileVm.RightGlues);
        //        }
        //}

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