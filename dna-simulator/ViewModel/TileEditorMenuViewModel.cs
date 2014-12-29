using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace dna_simulator.ViewModel
{
    /// <summary>
    /// Contains properties for a tile editor menu view to bind to.
    /// </summary>
    public class TileEditorMenuViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TileEditorViewModel class.
        /// </summary>
        public TileEditorMenuViewModel()
        {
            OpenColorPickerCommand = new RelayCommand<string>(ExecuteOpenColorPicker, CanOpenColorPicker);
        }

        #endregion

        #region Commands

        public RelayCommand<string> OpenColorPickerCommand { get; private set; }

        public bool CanOpenColorPicker(string message)
        {
            return true;
        }

        public void ExecuteOpenColorPicker(string message)
        {
            Messenger.Default.Send(new NotificationMessage<string>(message, "OpenColorPicker"));
        }

        #endregion
    }
}