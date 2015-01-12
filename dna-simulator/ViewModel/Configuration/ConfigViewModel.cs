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
        private IDataService _dataService;
        private IColorPickerService _colorPickerService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ConfigViewModel(IServiceBundle serviceBundle)
        {
            _dataService = serviceBundle.DataService;
            _colorPickerService = serviceBundle.ColorPickerService;

            CurrentMultiTileViewModel = new MultiTileViewModel(serviceBundle);

            var currentTile = CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.First();
            CurrentSingleTileViewModel = new SingleTileViewModel(serviceBundle, currentTile);

            // initialize commands
            ChangeGlueDisplayColorCommand = new RelayCommand<string>(ChangeGlueDisplayColor, CanChangeGlueDisplayColor);
            ConfigureEdgeCommand = new RelayCommand<GlueVm>(ConfigureEdge, CanConfigureEdge);
            ConfigureTileCommand = new RelayCommand(ConfigureTile, CanConfigureTile);
            AddTileCommand = new RelayCommand(AddTile, CanAddTile);
            SaveTileCommand = new RelayCommand(SaveTile, CanSaveTile);
            ChangeTileDisplayColorCommand = new RelayCommand<string>(ChangeTileDisplayColor, CanChangeTileDisplayColor);
            DisplayTileTypeCommand = new RelayCommand<object>(DisplayTileType, CanDisplayTileType);
            UpdateMultipleEdgesCommand = new RelayCommand<IList<GlueVm>>(UpdateMultipleEdges, CanUpdateMultipleEdges);
            AddGlueToCurrentTileCommand = new RelayCommand<ObservableCollection<GlueVm>>(AddGlueToCurrentTile, CanAddGlueToCurrentTile);
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

        public RelayCommand AddTileCommand { get; private set; }

        private bool CanAddTile()
        {
            return true;
        }

        private void AddTile()
        {
            // fetch tile from data service
            var tile = new TileType();
            _dataService.NewDefaultTile((item, error) => tile = item);

            // stage new tile in model
            _dataService.TileAssemblySystem.TileTypes.Add(tile.Label, tile);

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
            _dataService.TileAssemblySystem.TileTypes[CurrentSingleTileViewModel.CurrentTileTypeVm.Label] = TileTypeVm.ToTileType(CurrentSingleTileViewModel.CurrentTileTypeVm);
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

        public RelayCommand<ObservableCollection<GlueVm>> AddGlueToCurrentTileCommand { get; private set; }

        public bool CanAddGlueToCurrentTile(ObservableCollection<GlueVm> glues)
        {
            return true;
        }

        public void AddGlueToCurrentTile(ObservableCollection<GlueVm> glues)
        {
            // fetch default glue from data service
            var glue = new Glue();
            _dataService.NewDefaultGlue((item, error) => glue = item);

            // stage the update in model
            _dataService.TileAssemblySystem.TileTypes[CurrentSingleTileViewModel.CurrentTileTypeVm.Label] = TileTypeVm.ToTileType(CurrentSingleTileViewModel.CurrentTileTypeVm);

            // add glue to view model
            glues.Add(GlueVm.ToGlueVm(glue));
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