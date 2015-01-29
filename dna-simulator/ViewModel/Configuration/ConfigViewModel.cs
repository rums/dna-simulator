using System.Linq;
using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Messaging;

namespace dna_simulator.ViewModel.Configuration
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    /// </summary>
    public class ConfigViewModel : ViewModelBase
    {
        private MultiTileViewModel _currentMultiTileViewModel;
        private ViewModelBase _focusedViewModel;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ConfigViewModel(IServiceBundle serviceBundle)
        {
            CurrentMultiTileViewModel = (new ViewModelLocator()).MultiTileViewModel;

            if (CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.Count > 0)
            {
                var singleTileViewModel = (new ViewModelLocator()).SingleTileViewModel;
                FocusedViewModel = singleTileViewModel;
                singleTileViewModel.DisplayTileType(CurrentMultiTileViewModel.CurrentTileAssemblySystemVm.TileTypes.FirstOrDefault());
            }
            else
            {
                FocusedViewModel = (new ViewModelLocator()).Introduction;
            }

            Messenger.Default.Register<NotificationMessage<TileTypeVm>>(this, message =>
            {
                FocusedViewModel = (new ViewModelLocator()).SingleTileViewModel;
            });
        }

        public ViewModelBase FocusedViewModel
        {
            get { return _focusedViewModel; }
            set
            {
                if (Equals(value, _focusedViewModel)) return;
                _focusedViewModel = value;
                RaisePropertyChanged();
            }
        }

        public MultiTileViewModel CurrentMultiTileViewModel
        {
            get { return _currentMultiTileViewModel; }
            set
            {
                if (Equals(value, _currentMultiTileViewModel)) return;
                _currentMultiTileViewModel = value;
                RaisePropertyChanged();
            }
        }
    }
}