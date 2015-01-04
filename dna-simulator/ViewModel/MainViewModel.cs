using dna_simulator.Services;
using dna_simulator.ViewModel.Configuration;

namespace dna_simulator.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IServiceBundle _serviceBundle;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IServiceBundle serviceBundle)
        {
            _serviceBundle = serviceBundle;
            CurrentViewModel = new ConfigViewModel(_serviceBundle);
        }

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (Equals(value, _currentViewModel)) return;
                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }
    }
}