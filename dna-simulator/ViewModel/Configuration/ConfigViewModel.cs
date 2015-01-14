using dna_simulator.Model.Atam;
using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;
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
            ConfigureGlueCommand = new RelayCommand<GlueVm>(ConfigureGlue, CanConfigureGlue);
            ConfigureTileCommand = new RelayCommand(ConfigureTile, CanConfigureTile);
            AddTileCommand = new RelayCommand(AddTile, CanAddTile);
            SaveTileCommand = new RelayCommand(SaveTile, CanSaveTile);
            ChangeTileDisplayColorCommand = new RelayCommand<string>(ChangeTileDisplayColor, CanChangeTileDisplayColor);
            DisplayTileTypeCommand = new RelayCommand<object>(DisplayTileType, CanDisplayTileType);
            AddGlueToCurrentTileCommand = new RelayCommand<GlueVms>(AddGlueToCurrentTile, CanAddGlueToCurrentTile);
            RemoveGluesFromTopCommand = new RelayCommand<object>(RemoveGluesFromTop, CanRemoveGluesFromTop);
            RemoveGluesFromBottomCommand = new RelayCommand<object>(RemoveGluesFromBottom, CanRemoveGluesFromBottom);
            RemoveGluesFromLeftCommand = new RelayCommand<object>(RemoveGluesFromLeft, CanRemoveGluesFromLeft);
            RemoveGluesFromRightCommand = new RelayCommand<object>(RemoveGluesFromRight, CanRemoveGluesFromRight);
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

        public RelayCommand<GlueVm> ConfigureGlueCommand { get; private set; }

        public RelayCommand ConfigureTileCommand { get; private set; }

        public RelayCommand AddTileCommand { get; private set; }

        public RelayCommand SaveTileCommand { get; private set; }

        public RelayCommand<string> ChangeTileDisplayColorCommand { get; private set; }

        public RelayCommand<object> DisplayTileTypeCommand { get; private set; }

        public RelayCommand<GlueVms> AddGlueToCurrentTileCommand { get; private set; }

        public RelayCommand<object> RemoveGluesFromTopCommand { get; private set; } 

        public RelayCommand<object> RemoveGluesFromBottomCommand { get; private set; } 

        public RelayCommand<object> RemoveGluesFromLeftCommand { get; private set; } 

        public RelayCommand<object> RemoveGluesFromRightCommand { get; private set; } 

        private bool CanChangeGlueDisplayColor(string label)
        {
            return true;
        }

        private bool CanConfigureGlue(GlueVm glue)
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

        public bool CanAddGlueToCurrentTile(GlueVms glues)
        {
            return true;
        }

        public bool CanRemoveGluesFromTop(object glues)
        {
            return true;
        }

        public bool CanRemoveGluesFromBottom(object glues)
        {
            return true;
        }

        public bool CanRemoveGluesFromLeft(object glues)
        {
            return true;
        }

        public bool CanRemoveGluesFromRight(object glues)
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

        private void ConfigureGlue(GlueVm glue)
        {
            if (CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Contains(glue)) return;
            CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Add(glue);
            CurrentSingleTileViewModel.CurrentEditorModel = CurrentSingleTileViewModel.GlueEditorViewModel;
        }

        private void ConfigureTile()
        {
            CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Clear();
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

        public void AddGlueToCurrentTile(GlueVms glues)
        {
            // fetch default glue from data service
            var glue = new Glue();
            _dataService.NewDefaultGlue((item, error) => glue = item);

            // add glue to view model
            CurrentMultiTileViewModel.Glues.Add(GlueVm.ToGlueVm(glue));
            glues.Add(GlueVm.ToGlueVm(glue));

            // stage the update in model
            _dataService.TileAssemblySystem.TileTypes[CurrentSingleTileViewModel.CurrentTileTypeVm.Label] = TileTypeVm.ToTileType(CurrentSingleTileViewModel.CurrentTileTypeVm);
        }

        public void RemoveGluesFromTop(object glues)
        {
            var gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            foreach (var glue in gglues)
            {
                CurrentSingleTileViewModel.CurrentTileTypeVm.TopGlues.Remove(glue);
                CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            }

            // if the glue editor is currently open and empty, switch back to tile view
            if (!(CurrentSingleTileViewModel.CurrentEditorModel is GlueEditorViewModel)) return;
            if (CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Count == 0)
            {
                ConfigureTile();
            }
        }
        public void RemoveGluesFromBottom(object glues)
        {
            var gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            foreach (var glue in gglues)
            {
                CurrentSingleTileViewModel.CurrentTileTypeVm.BottomGlues.Remove(glue);
                CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            }

            // if the glue editor is currently open and empty, switch back to tile view
            if (!(CurrentSingleTileViewModel.CurrentEditorModel is GlueEditorViewModel)) return;
            if (CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Count == 0)
            {
                ConfigureTile();
            }
        }
        public void RemoveGluesFromLeft(object glues)
        {
            var gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            foreach (var glue in gglues)
            {
                CurrentSingleTileViewModel.CurrentTileTypeVm.LeftGlues.Remove(glue);
                CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            }

            // if the glue editor is currently open and empty, switch back to tile view
            if (!(CurrentSingleTileViewModel.CurrentEditorModel is GlueEditorViewModel)) return;
            if (CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Count == 0)
            {
                ConfigureTile();
            }
        }
        public void RemoveGluesFromRight(object glues)
        {
            var gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            foreach (var glue in gglues)
            {
                CurrentSingleTileViewModel.CurrentTileTypeVm.RightGlues.Remove(glue);
                CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            }

            // if the glue editor is currently open and empty, switch back to tile view
            if (!(CurrentSingleTileViewModel.CurrentEditorModel is GlueEditorViewModel)) return;
            if (CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Count == 0)
            {
                ConfigureTile();
            }
        }

    }
}