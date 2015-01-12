using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Media;

namespace dna_simulator.ViewModel
{
    public class ColorPickerViewModel : ViewModelBase
    {
        public ColorPickerViewModel()
        {
            // New default value syntax in C# 6 will be nice :)
            CurrentColor = Colors.White;
        }

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
    }
}