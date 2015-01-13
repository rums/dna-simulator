using System;
using dna_simulator.Model.Atam;
using System.Windows.Media;

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

            var glue = obj as GlueVm;

            return glue != null && Label.Equals(glue.Label);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Label.GetHashCode();
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
                DisplayColor = glue.DisplayColor,
                Label = glue.Label,
                Color = glue.Color,
                Strength = glue.Strength
            };
        }
    }
}