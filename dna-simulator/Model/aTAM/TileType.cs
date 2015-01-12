using System.Windows.Media;

namespace dna_simulator.Model.Atam
{
    public class TileType : ModelBase
    {
        private Color _displayColor;
        private string _label;
        private ObservableDictionary<string, Glue> _topEdges;
        private ObservableDictionary<string, Glue> _bottomEdges;
        private ObservableDictionary<string, Glue> _leftEdges;
        private ObservableDictionary<string, Glue> _rightEdges;

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

        public ObservableDictionary<string, Glue> TopEdges
        {
            get { return _topEdges; }
            set
            {
                if (Equals(value, _topEdges)) return;
                _topEdges = value;
                OnPropertyChanged();
            }
        }

        public ObservableDictionary<string, Glue> BottomEdges
        {
            get { return _bottomEdges; }
            set
            {
                if (Equals(value, _bottomEdges)) return;
                _bottomEdges = value;
                OnPropertyChanged();
            }
        }

        public ObservableDictionary<string, Glue> LeftEdges
        {
            get { return _leftEdges; }
            set
            {
                if (Equals(value, _leftEdges)) return;
                _leftEdges = value;
                OnPropertyChanged();
            }
        }

        public ObservableDictionary<string, Glue> RightEdges
        {
            get { return _rightEdges; }
            set
            {
                if (Equals(value, _rightEdges)) return;
                _rightEdges = value;
                OnPropertyChanged();
            }
        }
    }
}