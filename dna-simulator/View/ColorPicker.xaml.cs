using System.Windows;
using System.Windows.Controls;

namespace dna_simulator.View
{
    public partial class ColorPicker : ChildWindow
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}