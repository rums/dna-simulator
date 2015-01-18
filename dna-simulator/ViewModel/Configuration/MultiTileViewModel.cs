using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using dna_simulator.Model;
using dna_simulator.Model.Atam;
using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;

namespace dna_simulator.ViewModel.Configuration
{
    public class MultiTileViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private TileAssemblySystemVm _currentTileAssemblySystemVm;

        private ObservableSet<GlueVm> _glues;

        public MultiTileViewModel(IServiceBundle serviceBundle)
        {
            _dataService = serviceBundle.DataService;
            TileAssemblySystem tileAssemblySystem = _dataService.TileAssemblySystem;

            CurrentTileAssemblySystemVm = new TileAssemblySystemVm(_dataService)
            {
                Temperature = tileAssemblySystem.Temperature,
                TileTypes =
                    new ObservableSet<TileTypeVm>(
                        tileAssemblySystem.TileTypes.Values.Select(
                            t => t == null ? null : new TileTypeVm(t, _dataService))),
                Seed =
                    tileAssemblySystem.Seed == null
                        ? null
                        : new TileTypeVm(tileAssemblySystem.Seed, _dataService)
            };

            Glues = new ObservableSet<GlueVm>(_dataService.Glues.Values.Select(g => new GlueVm(g)));

            // Register event handlers
            _dataService.PropertyChanged += DataServiceOnPropertyChanged;

            _onGluesOnCollectionChanged = (s, e) => GluesOnCollectionChanged(e, Glues);
            _dataService.Glues.CollectionChanged += _onGluesOnCollectionChanged;
            _dataService.TileAssemblySystem.PropertyChanged += TileAssemblySystemOnPropertyChanged;
            _dataService.TileAssemblySystem.TileTypes.CollectionChanged += TileTypesOnCollectionChanged;
            foreach (var tile in _dataService.TileAssemblySystem.TileTypes.Values)
            {
                var tileVm = CurrentTileAssemblySystemVm.TileTypes.First(t => tile.Label == t.Label);
                tile.PropertyChanged += tileVm.TileTypeOnPropertyChanged;
                _onAttachedGluesOnCollectionChanged.Add(tileVm.TopGlues, (s, e) => AttachedGluesOnCollectionChanged(e, tileVm.TopGlues));
                tile.TopGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[tileVm.TopGlues];
                _onAttachedGluesOnCollectionChanged.Add(tileVm.BottomGlues, (s, e) => AttachedGluesOnCollectionChanged(e, tileVm.BottomGlues));
                tile.BottomGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[tileVm.BottomGlues];
                _onAttachedGluesOnCollectionChanged.Add(tileVm.LeftGlues, (s, e) => AttachedGluesOnCollectionChanged(e, tileVm.LeftGlues));
                tile.LeftGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[tileVm.LeftGlues];
                _onAttachedGluesOnCollectionChanged.Add(tileVm.RightGlues, (s, e) => AttachedGluesOnCollectionChanged(e, tileVm.RightGlues));
                tile.RightGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[tileVm.RightGlues];
            }
        }

        // Using dictionary of handlers so we can pass additional arg and unregister later
        private readonly NotifyCollectionChangedEventHandler _onGluesOnCollectionChanged;
        private readonly Dictionary<AttachedGluesVm, NotifyCollectionChangedEventHandler> _onAttachedGluesOnCollectionChanged = new Dictionary<AttachedGluesVm, NotifyCollectionChangedEventHandler>();

        private void GluesOnCollectionChanged(NotifyCollectionChangedEventArgs e, ObservableSet<GlueVm> glues)
        {
            if (e.NewItems != null)
            {
                if (e.NewItems.OfType<KeyValuePair<GlueLabel, Glue>>().Any())
                {
                    foreach (KeyValuePair<GlueLabel, Glue> item in e.NewItems)
                    {
                        glues.Add(new GlueVm(_dataService.Glues[item.Key]));
                        item.Value.PropertyChanged += glues.First(g => g.Label == item.Key.Label).GlueOnPropertyChanged;
                    }
                }
                else
                {
                    foreach (GlueLabel item in e.NewItems)
                    {
                        glues.Add(new GlueVm(_dataService.Glues[item]));
                        item.PropertyChanged += glues.First(g => g.Label == item.Label).GlueOnPropertyChanged;
                    }
                    
                }
            }
            if (e.OldItems != null)
            {
                if (e.OldItems.OfType<KeyValuePair<GlueLabel, Glue>>().Any())
                {
                    foreach (KeyValuePair<GlueLabel, Glue> item in e.OldItems)
                    {
                        item.Value.PropertyChanged -= glues.First(g => g.Label == item.Key.Label).GlueOnPropertyChanged;
                        glues.Remove(new GlueVm(item.Value));
                    }

                }
                else if (e.OldItems.OfType<GlueLabel>().Any())
                {
                    foreach (GlueLabel item in e.OldItems)
                    {
                        item.PropertyChanged -= glues.First(g => g.Label == item.Label).GlueOnPropertyChanged;
                        glues.Remove(glues.First(g => g.Label == item.Label));
                    }
                }
            }
        }
        
        private void AttachedGluesOnCollectionChanged(NotifyCollectionChangedEventArgs e, AttachedGluesVm glues)
        {
            if (e.NewItems != null)
            {
                if (e.NewItems.OfType<KeyValuePair<GlueLabel, Glue>>().Any())
                {
                    foreach (KeyValuePair<GlueLabel, Glue> item in e.NewItems)
                    {
                        glues.Add(new GlueVm(_dataService.Glues[item.Key]));
                        item.Value.PropertyChanged += glues.First(g => g.Label == item.Key.Label).GlueOnPropertyChanged;
                    }
                }
                else
                {
                    foreach (GlueLabel item in e.NewItems)
                    {
                        glues.Add(new GlueVm(_dataService.Glues[item]));
                        item.PropertyChanged += glues.First(g => g.Label == item.Label).GlueOnPropertyChanged;
                    }
                    
                }
            }
            if (e.OldItems != null)
            {
                if (e.OldItems.OfType<KeyValuePair<GlueLabel, Glue>>().Any())
                {
                    foreach (KeyValuePair<GlueLabel, Glue> item in e.OldItems)
                    {
                        item.Value.PropertyChanged -= glues.First(g => g.Label == item.Key.Label).GlueOnPropertyChanged;
                        glues.Remove(new GlueVm(item.Value));
                    }

                }
                else if (e.OldItems.OfType<GlueLabel>().Any())
                {
                    foreach (GlueLabel item in e.OldItems)
                    {
                        item.PropertyChanged -= glues.First(g => g.Label == item.Label).GlueOnPropertyChanged;
                        glues.Remove(glues.First(g => g.Label == item.Label));
                    }
                }
            }
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

        private void TileAssemblySystemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Temperature":
                    CurrentTileAssemblySystemVm.Temperature = _dataService.TileAssemblySystem.Temperature;
                    break;
                case "Seed":
                    CurrentTileAssemblySystemVm.Seed = _dataService.TileAssemblySystem.Seed == null
                        ? null
                        : new TileTypeVm(_dataService.TileAssemblySystem.Seed, _dataService);
                    break;
            }
        }

        private void TileTypesOnCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (KeyValuePair<string, TileType> item in e.NewItems)
                {
                    CurrentTileAssemblySystemVm.TileTypes.Add(new TileTypeVm(item.Value, _dataService));
                    var tileVm = CurrentTileAssemblySystemVm.TileTypes.First(t => item.Key == t.Label);
                    _onAttachedGluesOnCollectionChanged.Add(tileVm.TopGlues, (s, e2) => AttachedGluesOnCollectionChanged(e2, tileVm.TopGlues));
                    item.Value.TopGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[tileVm.TopGlues];
                    _onAttachedGluesOnCollectionChanged.Add(tileVm.BottomGlues, (s, e2) => AttachedGluesOnCollectionChanged(e2, tileVm.BottomGlues));
                    item.Value.BottomGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[tileVm.BottomGlues];
                    _onAttachedGluesOnCollectionChanged.Add(tileVm.LeftGlues, (s, e2) => AttachedGluesOnCollectionChanged(e2, tileVm.LeftGlues));
                    item.Value.LeftGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[tileVm.LeftGlues];
                    _onAttachedGluesOnCollectionChanged.Add(tileVm.RightGlues, (s, e2) => AttachedGluesOnCollectionChanged(e2, tileVm.RightGlues));
                    item.Value.RightGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[tileVm.RightGlues];
                }
            if (e.OldItems != null)
                foreach (KeyValuePair<string, TileType> item in e.OldItems)
                {
                    var tileVm = CurrentTileAssemblySystemVm.TileTypes.First(t => item.Key == t.Label);
                    item.Value.PropertyChanged -= tileVm.TileTypeOnPropertyChanged;
                    item.Value.TopGlues.CollectionChanged -= _onAttachedGluesOnCollectionChanged[tileVm.TopGlues];
                    _onAttachedGluesOnCollectionChanged.Remove(tileVm.TopGlues);
                    item.Value.BottomGlues.CollectionChanged -= _onAttachedGluesOnCollectionChanged[tileVm.BottomGlues];
                    _onAttachedGluesOnCollectionChanged.Remove(tileVm.BottomGlues);
                    item.Value.LeftGlues.CollectionChanged -= _onAttachedGluesOnCollectionChanged[tileVm.LeftGlues];
                    _onAttachedGluesOnCollectionChanged.Remove(tileVm.LeftGlues);
                    item.Value.RightGlues.CollectionChanged -= _onAttachedGluesOnCollectionChanged[tileVm.RightGlues];
                    _onAttachedGluesOnCollectionChanged.Remove(tileVm.RightGlues);
                    CurrentTileAssemblySystemVm.TileTypes.Remove(CurrentTileAssemblySystemVm.TileTypes.First(t => t.Label == item.Value.Label));
                }
        }

        private void DataServiceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TileAssemblySystem":
                    CurrentTileAssemblySystemVm = new TileAssemblySystemVm(_dataService)
                    {
                        Temperature = _dataService.TileAssemblySystem.Temperature,
                        Seed =
                            _dataService.TileAssemblySystem.Seed == null
                                ? null
                                : new TileTypeVm(_dataService.TileAssemblySystem.Seed, _dataService),
                        TileTypes = new ObservableSet<TileTypeVm>(_dataService.TileAssemblySystem.TileTypes.Values.Select(t => new TileTypeVm(t, _dataService)))
                    };
                    break;
                case "Glues":
                    Glues.CollectionChanged -= _onGluesOnCollectionChanged;
                    Glues = new ObservableSet<GlueVm>(_dataService.Glues.Values.Select(g => new GlueVm(g)));
                    break;
            }
        }
    }
}