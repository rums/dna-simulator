using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using dna_simulator.Annotations;
using dna_simulator.Model.aTAM;
using Microsoft.Practices.Prism.Commands;

namespace dna_simulator.ViewModel.aTAM
{
    public class SingleTile : INotifyPropertyChanged
    {
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
        #endregion

        #region Constructors
        public SingleTile()
        {
            ChangeColor = new DelegateCommand<object>(ChangeColorAction, CanChangeColor);
            ChangeStrength = new DelegateCommand<object>(ChangeStrengthAction, CanChangeStrength);
        }
        #endregion

        #region Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        public DelegateCommand<object> ChangeColor { get; private set; }
        public DelegateCommand<object> ChangeStrength { get; private set; }

        private bool CanChangeColor(object parameter)
        {
            return true;
        }

        private bool CanChangeStrength(object parameter)
        {
            return true;
        }

        private void ChangeColorAction(object parameter)
        {

        }

        private void ChangeStrengthAction(object parameter)
        {
            
        }
        #endregion
    }
}
