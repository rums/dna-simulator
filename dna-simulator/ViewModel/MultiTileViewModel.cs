using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using dna_simulator.Model;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace dna_simulator.ViewModel
{
    public class MultiTileViewModel : ViewModelBase
    {
        private IDataService _dataService;

        #region Constructors

        public MultiTileViewModel(IDataService dataService)
        {
            _dataService = dataService;
            var tileAssemblySystem = _dataService.TileAssemblySystem;

            // Initialize TileAssemblySystemVm
            CurrentTileAssemblySystemVm = new TileAssemblySystemVm
            {
                Temperature = tileAssemblySystem.Temperature,
                TileTypes = new ObservableCollection<TileTypeVm>(),
                Seed = TileTypeVm.ToTileTypeVm(tileAssemblySystem.Seed, tileAssemblySystem)
            };
            foreach (var tvm in tileAssemblySystem.TileTypes.Select(t => new TileTypeVm
            {
                DisplayColor = t.Value.DisplayColor,
                Label = t.Value.Label,
                Top = new GlueVm { Color = t.Value.Top.Color, Strength = t.Value.Top.Strength, DisplayColor = t.Value.Top.DisplayColor },
                Bottom = new GlueVm { Color = t.Value.Bottom.Color, Strength = t.Value.Bottom.Strength, DisplayColor = t.Value.Bottom.DisplayColor },
                Left = new GlueVm { Color = t.Value.Left.Color, Strength = t.Value.Left.Strength, DisplayColor = t.Value.Left.DisplayColor },
                Right = new GlueVm { Color = t.Value.Right.Color, Strength = t.Value.Right.Strength, DisplayColor = t.Value.Right.DisplayColor },
                IsSeed = (tileAssemblySystem.Seed.Label == t.Value.Label)
            }))
            {
                CurrentTileAssemblySystemVm.TileTypes.Add(tvm);
            }

            // Register event handlers
            _dataService.PropertyChanged += DataServiceOnPropertyChanged;

            // initialize commands
            DisplayTileTypeCommand = new RelayCommand<object>(ExecuteDisplayTileType, CanDisplayTileType);
        }

        #endregion
        #region Properties

        private TileAssemblySystemVm _currentTileAssemblySystemVm;

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

        #endregion Properties

        #region Commands

        public RelayCommand<object> DisplayTileTypeCommand { get; private set; }

        public bool CanDisplayTileType(object o)
        {
            return true;
        }

        public void ExecuteDisplayTileType(object o)
        {
            var tile = o as TileTypeVm;
            Messenger.Default.Send(new NotificationMessage<TileTypeVm>(tile, "DisplayTile"));
        }

        #endregion

        #region Event handlers


        private void DataServiceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TileAssemblySystem":
                    CurrentTileAssemblySystemVm.TileTypes = new ObservableCollection<TileTypeVm>(_dataService
                        .TileAssemblySystem
                        .TileTypes
                        .Values
                        .Select(t => TileTypeVm.ToTileTypeVm(t, _dataService.TileAssemblySystem)));
                    RaisePropertyChanged("CurrentTileAssemblySystemVm");
                    break;
            }
        }

        #endregion
    }
}