using dna_simulator.Annotations;
using dna_simulator.Model.aTAM;
using System.ComponentModel;

namespace dna_simulator.ViewModel.aTAM
{
    public class MultiTile : INotifyPropertyChanged
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

        private TileAssemblySystem _tileAssemblySystem;

        public TileAssemblySystem TileAssemblySystem
        {
            get { return _tileAssemblySystem; }
            set
            {
                if (Equals(value, _tileAssemblySystem)) return;
                _tileAssemblySystem = value;
                OnPropertyChanged("TileAssemblySystem");
            }
        }

        #endregion Properties
    }
}