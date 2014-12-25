using dna_simulator.Annotations;
using dna_simulator.Model.aTAM;
using dna_simulator.View;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;

namespace dna_simulator.ViewModel.aTAM
{
    public class SingleTile : INotifyPropertyChanged
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

        private TileType _tile;

        public TileType Tile
        {
            get { return _tile; }
            set
            {
                if (Equals(value, _tile)) return;
                _tile = value;
                OnPropertyChanged("Tile");
            }
        }

        #endregion Properties

        #region Constructors

        public SingleTile()
        {
            OpenColorPickerCommand = new DelegateCommand<bool?>(OpenColorPickerAction, CanOpenColorPicker);
        }

        #endregion Constructors

        #region Commands

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

        public DelegateCommand<bool?> OpenColorPickerCommand { get; private set; }

        private bool CanOpenColorPicker(bool? open)
        {
            return true;
        }

        private void OpenColorPickerAction(bool? open)
        {
            if (open.HasValue)
            {
                ColorPickerIsOpen = open;
            }
        }

        #endregion Commands
    }
}