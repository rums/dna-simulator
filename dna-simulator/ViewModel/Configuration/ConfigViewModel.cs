using dna_simulator.Model.Atam;
using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace dna_simulator.ViewModel.Configuration
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// </summary>
    public class ConfigViewModel : ViewModelBase
    {
        private IServiceBundle _serviceBundle;
        private IDataService _dataService;
        private IColorPickerService _colorPickerService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ConfigViewModel(IServiceBundle serviceBundle)
        {
            _serviceBundle = serviceBundle;
            _dataService = _serviceBundle.DataService;
            _colorPickerService = _serviceBundle.ColorPickerService;

            CurrentMultiTileViewModel = new MultiTileViewModel(_serviceBundle);

            var currentTile = CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.First();
            CurrentSingleTileViewModel = new SingleTileViewModel(_serviceBundle, currentTile);

            // initialize commands
            ChangeGlueDisplayColorCommand = new RelayCommand<string>(ChangeGlueDisplayColor, CanChangeGlueDisplayColor);
            ConfigureEdgeCommand = new RelayCommand<GlueVm>(ConfigureEdge, CanConfigureEdge);
            ConfigureTileCommand = new RelayCommand(ConfigureTile, CanConfigureTile);
            CreateTileCommand = new RelayCommand(CreateTile, CanCreateTile);
            SaveTileCommand = new RelayCommand(SaveTile, CanSaveTile);
            ChangeTileDisplayColorCommand = new RelayCommand<string>(ChangeTileDisplayColor, CanChangeTileDisplayColor);
            DisplayTileTypeCommand = new RelayCommand<object>(DisplayTileType, CanDisplayTileType);
            UpdateMultipleEdgesCommand = new RelayCommand<IList<GlueVm>>(UpdateMultipleEdges, CanUpdateMultipleEdges);
        }

        private SingleTileViewModel _currentSingleTileViewModel;
        private MultiTileViewModel _currentMultiTileViewModel;

        public SingleTileViewModel CurrentSingleTileViewModel
        {
            get { return _currentSingleTileViewModel; }
            set
            {
                if (Equals(value, _currentSingleTileViewModel)) return;
                _currentSingleTileViewModel = value;
                RaisePropertyChanged();
            }
        }

        public MultiTileViewModel CurrentMultiTileViewModel
        {
            get { return _currentMultiTileViewModel; }
            set
            {
                if (Equals(value, _currentMultiTileViewModel)) return;
                _currentMultiTileViewModel = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<string> ChangeGlueDisplayColorCommand { get; private set; }

        private bool CanChangeGlueDisplayColor(string label)
        {
            return true;
        }

        private void ChangeGlueDisplayColor(string label)
        {
            _colorPickerService.ShowColorPicker(c =>
            {
                foreach (var glue in CurrentMultiTileViewModel.Edges.Where(e => e.Label == label))
                {
                    glue.DisplayColor = c;
                }
            });
        }

        public RelayCommand<GlueVm> ConfigureEdgeCommand { get; private set; }

        private bool CanConfigureEdge(GlueVm glue)
        {
            return true;
        }

        private void ConfigureEdge(GlueVm glue)
        {
            CurrentSingleTileViewModel.GlueVmList.GlueVms.Add(glue);
            CurrentSingleTileViewModel.GlueVmList.GlueVms = new ObservableCollection<GlueVm>(CurrentSingleTileViewModel.GlueVmList.GlueVms.Distinct());
            CurrentSingleTileViewModel.CurrentEditorModel = CurrentSingleTileViewModel.GlueVmList;
        }

        public RelayCommand ConfigureTileCommand { get; private set; }

        private bool CanConfigureTile()
        {
            return true;
        }

        private void ConfigureTile()
        {
            CurrentSingleTileViewModel.GlueVmList.GlueVms.Clear();
            CurrentSingleTileViewModel.CurrentEditorModel = CurrentSingleTileViewModel.CurrentTileTypeVm;
        }

        public RelayCommand CreateTileCommand { get; private set; }

        private bool CanCreateTile()
        {
            return true;
        }

        private void CreateTile()
        {
            // fetch tile from data service
            var tile = new TileType();
            _dataService.NewDefaultTile((item, error) => tile = item);
            // save new tile
            _dataService.TileAssemblySystem.TileTypes.Add(tile.Id, tile);
            // add new tile to view model
            CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.Add(TileTypeVm.ToTileTypeVm(tile, _dataService.TileAssemblySystem));
            CurrentSingleTileViewModel.CurrentTileTypeVm = CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.Last();

            // display the new tile
            DisplayTileType(CurrentSingleTileViewModel.CurrentTileTypeVm);
        }

        public RelayCommand SaveTileCommand { get; private set; }

        private bool CanSaveTile()
        {
            return true;
        }

        private void SaveTile()
        {
            _dataService.TileAssemblySystem.TileTypes[CurrentSingleTileViewModel.CurrentTileTypeVm.Id] = TileTypeVm.ToTileTypeBase(CurrentSingleTileViewModel.CurrentTileTypeVm);
            _dataService.Commit();
        }

        public RelayCommand<string> ChangeTileDisplayColorCommand { get; private set; }

        private bool CanChangeTileDisplayColor(string label)
        {
            return true;
        }

        private void ChangeTileDisplayColor(string label)
        {
            _colorPickerService.ShowColorPicker(c =>
            {
                foreach (var tile in CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.Where(e => e.Label == label))
                {
                    tile.DisplayColor = c;
                }
            });
        }

        public RelayCommand<object> DisplayTileTypeCommand { get; private set; }

        public bool CanDisplayTileType(object o)
        {
            return true;
        }

        public void DisplayTileType(object o)
        {
            // save current tile
            SaveTile();

            // switch tile context
            var tile = o as TileTypeVm;
            CurrentSingleTileViewModel.CurrentTileTypeVm = tile;

            // we should be configuring the new tile
            ConfigureTile();
        }

        public RelayCommand<IList<GlueVm>> UpdateMultipleEdgesCommand { get; private set; }

        public bool CanUpdateMultipleEdges(IList<GlueVm> edges)
        {
            return true;
        }

        public void UpdateMultipleEdges(IList<GlueVm> edges)
        {
        }
    }
}