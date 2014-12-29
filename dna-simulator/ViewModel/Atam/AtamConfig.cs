using dna_simulator.Model;
using dna_simulator.Model.Atam;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace dna_simulator.ViewModel.Atam
{
    public class AtamConfig : ViewModelBase
    {
        #region Constructors

        public AtamConfig(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetTileAssemblySystem((item, error) =>
            {
                if (error != null)
                {
                    // Report error here
                    return;
                }

                if (item.TileTypes == null) return;

                // Initialize MultiTileViewModel
                MultiTileViewModel.Temperature = item.Temperature;
                MultiTileViewModel.SingleTileViewModels = new ObservableCollection<SingleTileViewModel>();
                foreach (var tvm in item.TileTypes.Select(t => new SingleTileViewModel
                {
                    DisplayColor = t.DisplayColor,
                    Label = t.Label,
                    TopGlue = t.Top,
                    BottomGlue = t.Bottom,
                    LeftGlue = t.Left,
                    RightGlue = t.Right,
                    TopGlueBrush = new SolidColorBrush(t.Top.DisplayColor),
                    BottomGlueBrush = new SolidColorBrush(t.Bottom.DisplayColor),
                    LeftGlueBrush = new SolidColorBrush(t.Left.DisplayColor),
                    RightGlueBrush = new SolidColorBrush(t.Right.DisplayColor),
                    DisplayColorBrush = new SolidColorBrush(t.DisplayColor),
                    IsSeed = (item.Seed.Label == t.Label)
                }))
                {
                    MultiTileViewModel.SingleTileViewModels.Add(tvm);
                }
                // initialize CurrentTileViewModel
                CurrentTileViewModel = MultiTileViewModel.SingleTileViewModels[0];
            });
            CreateTileCommand = new RelayCommand<object>(ExecuteCreateTile, CanCreateTile);
            OpenColorPickerCommand = new RelayCommand<string>(ExecuteOpenColorPicker, CanOpenColorPicker);
            CurrentView = CurrentTileViewModel;
            Messenger.Default.Register<NotificationMessage>(this, message =>
            {
                switch (message.Notification)
                {
                    case "ApplyColorDone":
                        CloseColorPicker();
                        break;
                }
            });
            Messenger.Default.Register<NotificationMessage<Color>>(this, ExecuteSaveColor);
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
        }

        #endregion Constructors

        #region Properties

        private IDataService _dataService;

        private int _currentTileIndex;

        private SingleTileViewModel _currentTileViewModel;

        public SingleTileViewModel CurrentTileViewModel
        {
            get
            {
                if (_currentTileViewModel == null)
                {
                    CurrentTileViewModel = new SingleTileViewModel
                    {
                        Label = "Tile 0",
                        TopGlue = new Glue { Color = 0, Strength = 0 },
                        BottomGlue = new Glue { Color = 0, Strength = 0 },
                        LeftGlue = new Glue { Color = 0, Strength = 0 },
                        RightGlue = new Glue { Color = 0, Strength = 0 },
                        IsSeed = true,
                        DisplayColorBrush = new SolidColorBrush(Colors.Purple),
                        TopGlueBrush = new SolidColorBrush(Colors.Blue),
                        BottomGlueBrush = new SolidColorBrush(Colors.Blue),
                        LeftGlueBrush = new SolidColorBrush(Colors.Blue),
                        RightGlueBrush = new SolidColorBrush(Colors.Blue),
                    };
                }
                return _currentTileViewModel;
            }
            set
            {
                if (Equals(value, _currentTileViewModel)) return;
                _currentTileViewModel = value;
                RaisePropertyChanged("CurrentTileViewModel");
            }
        }

        private MultiTileViewModel _multiTileViewModel;

        public MultiTileViewModel MultiTileViewModel
        {
            get
            {
                if (_multiTileViewModel != null) return _multiTileViewModel;
                MultiTileViewModel = new MultiTileViewModel
                {
                    Temperature = 0,
                };
                return _multiTileViewModel;
            }
            set
            {
                if (Equals(value, _multiTileViewModel)) return;
                _multiTileViewModel = value;
                RaisePropertyChanged("MultiTileViewModel");
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
                RaisePropertyChanged("ColorPickerIsOpen");
            }
        }

        public ColorPickerViewModel ColorPickerViewModel { get; set; }

        private ViewModelBase _currentView;

        public ViewModelBase CurrentView
        {
            get { return _currentView; }
            set
            {
                if (Equals(value, _currentView)) return;
                _currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }

        #endregion Properties

        #region Commands

        public RelayCommand<string> OpenColorPickerCommand { get; private set; }

        private bool CanOpenColorPicker(string targetProperty)
        {
            return true;
        }

        private void ExecuteOpenColorPicker(string targetProperty)
        {
            CurrentView = new ColorPickerViewModel
                {
                    CurrentColor = Colors.Green,
                    BaseViewModel = this,
                    TargetProperty = targetProperty
                };
        }

        public RelayCommand<object> CreateTileCommand { get; private set; }

        private bool CanCreateTile(object o)
        {
            return true;
        }

        private void ExecuteCreateTile(object o)
        {
        }

        #endregion Commands

        #region Messenger methods

        private void CloseColorPicker()
        {
            CurrentView = CurrentTileViewModel;
        }

        private void ExecuteSaveColor(NotificationMessage<Color> message)
        {
            var color = message.Content;
            switch (message.Notification)
            {
                case "TileColor":
                    CurrentTileViewModel.DisplayColor = color;
                    CurrentTileViewModel.DisplayColorBrush = new SolidColorBrush(color);
                    _dataService.SetTileType(new TileType
                    {
                        Top = CurrentTileViewModel.TopGlue,
                        Bottom = CurrentTileViewModel.BottomGlue,
                        Left = CurrentTileViewModel.LeftGlue,
                        Right = CurrentTileViewModel.RightGlue,
                        DisplayColor = color,
                        Label = CurrentTileViewModel.Label
                    }, _currentTileIndex);
                    break;
            }
        }

        #endregion Messenger methods
    }
}