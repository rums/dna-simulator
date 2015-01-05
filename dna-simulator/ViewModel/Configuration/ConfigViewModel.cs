using System.Collections.ObjectModel;
using System.Linq;
using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

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
            CreateTileCommand = new RelayCommand(ExecuteCreateTile, CanCreateTile);
            SaveTileCommand = new RelayCommand(ExecuteSaveTile, CanSaveTile);
            ChangeTileDisplayColorCommand = new RelayCommand(ChangeTileDisplayColor, CanChangeTileDisplayColor);
            DisplayTileTypeCommand = new RelayCommand<object>(ExecuteDisplayTileType, CanDisplayTileType);
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
                foreach (var glue in CurrentSingleTileViewModel.CurrentTileTypeVm.TopEdges.Where(e => e.Label == label))
                {
                    glue.DisplayColor = c;
                }
                foreach (var glue in CurrentSingleTileViewModel.CurrentTileTypeVm.BottomEdges.Where(e => e.Label == label))
                {
                    glue.DisplayColor = c;
                }
                foreach (var glue in CurrentSingleTileViewModel.CurrentTileTypeVm.LeftEdges.Where(e => e.Label == label))
                {
                    glue.DisplayColor = c;
                }
                foreach (var glue in CurrentSingleTileViewModel.CurrentTileTypeVm.RightEdges.Where(e => e.Label == label))
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

        private void ExecuteCreateTile()
        {
            ExecuteSaveTile();
            _dataService.NewDefaultTile((item, error) =>
            {
                CurrentSingleTileViewModel.CurrentTileTypeVm = TileTypeVm.ToTileTypeVm(item, _dataService.TileAssemblySystem);
            });
            CurrentSingleTileViewModel.CurrentEditorModel = CurrentSingleTileViewModel.CurrentTileTypeVm;
            //RaisePropertyChanged("CurrentTileTypeVm");
        }

        public RelayCommand SaveTileCommand { get; private set; }

        private bool CanSaveTile()
        {
            return true;
        }

        private void ExecuteSaveTile()
        {
            _dataService.TileAssemblySystem.TileTypes[CurrentSingleTileViewModel.CurrentTileTypeVm.Id] = TileTypeVm.ToTileTypeBase(CurrentSingleTileViewModel.CurrentTileTypeVm);
            _dataService.Commit();
        }

        public RelayCommand ChangeTileDisplayColorCommand { get; private set; }

        private bool CanChangeTileDisplayColor()
        {
            return true;
        }

        private void ChangeTileDisplayColor()
        {
            _colorPickerService.ShowColorPicker(c =>
            {
                CurrentSingleTileViewModel.CurrentTileTypeVm.DisplayColor = c;
            });
        }

        public RelayCommand<object> DisplayTileTypeCommand { get; private set; }

        public bool CanDisplayTileType(object o)
        {
            return true;
        }

        public void ExecuteDisplayTileType(object o)
        {
            // save current tile
            ExecuteSaveTile();

            // switch tile context
            var tile = o as TileTypeVm;
            CurrentSingleTileViewModel.CurrentTileTypeVm = tile;

            // we should be configuring the new tile
            ConfigureTile();
        }
    }
}