using dna_simulator.Model.Atam;
using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace dna_simulator.ViewModel.Atam
{
    public class GlueViewModel : ViewModelBase
    {
        #region Properties

        private Glue _glue;

        public Glue Glue
        {
            get { return _glue; }
            set
            {
                if (Equals(value, _glue)) return;
                _glue = value;
                RaisePropertyChanged("Glue");
            }
        }

        public Brush DisplayColor { get; set; }

        #endregion Properties
    }
}