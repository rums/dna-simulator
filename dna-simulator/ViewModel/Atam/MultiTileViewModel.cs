using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace dna_simulator.ViewModel.Atam
{
    public class MultiTileViewModel : ViewModelBase
    {
        #region Properties

        private ObservableCollection<SingleTileViewModel> _singleTileViewModels;

        public ObservableCollection<SingleTileViewModel> SingleTileViewModels
        {
            get { return _singleTileViewModels; }
            set
            {
                if (Equals(value, _singleTileViewModels)) return;
                _singleTileViewModels = value;
                RaisePropertyChanged();
            }
        }

        private int _temperature;

        public int Temperature
        {
            get { return _temperature; }
            set
            {
                if (Equals(value, _temperature)) return;
                _temperature = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}