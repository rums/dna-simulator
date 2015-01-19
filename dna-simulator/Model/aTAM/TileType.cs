using System.Windows.Media;

namespace dna_simulator.Model.Atam
{
    public class TileType : ModelBase
    {
        private Color _displayColor;
        private string _label;
        private ObservableSet<GlueLabel> _topGlues;
        private ObservableSet<GlueLabel> _bottomGlues;
        private ObservableSet<GlueLabel> _leftGlues;
        private ObservableSet<GlueLabel> _rightGlues;

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

        public ObservableSet<GlueLabel> TopGlues
        {
            get { return _topGlues; }
            set
            {
                if (Equals(value, _topGlues)) return;
                _topGlues = value;
                OnPropertyChanged();
            }
        }

        public ObservableSet<GlueLabel> BottomGlues
        {
            get { return _bottomGlues; }
            set
            {
                if (Equals(value, _bottomGlues)) return;
                _bottomGlues = value;
                OnPropertyChanged();
            }
        }

        public ObservableSet<GlueLabel> LeftGlues
        {
            get { return _leftGlues; }
            set
            {
                if (Equals(value, _leftGlues)) return;
                _leftGlues = value;
                OnPropertyChanged();
            }
        }

        public ObservableSet<GlueLabel> RightGlues
        {
            get { return _rightGlues; }
            set
            {
                if (Equals(value, _rightGlues)) return;
                _rightGlues = value;
                OnPropertyChanged();
            }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var tile = obj as Glue;

            return tile != null && Label.Equals(tile.Label);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }
    }
}