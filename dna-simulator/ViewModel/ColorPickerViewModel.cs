using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace dna_simulator.ViewModel
{
    public class ColorPickerViewModel : ViewModelBase
    {
        #region Constructors

        public ColorPickerViewModel()
        {
            // New default value syntax in C# 6 will be nice :)
            CurrentColor = Colors.Red;
            ApplyColorCommand = new RelayCommand(ExecuteApplyColor, CanApplyColor);
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
                RaisePropertyChanged("CurrentColor");
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
            Messenger.Default.Send(new NotificationMessage("ApplyColorDone"));
        }

        #endregion Commands
    }
}