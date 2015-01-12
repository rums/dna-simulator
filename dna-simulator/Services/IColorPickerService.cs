using System;
using System.Windows.Media;

namespace dna_simulator.Services
{
    public interface IColorPickerService
    {
        void ShowColorPicker(Action<Color> onCloseCallback);
    }
}