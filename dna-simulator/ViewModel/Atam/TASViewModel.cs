using dna_simulator.Annotations;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace dna_simulator.ViewModel.Atam
{
    public class TasViewModel : INotifyPropertyChanged
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

        private ObservableCollection<TileTypeViewModel> _tileTypeViewModels;

        public ObservableCollection<TileTypeViewModel> TileTypeViewModels
        {
            get { return _tileTypeViewModels; }
            set
            {
                if (Equals(value, _tileTypeViewModels)) return;
                _tileTypeViewModels = value;
                OnPropertyChanged("TileTypeViewModels");
            }
        }

        private int _temperature;

        public int Temperature
        {
            get { return _temperature; }
            set
            {
                if (value == _temperature) return;
                _temperature = value;
                OnPropertyChanged("Temperature");
            }
        }

        #endregion Properties
    }
}