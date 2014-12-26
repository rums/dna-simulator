using dna_simulator.Annotations;
using System.ComponentModel;
using System.Windows.Media;

namespace dna_simulator.ViewModel.Atam
{
    public class TileTypeViewModel : INotifyPropertyChanged
    {
        #region Implement INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Implement INotifyPropertyChanged

        #region Properties

        private GlueViewModel _top;

        public GlueViewModel Top
        {
            get { return _top; }
            set
            {
                if (Equals(value, _top)) return;
                _top = value;
                OnPropertyChanged("Top");
            }
        }

        private GlueViewModel _bottom;

        public GlueViewModel Bottom
        {
            get { return _bottom; }
            set
            {
                if (Equals(value, _bottom)) return;
                _bottom = value;
                OnPropertyChanged("Bottom");
            }
        }

        private GlueViewModel _left;

        public GlueViewModel Left
        {
            get { return _left; }
            set
            {
                if (Equals(value, _left)) return;
                _left = value;
                OnPropertyChanged("Left");
            }
        }

        private GlueViewModel _right;

        public GlueViewModel Right
        {
            get { return _right; }
            set
            {
                if (Equals(value, _right)) return;
                _right = value;
                OnPropertyChanged("Right");
            }
        }

        private string _label;

        public string Label
        {
            get { return _label; }
            set
            {
                if (value == _label) return;
                _label = value;
                OnPropertyChanged("Label");
            }
        }

        private bool _isSeed;

        public bool IsSeed
        {
            get { return _isSeed; }
            set
            {
                if (value.Equals(_isSeed)) return;
                _isSeed = value;
                OnPropertyChanged("IsSeed");
            }
        }

        public SolidColorBrush DisplayColor { get; set; }

        #endregion Properties
    }
}