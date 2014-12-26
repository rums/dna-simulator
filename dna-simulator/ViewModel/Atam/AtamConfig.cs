using dna_simulator.Annotations;
using dna_simulator.Model.Atam;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace dna_simulator.ViewModel.Atam
{
    public class AtamConfig : INotifyPropertyChanged
    {
        #region Implement INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Implement INotifyPropertyChanged

        #region Properties

        private TasViewModel _tileAssemblySystem;

        public TasViewModel TileAssemblySystem
        {
            get { return _tileAssemblySystem; }
            set
            {
                if (Equals(value, _tileAssemblySystem)) return;
                _tileAssemblySystem = value;
                OnPropertyChanged("TileAssemblySystem");
            }
        }

        private TileTypeViewModel _currentTileType;

        public TileTypeViewModel CurrentTileType
        {
            get { return _currentTileType; }
            set
            {
                if (Equals(value, _currentTileType)) return;
                _currentTileType = value;
                OnPropertyChanged("CurrentTileType");
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
                OnPropertyChanged("ColorPickerIsOpen");
            }
        }

        public ColorPickerViewModel ColorPicker { get; set; }

        #endregion Properties

        #region Constructors

        public AtamConfig()
        {
            CurrentTileType = CurrentTileType ??
                              new TileTypeViewModel
                              {
                                  Label = "Tile 0",
                                  IsSeed = true,
                                  DisplayColor = new SolidColorBrush(Colors.Purple),
                                  Top = new GlueViewModel
                                 {
                                     Glue = new Glue { Color = 0, Strength = 0 },
                                     DisplayColor = new SolidColorBrush(Colors.Red)
                                 },
                                  Bottom = new GlueViewModel
                                  {
                                      Glue = new Glue { Color = 0, Strength = 0 },
                                      DisplayColor = new SolidColorBrush(Colors.Blue)
                                  },
                                  Left = new GlueViewModel
                                  {
                                      Glue = new Glue { Color = 0, Strength = 0 },
                                      DisplayColor = new SolidColorBrush(Colors.Green)
                                  },
                                  Right = new GlueViewModel
                                  {
                                      Glue = new Glue { Color = 0, Strength = 0 },
                                      DisplayColor = new SolidColorBrush(Colors.Cyan)
                                  }
                              };
            TileAssemblySystem = TileAssemblySystem ??
                                 new TasViewModel
                                 {
                                     Temperature = 0,
                                     TileTypeViewModels = new ObservableCollection<TileTypeViewModel>()
                                 };
            TileAssemblySystem.TileTypeViewModels.Add(CurrentTileType);
            CreateTileCommand = new DelegateCommand<object>(ExecuteCreateTile, CanCreateTile);
            OpenColorPickerCommand = new DelegateCommand<string>(ExecuteOpenColorPicker, CanOpenColorPicker);
            ApplyColorCommand = new DelegateCommand<object>(ExecuteApplyColor, CanApplyColor);
        }

        #endregion Constructors

        #region Commands

        public DelegateCommand<string> OpenColorPickerCommand { get; private set; }

        private bool CanOpenColorPicker(string targetProperty)
        {
            return true;
        }

        private void ExecuteOpenColorPicker(string targetProperty)
        {
            ColorPicker = ColorPicker ??
                          new ColorPickerViewModel { CurrentColor = Colors.White, TargetProperty = targetProperty };
            ColorPickerIsOpen = true;
        }

        public DelegateCommand<object> CreateTileCommand { get; private set; }

        private bool CanCreateTile(object o)
        {
            return true;
        }

        private void ExecuteCreateTile(object o)
        {
        }

        public DelegateCommand<object> ApplyColorCommand { get; private set; }

        private bool CanApplyColor(object o)
        {
            return true;
        }

        private void ExecuteApplyColor(object o)
        {
            if (ColorPicker == null) return;
            var propertyInfo = GetProp(GetType(), ColorPicker.TargetProperty);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(propertyInfo, new SolidColorBrush(ColorPicker.CurrentColor), null);
            }
        }

        #endregion Commands

        public PropertyInfo GetProp(Type baseType, string propertyName)
        {
            string[] parts = propertyName.Split('.');

            return (parts.Length > 1)
                ? GetProp(baseType.GetProperty(parts[0]).PropertyType, parts.Skip(1).Aggregate((a, i) => a + "." + i))
                : baseType.GetProperty(propertyName);
        }
    }
}