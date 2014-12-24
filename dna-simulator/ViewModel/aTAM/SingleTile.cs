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
    class SingleTile : INotifyPropertyChanged
    {
        #region Private
        private string _label;
        private Glue _topGlue;
        private Glue _bottomGlue;
        private Glue _leftGlue;
        private Glue _rightGlue;
        #endregion

        #region Properties
        public TileType TileType { get; private set; }

        public string Label
        {
            get { return _label; }
            set
            {
                if (value == _label) return;
                _label = value;
                OnPropertyChanged("Label");
            }
        }

        public Glue TopGlue
        {
            get { return _topGlue; }
            set
            {
                if (Equals(value, _topGlue)) return;
                _topGlue = value;
                OnPropertyChanged("TopGlue");
            }
        }

        public Glue BottomGlue
        {
            get { return _bottomGlue; }
            set
            {
                if (Equals(value, _bottomGlue)) return;
                _bottomGlue = value;
                OnPropertyChanged("BottomGlue");
            }
        }

        public Glue LeftGlue
        {
            get { return _leftGlue; }
            set
            {
                if (Equals(value, _leftGlue)) return;
                _leftGlue = value;
                OnPropertyChanged("LeftGlue");
            }
        }

        public Glue RightGlue
        {
            get { return _rightGlue; }
            set
            {
                if (Equals(value, _rightGlue)) return;
                _rightGlue = value;
                OnPropertyChanged("RightGlue");
            }
        }
        #endregion

        #region Constructors
        public SingleTile(TileType tileType)
        {
            TileType = tileType;
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
