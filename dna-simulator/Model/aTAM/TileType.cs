using System.Windows.Media;

namespace dna_simulator.Model.Atam
{
    public class TileType : ModelBase
    {
        private Color _displayColor;
        private string _label;
        private ObservableDictionary<string, Glue> _topGlues;
        private ObservableDictionary<string, Glue> _bottomGlues;
        private ObservableDictionary<string, Glue> _leftGlues;
        private ObservableDictionary<string, Glue> _rightGlues;

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

        public ObservableDictionary<string, Glue> TopGlues
        {
            get { return _topGlues; }
            set
            {
                if (Equals(value, _topGlues)) return;
                _topGlues = value;
                OnPropertyChanged();
            }
        }

        public ObservableDictionary<string, Glue> BottomGlues
        {
            get { return _bottomGlues; }
            set
            {
                if (Equals(value, _bottomGlues)) return;
                _bottomGlues = value;
                OnPropertyChanged();
            }
        }

        public ObservableDictionary<string, Glue> LeftGlues
        {
            get { return _leftGlues; }
            set
            {
                if (Equals(value, _leftGlues)) return;
                _leftGlues = value;
                OnPropertyChanged();
            }
        }

        public ObservableDictionary<string, Glue> RightGlues
        {
            get { return _rightGlues; }
            set
            {
                if (Equals(value, _rightGlues)) return;
                _rightGlues = value;
                OnPropertyChanged();
            }
        }
    }
}