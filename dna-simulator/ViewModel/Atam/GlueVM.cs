using System.Windows.Media;
using dna_simulator.Model.Atam;

namespace dna_simulator.ViewModel.Atam
{
    public class GlueVm : ViewModelBase
    {
        // from model
        private int _id;
        private Color _displayColor;
        private string _label;
        private int _color;
        private int _strength;

        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                RaisePropertyChanged();
            }
        }

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

        public string Label
        {
            get { return _label; }
            set
            {
                if (value == _label) return;
                _label = value;
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


        /// <summary>
        /// Convert a GlueVm to a Glue
        /// </summary>
        /// <param name="glue">GlueVm to be converted to Glue</param>
        /// <returns>Glue</returns>
        public static Glue ToGlue(GlueVm glue)
        {
            return new Glue
            {
                Id = glue.Id,
                DisplayColor = glue.DisplayColor,
                Label = glue.Label,
                Color = glue.Color,
                Strength = glue.Strength
            };
        }

        /// <summary>
        /// Convert a Glue to a GlueVm
        /// </summary>
        /// <param name="glue">Glue to be converted to GlueVm</param>
        /// <returns>GlueVm</returns>
        public static GlueVm ToGlueVm(Glue glue)
        {
            return new GlueVm
            {
                Id = glue.Id,
                DisplayColor = glue.DisplayColor,
                Label = glue.Label,
                Color = glue.Color,
                Strength = glue.Strength
            };
        }
    }
}