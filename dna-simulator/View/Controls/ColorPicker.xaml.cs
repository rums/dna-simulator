using System;
using System.Windows.Media;

namespace dna_simulator.View.Controls
{
    public partial class ColorPicker
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        public Action<Color> CloseAction { get; set; }
    }
}