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

        private SingleTileViewModel _currentSingleTileViewModel;
        private MultiTileViewModel _currentMultiTileViewModel;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ConfigViewModel(IServiceBundle serviceBundle)
        {
            _dataService = serviceBundle.DataService;
            _colorPickerService = serviceBundle.ColorPickerService;

            CurrentMultiTileViewModel = new MultiTileViewModel(serviceBundle);

            var currentTile = CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.First();
            CurrentSingleTileViewModel = new SingleTileViewModel(currentTile);

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

        public RelayCommand<GlueVm> ConfigureEdgeCommand { get; private set; }

        public RelayCommand ConfigureTileCommand { get; private set; }

        public RelayCommand AddTileCommand { get; private set; }

        public RelayCommand SaveTileCommand { get; private set; }

        public RelayCommand<string> ChangeTileDisplayColorCommand { get; private set; }

        public RelayCommand<object> DisplayTileTypeCommand { get; private set; }

        public RelayCommand<ObservableCollection<GlueVm>> AddGlueToCurrentTileCommand { get; private set; }

        public RelayCommand<IList<GlueVm>> UpdateMultipleEdgesCommand { get; private set; }

        private bool CanChangeGlueDisplayColor(string label)
        {
            return true;
        }

        private bool CanConfigureEdge(GlueVm glue)
        {
            return true;
        }

        private bool CanConfigureTile()
        {
            return true;
        }

        private bool CanAddTile()
        {
            return true;
        }

        private bool CanSaveTile()
        {
            return true;
        }

        private bool CanChangeTileDisplayColor(string label)
        {
            return true;
        }

        public bool CanDisplayTileType(object o)
        {
            return true;
        }

        public bool CanAddGlueToCurrentTile(ObservableCollection<GlueVm> glues)
        {
            return true;
        }

        public bool CanUpdateMultipleEdges(IList<GlueVm> edges)
        {
            return true;
        }

        private void ChangeGlueDisplayColor(string label)
        {
            _colorPickerService.ShowColorPicker(c =>
            {
                foreach (var glue in CurrentMultiTileViewModel.Glues.Where(e => e.Label == label))
                {
                    glue.DisplayColor = c;
                }
            });
        }

        private void ConfigureEdge(GlueVm glue)
        {
            CurrentSingleTileViewModel.GlueVmList.GlueVms.Add(glue);
            CurrentSingleTileViewModel.GlueVmList.GlueVms = new ObservableCollection<GlueVm>(CurrentSingleTileViewModel.GlueVmList.GlueVms.Distinct());
            CurrentSingleTileViewModel.CurrentEditorModel = CurrentSingleTileViewModel.GlueVmList;
        }

        private void ConfigureTile()
        {
            CurrentSingleTileViewModel.GlueVmList.GlueVms.Clear();
            CurrentSingleTileViewModel.CurrentEditorModel = CurrentSingleTileViewModel.CurrentTileTypeVm;
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

        private void SaveTile()
        {
            _dataService.TileAssemblySystem.TileTypes[CurrentSingleTileViewModel.CurrentTileTypeVm.Label] = TileTypeVm.ToTileType(CurrentSingleTileViewModel.CurrentTileTypeVm);
            _dataService.Commit();
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

        public void AddGlueToCurrentTile(ObservableCollection<GlueVm> glues)
        {
            // fetch default glue from data service
            var glue = new Glue();
            _dataService.NewDefaultGlue((item, error) => glue = item);

            // add glue to view model
            glues.Add(GlueVm.ToGlueVm(glue));

            // stage the update in model
            _dataService.TileAssemblySystem.TileTypes[CurrentSingleTileViewModel.CurrentTileTypeVm.Label] = TileTypeVm.ToTileType(CurrentSingleTileViewModel.CurrentTileTypeVm);
        }

        public void UpdateMultipleEdges(IList<GlueVm> edges)
        {
        }
    }
}