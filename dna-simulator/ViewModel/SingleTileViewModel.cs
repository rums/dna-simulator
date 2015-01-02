using System.Linq;
using System.Windows.Media;
using dna_simulator.Model;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace dna_simulator.ViewModel
{
    public class SingleTileViewModel : ViewModelBase
    {
        private IDataService _dataService;

        #region Constructors

        public SingleTileViewModel(IDataService dataService)
        {
            _dataService = dataService;
            // initialize TileTypeVm
            var defaultTileType = _dataService.TileAssemblySystem.TileTypes.Values.First();
            CurrentTileTypeVm = new TileTypeVm
            {
                DisplayColor = defaultTileType.DisplayColor,
                Label = defaultTileType.Label,
                Top = new GlueVm {
                    Color = defaultTileType.Top.Color,
                    Strength = defaultTileType.Top.Strength,
                    DisplayColor = defaultTileType.Top.DisplayColor
                },
                Bottom = new GlueVm
                {
                    Color = defaultTileType.Bottom.Color,
                    Strength = defaultTileType.Bottom.Strength,
                    DisplayColor = defaultTileType.Bottom.DisplayColor
                },
                Left = new GlueVm
                {
                    Color = defaultTileType.Left.Color,
                    Strength = defaultTileType.Left.Strength,
                    DisplayColor = defaultTileType.Left.DisplayColor
                },
                Right = new GlueVm
                {
                    Color = defaultTileType.Right.Color,
                    Strength = defaultTileType.Right.Strength,
                    DisplayColor = defaultTileType.Right.DisplayColor
                },
            };
            // initialize commands
            CreateTileCommand = new RelayCommand(ExecuteCreateTile, CanCreateTile);
            SaveTileCommand = new RelayCommand(ExecuteSaveTile, CanSaveTile);
            OpenColorPickerCommand = new RelayCommand<string>(ExecuteOpenColorPicker, CanOpenColorPicker);

            // register message listeners
            Messenger.Default.Register<NotificationMessage<string>>(this, message =>
            {
                var edgeName = message.Content;
                switch (message.Notification)
                {
                    case "OpenColorPicker":
                        ExecuteOpenColorPicker(edgeName);
                        break;
                }
            });
            Messenger.Default.Register<NotificationMessage<Color>>(this, ExecuteSaveColor);
            Messenger.Default.Register<NotificationMessage<TileTypeVm>>(this, message =>
            {
                var tile = message.Content;
                switch (message.Notification)
                {
                    case "DisplayTile":
                        CurrentTileTypeVm = tile;
                        break;
                }
            });
        }

        #endregion

        #region Properties

        private TileTypeVm _currentTileTypeVm;

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

        private bool? _colorPickerIsOpen;

        public bool? ColorPickerIsOpen
        {
            get { return _colorPickerIsOpen; }
            set
            {
                if (value.Equals(_colorPickerIsOpen)) return;
                _colorPickerIsOpen = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Commands

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
            _dataService.Commit();
        }

        public RelayCommand<string> OpenColorPickerCommand { get; private set; }

        private bool CanOpenColorPicker(string targetProperty)
        {
            return true;
        }

        private void ExecuteOpenColorPicker(string targetProperty)
        {
            ColorPickerIsOpen = true;
            Messenger.Default.Send(new NotificationMessage<string>(targetProperty, "TargetProperty"));
        }

        #endregion

        #region Messenger methods

        private void ExecuteSaveColor(NotificationMessage<Color> message)
        {
            var color = message.Content;
            switch (message.Notification)
            {
                case "TileColor":
                    CurrentTileTypeVm.DisplayColor = color;
                    break;
                case "Top":
                    CurrentTileTypeVm.Top.DisplayColor = color;
                    break;
                case "Bottom":
                    CurrentTileTypeVm.Bottom.DisplayColor = color;
                    break;
                case "Left":
                    CurrentTileTypeVm.Left.DisplayColor = color;
                    break;
                case "Right":
                    CurrentTileTypeVm.Right.DisplayColor = color;
                    break;
            }
            _dataService.TileAssemblySystem.TileTypes[CurrentTileTypeVm.Label] = TileTypeVm.ToTileType(CurrentTileTypeVm);
        }

        #endregion Messenger methods
    }
}