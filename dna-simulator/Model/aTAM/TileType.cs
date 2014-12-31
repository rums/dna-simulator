using System.Windows.Media;

namespace dna_simulator.Model.Atam
{
    public class TileType : ModelBase
    {
        private Color _displayColor;
        private string _label;
        private Glue _top;
        private Glue _bottom;
        private Glue _left;
        private Glue _right;

        public Color DisplayColor
        {
            get { return _displayColor; }
            set
            {
                if (value.Equals(_displayColor)) return;
                _displayColor = value;
                OnPropertyChanged();
            }
        }

        public string Label
        {
            get { return _label; }
            set
            {
                if (value == _label) return;
                _label = value;
                OnPropertyChanged();
            }
        }

        public Glue Top
        {
            get { return _top; }
            set
            {
                if (Equals(value, _top)) return;
                _top = value;
                OnPropertyChanged();
            }
        }

        public Glue Bottom
        {
            get { return _bottom; }
            set
            {
                if (Equals(value, _bottom)) return;
                _bottom = value;
                OnPropertyChanged();
            }
        }

        public Glue Left
        {
            get { return _left; }
            set
            {
                if (Equals(value, _left)) return;
                _left = value;
                OnPropertyChanged();
            }
        }

        public Glue Right
        {
            get { return _right; }
            set
            {
                if (Equals(value, _right)) return;
                _right = value;
                OnPropertyChanged();
            }
        }
    }
}