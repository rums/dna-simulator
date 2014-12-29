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

        /// <summary>
        /// This command asks for a color picker. It takes a string which identifies the applicable property (e.g., TileColor).
        /// Note: This mechanism seems dirty and could probably be improved; a simple typo in
        /// the string being passed may be a difficult bug.
        /// </summary>
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