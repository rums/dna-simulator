using dna_simulator.View.Controls;
using System;
using System.Windows.Media;

namespace dna_simulator.Services
{
    public class ColorPickerService : IColorPickerService
    {
        public void ShowColorPicker(Action<Color> onCloseCallback)
        {
            var dialog = new ColorPicker { CloseAction = onCloseCallback };
            dialog.Closed += OnClosed;
            dialog.Show();
        }

        private static void OnClosed(object sender, EventArgs e)
        {
            var dialog = sender as ColorPicker;
            if (dialog == null) return;
            dialog.Closed -= OnClosed;
            dialog.CloseAction(dialog.ColorPickr.Color);
        }
    }
}