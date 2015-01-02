using System.Windows.Media;

namespace dna_simulator.ViewModel.Atam
{
    public class GlueVm : ViewModelBase
    {
        // from model
        private Color _displayColor;
        private int _color;
        private int _strength;

        // viewmodel specific
        private string _name;

        public Color DisplayColor
        {
            get { return _displayColor; }
            set
            {
                if (value.Equals(_displayColor)) return;
                _displayColor = value;
                RaisePropertyChanged();
            }
        }

        public int Color
        {
            get { return _color; }
            set
            {
                if (value == _color) return;
                _color = value;
                RaisePropertyChanged();
            }
        }

        public int Strength
        {
            get { return _strength; }
            set
            {
                if (value == _strength) return;
                _strength = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                RaisePropertyChanged();
            }
        }
    }
}
