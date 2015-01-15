using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using dna_simulator.Model;
using dna_simulator.Model.Atam;
using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;

namespace dna_simulator.ViewModel.Configuration
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    /// </summary>
    public class ConfigViewModel : ViewModelBase
    {
        private readonly IColorPickerService _colorPickerService;
        private readonly IDataService _dataService;

        private MultiTileViewModel _currentMultiTileViewModel;
        private SingleTileViewModel _currentSingleTileViewModel;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ConfigViewModel(IServiceBundle serviceBundle)
        {
            _dataService = serviceBundle.DataService;
            _colorPickerService = serviceBundle.ColorPickerService;

            CurrentMultiTileViewModel = new MultiTileViewModel(serviceBundle);

            TileTypeVm currentTile = CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.First();
            CurrentSingleTileViewModel = new SingleTileViewModel(currentTile);

            // initialize commands
            ChangeGlueDisplayColorCommand = new RelayCommand<string>(ChangeGlueDisplayColor, CanChangeGlueDisplayColor);
            ConfigureGlueCommand = new RelayCommand<GlueVm>(ConfigureGlue, CanConfigureGlue);
            ConfigureTileCommand = new RelayCommand(ConfigureTile, CanConfigureTile);
            AddTileCommand = new RelayCommand(AddTile, CanAddTile);
            DeleteTilesCommand = new RelayCommand<object>(DeleteTiles, CanDeleteTiles);
            SaveTileCommand = new RelayCommand(SaveTile, CanSaveTile);
            ChangeTileDisplayColorCommand = new RelayCommand<string>(ChangeTileDisplayColor, CanChangeTileDisplayColor);
            DisplayTileTypeCommand = new RelayCommand<object>(DisplayTileType, CanDisplayTileType);
            AddGlueCommand = new RelayCommand<ObservableSet<GlueVm>>(AddGlue,
                CanAddGlue);
            RemoveGluesFromTopCommand = new RelayCommand<object>(RemoveGluesFromTop, CanRemoveGluesFromTop);
            RemoveGluesFromBottomCommand = new RelayCommand<object>(RemoveGluesFromBottom, CanRemoveGluesFromBottom);
            RemoveGluesFromLeftCommand = new RelayCommand<object>(RemoveGluesFromLeft, CanRemoveGluesFromLeft);
            RemoveGluesFromRightCommand = new RelayCommand<object>(RemoveGluesFromRight, CanRemoveGluesFromRight);
            DeleteGluesCommand = new RelayCommand<object>(DeleteGlues, CanDeleteGlues);
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

        public RelayCommand<object> DeleteTilesCommand { get; private set; }

        public RelayCommand SaveTileCommand { get; private set; }

        public RelayCommand<string> ChangeTileDisplayColorCommand { get; private set; }

        public RelayCommand<object> DisplayTileTypeCommand { get; private set; }

        public RelayCommand<ObservableSet<GlueVm>> AddGlueCommand { get; private set; }

        public RelayCommand<object> RemoveGluesFromTopCommand { get; private set; }

        public RelayCommand<object> RemoveGluesFromBottomCommand { get; private set; }

        public RelayCommand<object> RemoveGluesFromLeftCommand { get; private set; }

        public RelayCommand<object> RemoveGluesFromRightCommand { get; private set; }

        public RelayCommand<object> DeleteGluesCommand { get; private set; }

        /* Command canExecute methods */

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

        private bool CanDeleteTiles(object tiles)
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

        public bool CanAddGlue(ObservableSet<GlueVm> glues)
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

        public bool CanDeleteGlues(object glues)
        {
            return true;
        }

        /* Command execution methods */

        private void ChangeGlueDisplayColor(string label)
        {
            _colorPickerService.ShowColorPicker(c =>
            {
                foreach (GlueVm glue in CurrentMultiTileViewModel.Glues.Where(e => e.Label == label))
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
            CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.Add(TileTypeVm.ToTileTypeVm(tile,
                _dataService.TileAssemblySystem));
            CurrentSingleTileViewModel.CurrentTileTypeVm =
                CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.Last();

            // display the new tile
            DisplayTileType(CurrentSingleTileViewModel.CurrentTileTypeVm);
        }

        private void DeleteTiles(object tiles)
        {
            List<TileTypeVm> toRemove = (tiles as IList).Cast<TileTypeVm>().ToList();
            if (toRemove == null) return;
            foreach (TileTypeVm tile in toRemove)
            {
                CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.Remove(tile);
            }

            HideTileEditorIfEmpty();
        }

        private void SaveTile()
        {
            _dataService.TileAssemblySystem.TileTypes[CurrentSingleTileViewModel.CurrentTileTypeVm.Label] =
                TileTypeVm.ToTileType(CurrentSingleTileViewModel.CurrentTileTypeVm);
            _dataService.Commit();
        }

        private void ChangeTileDisplayColor(string label)
        {
            _colorPickerService.ShowColorPicker(c =>
            {
                foreach (
                    TileTypeVm tile in
                        CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.Where(e => e.Label == label))
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

        public void AddGlue(ObservableSet<GlueVm> glues)
        {
            // fetch default glue from data service
            var glue = new Glue();
            _dataService.NewDefaultGlue((item, error) => glue = item);

            // add glue to view model
            glues.Add(GlueVm.ToGlueVm(glue));
            if (glues != CurrentMultiTileViewModel.Glues)
            {
                CurrentMultiTileViewModel.Glues.Add(GlueVm.ToGlueVm(glue));
            }

            // stage the update in model
            _dataService.TileAssemblySystem.TileTypes[CurrentSingleTileViewModel.CurrentTileTypeVm.Label] =
                TileTypeVm.ToTileType(CurrentSingleTileViewModel.CurrentTileTypeVm);
        }

        public void RemoveGluesFromTop(object glues)
        {
            List<GlueVm> gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            foreach (GlueVm glue in gglues)
            {
                CurrentSingleTileViewModel.CurrentTileTypeVm.TopGlues.Remove(glue);
                CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            }

            HideGlueEditorIfEmpty();
        }

        public void RemoveGluesFromBottom(object glues)
        {
            List<GlueVm> gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            foreach (GlueVm glue in gglues)
            {
                CurrentSingleTileViewModel.CurrentTileTypeVm.BottomGlues.Remove(glue);
                CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            }

            HideGlueEditorIfEmpty();
        }

        public void RemoveGluesFromLeft(object glues)
        {
            List<GlueVm> gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            foreach (GlueVm glue in gglues)
            {
                CurrentSingleTileViewModel.CurrentTileTypeVm.LeftGlues.Remove(glue);
                CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            }

            HideGlueEditorIfEmpty();
        }

        public void RemoveGluesFromRight(object glues)
        {
            List<GlueVm> gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            foreach (GlueVm glue in gglues)
            {
                CurrentSingleTileViewModel.CurrentTileTypeVm.RightGlues.Remove(glue);
                CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            }

            HideGlueEditorIfEmpty();
        }

        public void DeleteGlues(object glues)
        {
            List<GlueVm> toRemove = (glues as IList).Cast<GlueVm>().ToList();
            if (toRemove == null) return;
            foreach (GlueVm glue in toRemove)
            {
                CurrentMultiTileViewModel.Glues.Remove(glue);
                foreach (TileTypeVm tile in CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes)
                {
                    tile.TopGlues.Remove(glue);
                    tile.BottomGlues.Remove(glue);
                    tile.LeftGlues.Remove(glue);
                    tile.RightGlues.Remove(glue);
                }
                CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            }

            HideGlueEditorIfEmpty();
        }

        /* Regular methods */

        private void HideGlueEditorIfEmpty()
        {
            if (!(CurrentSingleTileViewModel.CurrentEditorModel is GlueEditorViewModel)) return;
            if (CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Count == 0)
            {
                ConfigureTile();
            }
        }

        private void HideTileEditorIfEmpty()
        {
            if (!(CurrentSingleTileViewModel.CurrentEditorModel is TileTypeVm)) return;
            if (CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.Count == 0)
            {
                CurrentSingleTileViewModel.CurrentEditorModel = null;
            }
        }

        public void SaveChanges()
        {
            _dataService.TileAssemblySystem = new TileAssemblySystem
            {
                Seed = TileTypeVm.ToTileType(CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.Seed),
                Temperature = CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.Temperature,
                TileTypes = new ObservableDictionary<string, TileType>(CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.ToDictionary(t => t.Label, TileTypeVm.ToTileType))
            };
            _dataService.Commit();
        }
    }
}