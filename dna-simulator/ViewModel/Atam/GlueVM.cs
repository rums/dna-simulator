using System.ComponentModel;
using System.Windows.Media;
using dna_simulator.Model.Atam;

namespace dna_simulator.ViewModel.Atam
{
    public class GlueVm : ViewModelBase
    {
        private readonly Glue _glue;
        private int _color;
        private Color _displayColor;

        private string _label;
        private int _strength;

        public GlueVm(Glue glue)
        {
            _glue = glue;
            DisplayColor = glue.DisplayColor;
            Label = glue.Label;
            Color = glue.Color;
            Strength = glue.Strength;
        }

        public Color DisplayColor
        {
            get { return _displayColor; }
            set
            {
                if (value.Equals(_displayColor)) return;
                _displayColor = value;
                _glue.DisplayColor = value;
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
                _glue.Label = value;
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
                _glue.Color = value;
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
                _glue.Strength = value;
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

        public void GlueOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Label":
                    Label = _glue.Label;
                    break;
                case "DisplayColor":
                    DisplayColor = _glue.DisplayColor;
                    break;
                case "Color":
                    Color = _glue.Color;
                    break;
                case "Strength":
                    Strength = _glue.Strength;
                    break;
            }
        }

        /// <summary>
        ///     Convert a GlueVm to a Glue
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
    }
}