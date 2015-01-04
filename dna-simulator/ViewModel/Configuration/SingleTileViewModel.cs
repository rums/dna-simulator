using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;

namespace dna_simulator.ViewModel.Configuration
{
    public class SingleTileViewModel : ViewModelBase
    {
        public IServiceBundle ServiceBundle;
        private IDataService _dataService;
        private IColorPickerService _colorPickerService;

        private TileTypeVm _currentTileTypeVm;
        private ViewModelBase _currentEditorModel;

        public SingleTileViewModel(IServiceBundle serviceBundle)
        {
            ServiceBundle = serviceBundle;
            _dataService = ServiceBundle.DataService;
            _colorPickerService = ServiceBundle.ColorPickerService;

            // initialize properties
            var defaultTileType = _dataService.TileAssemblySystem.TileTypes.Values.First();
            CurrentTileTypeVm = TileTypeVm.ToTileTypeVm(defaultTileType);
            CurrentEditorModel = CurrentTileTypeVm;

            // initialize commands
            CreateTileCommand = new RelayCommand(ExecuteCreateTile, CanCreateTile);
            SaveTileCommand = new RelayCommand(ExecuteSaveTile, CanSaveTile);
            ChangeTileDisplayColorCommand = new RelayCommand(ChangeTileDisplayColor, CanChangeTileDisplayColor);

            // register message listeners
            Messenger.Default.Register<NotificationMessage<string>>(this, message =>
            {
                var edgeName = message.Content;
                switch (message.Notification)
                {
                    case "OpenColorPicker":
                        ChangeTileDisplayColor();
                        break;
                }
            });
            Messenger.Default.Register<NotificationMessage<TileTypeVm>>(this, message =>
            {
                var tile = message.Content;
                switch (message.Notification)
                {
                    case "DisplayTile":
                        ExecuteSaveTile();
                        CurrentEditorModel = tile;
                        CurrentTileTypeVm = tile;
                        break;
                }
            });
        }

        public TileTypeVm CurrentTileTypeVm
        {
            get { return _currentTileTypeVm; }
            set
            {
                if (Equals(value, _currentTileTypeVm)) return;
                _currentTileTypeVm = value;
                RaisePropertyChanged();
            }
        }

        public ViewModelBase CurrentEditorModel
        {
            get { return _currentEditorModel; }
            set
            {
                if (Equals(value, _currentEditorModel)) return;
                _currentEditorModel = value;
                RaisePropertyChanged();
            }
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
                CurrentTileTypeVm = TileTypeVm.ToTileTypeVm(item, _dataService.TileAssemblySystem);
            });
            RaisePropertyChanged("CurrentTileTypeVm");
        }

        public RelayCommand SaveTileCommand { get; private set; }

        private bool CanSaveTile()
        {
            return true;
        }

        private void ExecuteSaveTile()
        {
            _dataService.TileAssemblySystem.TileTypes[CurrentTileTypeVm.Id] = TileTypeVm.ToTileTypeBase(CurrentTileTypeVm);
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
                CurrentTileTypeVm.DisplayColor = c;
            });
        }
    }
}