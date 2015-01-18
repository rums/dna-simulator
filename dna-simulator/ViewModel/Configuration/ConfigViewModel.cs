using System.Collections;
using System.Collections.Generic;
using System.Linq;
using dna_simulator.Exceptions;
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

            if (CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.Count > 0)
            {
                TileTypeVm currentTile = CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.First();
                CurrentSingleTileViewModel = new SingleTileViewModel(currentTile);
            }
            else
            {
                CurrentSingleTileViewModel = new SingleTileViewModel(null);
            }
            // initialize commands
            ChangeGlueDisplayColorCommand = new RelayCommand<string>(ChangeGlueDisplayColor, CanChangeGlueDisplayColor);
            ConfigureGlueCommand = new RelayCommand<GlueVm>(ConfigureGlue, CanConfigureGlue);
            ConfigureTileCommand = new RelayCommand(ConfigureTile, CanConfigureTile);
            AddTileCommand = new RelayCommand(AddTile, CanAddTile);
            DeleteTilesCommand = new RelayCommand<object>(DeleteTiles, CanDeleteTiles);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            ChangeTileDisplayColorCommand = new RelayCommand<string>(ChangeTileDisplayColor, CanChangeTileDisplayColor);
            DisplayTileTypeCommand = new RelayCommand<object>(DisplayTileType, CanDisplayTileType);
            AddGlueCommand = new RelayCommand(AddGlue, CanAddGlue);
            AddGlueToTileCommand = new RelayCommand<object>(AddGlueToTile, CanAddGlueToTile);
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

        public RelayCommand SaveChangesCommand { get; private set; }

        public RelayCommand<string> ChangeTileDisplayColorCommand { get; private set; }

        public RelayCommand<object> DisplayTileTypeCommand { get; private set; }

        public RelayCommand AddGlueCommand { get; private set; }

        public RelayCommand<object> AddGlueToTileCommand { get; private set; }

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

        private bool CanSaveChanges()
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

        public bool CanAddGlue()
        {
            return true;
        }

        public bool CanAddGlueToTile(object o)
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
            var tile = _dataService.AddTile();

            // display the new tile
            DisplayTileType(CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.First(t => t.Label == tile.Label));
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

        private void ChangeTileDisplayColor(string label)
        {
            _colorPickerService.ShowColorPicker(c =>
            {
                foreach (
                    TileType tile in _dataService.GetTileAssemblySystem().TileTypes.Values.Where(e => e.Label == label))
                {
                    tile.DisplayColor = c;
                }
            });
        }

        public void DisplayTileType(object o)
        {
            // switch tile context
            var tile = o as TileTypeVm;
            CurrentSingleTileViewModel.CurrentTileTypeVm = tile;

            // we should be configuring the new tile
            ConfigureTile();
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

        public void AddGlueToTile(object o)
        {
            try
            {
                var attachedGlues = o as AttachedGluesVm;
                if (attachedGlues == null) return;
                if (attachedGlues.FocusedGlue == null)
                    _dataService.AddGlue(attachedGlues.FocusedTile.Label, attachedGlues.FocusedEdge);
                else
                {
                    _dataService.AddGlue(GlueVm.ToGlue(attachedGlues.FocusedGlue), attachedGlues.FocusedTile.Label,
                        attachedGlues.FocusedEdge);
                }
            }
            catch (InvalidTileTypeException)
            {
            }
        }

        public void RemoveGluesFromTop(object glues)
        {
            // TODO: Modify these removal functions to use the dataservice
            List<GlueVm> toRemove = (glues as IList).Cast<GlueVm>().ToList();
            if (toRemove == null) return;
            _dataService.RemoveGlues(toRemove.Select(GlueVm.ToGlue).ToList());

            HideGlueEditorIfEmpty();
            //List<GlueVm> gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            //foreach (GlueVm glue in gglues)
            //{
            //    CurrentSingleTileViewModel.CurrentTileTypeVm.TopGlues.Remove(glue);
            //    CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            //}

            //HideGlueEditorIfEmpty();
        }

        public void RemoveGluesFromBottom(object glues)
        {
            //List<GlueVm> gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            //foreach (GlueVm glue in gglues)
            //{
            //    CurrentSingleTileViewModel.CurrentTileTypeVm.BottomGlues.Remove(glue);
            //    CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            //}

            //HideGlueEditorIfEmpty();
        }

        public void RemoveGluesFromLeft(object glues)
        {
            //List<GlueVm> gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            //foreach (GlueVm glue in gglues)
            //{
            //    CurrentSingleTileViewModel.CurrentTileTypeVm.LeftGlues.Remove(glue);
            //    CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            //}

            //HideGlueEditorIfEmpty();
        }

        public void RemoveGluesFromRight(object glues)
        {
            //List<GlueVm> gglues = (glues as ObservableCollection<object>).Cast<GlueVm>().ToList();
            //foreach (GlueVm glue in gglues)
            //{
            //    CurrentSingleTileViewModel.CurrentTileTypeVm.RightGlues.Remove(glue);
            //    CurrentSingleTileViewModel.GlueEditorViewModel.Glues.Remove(glue);
            //}

            //HideGlueEditorIfEmpty();
        }

        public void DeleteGlues(object glues)
        {
            List<GlueVm> toRemove = (glues as IList).Cast<GlueVm>().ToList();
            if (toRemove == null) return;
            _dataService.RemoveGlues(toRemove.Select(GlueVm.ToGlue).ToList());

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
                TileTypes =
                    new ObservableDictionary<string, TileType>(
                        CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.ToDictionary(t => t.Label,
                            TileTypeVm.ToTileType))
            };
            _dataService.Commit();
        }
    }
}