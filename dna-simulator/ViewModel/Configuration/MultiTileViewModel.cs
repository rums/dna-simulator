using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using dna_simulator.Exceptions;
using dna_simulator.Model;
using dna_simulator.Model.Atam;
using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace dna_simulator.ViewModel.Configuration
{
    public class MultiTileViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly NotifyCollectionChangedEventHandler _onGluesOnCollectionChanged;
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

            // initialize commands
            AddTileCommand = new RelayCommand(AddTile);
            DeleteTilesCommand = new RelayCommand<object>(DeleteTiles);
            FocusTileTypeCommand = new RelayCommand<TileTypeVm>(FocusTileType);
            AddGlueCommand = new RelayCommand(AddGlue);
            DeleteGluesCommand = new RelayCommand<object>(DeleteGlues);
            SaveChangesCommand = new RelayCommand(SaveChanges);

            // Register event handlers
            _dataService.PropertyChanged += DataServiceOnPropertyChanged;

            _onGluesOnCollectionChanged = (s, e) => GluesOnCollectionChanged(e, Glues);
            _dataService.Glues.CollectionChanged += _onGluesOnCollectionChanged;
            _dataService.TileAssemblySystem.PropertyChanged += TileAssemblySystemOnPropertyChanged;
            _dataService.TileAssemblySystem.TileTypes.CollectionChanged += TileTypesOnCollectionChanged;
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

        public RelayCommand AddTileCommand { get; private set; }

        public RelayCommand<object> DeleteTilesCommand { get; private set; }

        public RelayCommand<TileTypeVm> FocusTileTypeCommand { get; private set; }

        public RelayCommand AddGlueCommand { get; private set; }

        public RelayCommand<object> DeleteGluesCommand { get; private set; }

        public RelayCommand SaveChangesCommand { get; private set; }

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
                }
            if (e.OldItems != null)
                foreach (KeyValuePair<string, TileType> item in e.OldItems)
                {
                    CurrentTileAssemblySystemVm.TileTypes.Remove(
                        CurrentTileAssemblySystemVm.TileTypes.First(t => t.Label == item.Value.Label));
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
                        TileTypes =
                            new ObservableSet<TileTypeVm>(
                                _dataService.TileAssemblySystem.TileTypes.Values.Select(
                                    t => new TileTypeVm(t, _dataService)))
                    };
                    break;

                case "Glues":
                    Glues.CollectionChanged -= _onGluesOnCollectionChanged;
                    Glues = new ObservableSet<GlueVm>(_dataService.Glues.Values.Select(g => new GlueVm(g)));
                    break;
            }
        }

        // Command execution methods

        private void AddTile()
        {
            TileType tile = _dataService.AddTile();

            // display the new tile
            Messenger.Default.Send(new NotificationMessage<TileTypeVm>(new TileTypeVm(tile, _dataService), "Focus tile"));
        }

        private void DeleteTiles(object tiles)
        {
            List<TileTypeVm> toRemove = (tiles as IList).Cast<TileTypeVm>().ToList();
            if (toRemove == null) return;
            foreach (TileTypeVm tile in toRemove)
            {
                _dataService.TileAssemblySystem.TileTypes.Remove(tile.Label);
            }

            HideTileEditorIfEmpty();
        }

        public void AddGlue()
        {
            try
            {
                _dataService.AddGlue();
            }
            catch (InvalidTileTypeException)
            {
            }
        }

        public void DeleteGlues(object glues)
        {
            List<GlueVm> toRemove = (glues as IList).Cast<GlueVm>().ToList();
            if (toRemove == null) return;
            _dataService.RemoveGlues(toRemove.Select(GlueVm.ToGlue).ToList());
        }

        public void SaveChanges()
        {
            _dataService.Commit();
        }

        public void FocusTileType(TileTypeVm tile)
        {
            Messenger.Default.Send(new NotificationMessage<TileTypeVm>(tile, "Focus tile"));
        }

        // Regular methods

        private void HideTileEditorIfEmpty()
        {
            //if (!(CurrentSingleTileViewModel.CurrentEditorModel is TileTypeVm)) return;
            //if (CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.Count == 0)
            //{
            //    CurrentSingleTileViewModel.CurrentEditorModel = null;
            //}
        }
    }
}