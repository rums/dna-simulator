using dna_simulator.Model;

namespace dna_simulator.ViewModel.Atam
{
    public class AtamConfig : ViewModelBase
    {
        #region Constructors

        public AtamConfig(IDataService dataService)
        {
            _dataService = dataService;
            // initialize MultiTileViewModel
            MultiTileViewModel = new MultiTileViewModel(_dataService);
            // initialize SingleTileViewModel
            SingleTileViewModel = new SingleTileViewModel(_dataService);

            // initialize commands
            CurrentView = SingleTileViewModel;
        }

        #endregion Constructors

        #region Properties

        private IDataService _dataService;

        private SingleTileViewModel _singleTileViewModel;

        public SingleTileViewModel SingleTileViewModel
        {
            get { return _singleTileViewModel; }
            set
            {
                if (Equals(value, _singleTileViewModel)) return;
                _singleTileViewModel = value;
                RaisePropertyChanged();
            }
        }

        private MultiTileViewModel _multiTileViewModel;

        public MultiTileViewModel MultiTileViewModel
        {
            get { return _multiTileViewModel; }
            set
            {
                if (Equals(value, _multiTileViewModel)) return;
                _multiTileViewModel = value;
                RaisePropertyChanged();
            }
        }

        private ViewModelBase _currentView;

        public ViewModelBase CurrentView
        {
            get { return _currentView; }
            set
            {
                if (Equals(value, _currentView)) return;
                _currentView = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Commands


        #endregion Commands
    }
}