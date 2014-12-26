using dna_simulator.Annotations;
using dna_simulator.Model.Atam;
using System.ComponentModel;
using System.Windows.Media;

namespace dna_simulator.ViewModel.Atam
{
    public class GlueViewModel : INotifyPropertyChanged
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

        private Glue _glue;

        public Glue Glue
        {
            get { return _glue; }
            set
            {
                if (Equals(value, _glue)) return;
                _glue = value;
                OnPropertyChanged("Glue");
            }
        }

        public Brush DisplayColor { get; set; }

        #endregion Properties
    }
}