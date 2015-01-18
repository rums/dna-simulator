using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace dna_simulator.Model.Atam
{
    public class GlueLabel : ModelBase
    {
        public GlueLabel(string label)
        {
            Label = label;
        }

        private string _label;

        public string Label
        {
            get { return _label; }
            set
            {
                if (Equals(value, _label)) return;
                _label = value;
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

            var glueLabel = obj as GlueLabel;
            return glueLabel != null && Label.Equals(glueLabel.Label);
        }

// override object.GetHashCode
        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }
    }
}
