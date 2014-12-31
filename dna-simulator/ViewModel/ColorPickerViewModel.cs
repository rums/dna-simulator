using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Media;

namespace dna_simulator.ViewModel
{
    public class ColorPickerViewModel : ViewModelBase
    {
        #region Constructors

        public ColorPickerViewModel()
        {
            // New default value syntax in C# 6 will be nice :)
            CurrentColor = Colors.White;
            ApplyColorCommand = new RelayCommand(ExecuteApplyColor, CanApplyColor);
            // Register message listeners
            Messenger.Default.Register<NotificationMessage<string>>(this, message =>
            {
                var targetProperty = message.Content;
                switch (message.Notification)
                {
                    case "TargetProperty":
                        TargetProperty = targetProperty;
                        break;
                }
            });
        }

        #endregion Constructors

        #region Properties

        private Color _currentColor;

        public Color CurrentColor
        {
            get { return _currentColor; }
            set
            {
                if (Equals(value, _currentColor)) return;
                _currentColor = value;
                RaisePropertyChanged();
            }
        }

        public string TargetProperty { get; set; }

        #endregion Properties

        #region Commands

        public RelayCommand ApplyColorCommand { get; private set; }

        private bool CanApplyColor()
        {
            return true;
        }

        private void ExecuteApplyColor()
        {
            Messenger.Default.Send(new NotificationMessage<Color>(CurrentColor, TargetProperty));
        }

        #endregion Commands
    }
}