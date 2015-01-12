using dna_simulator.Services;
using GalaSoft.MvvmLight.Views;

namespace dna_simulator.Design
{
    public class DesignServiceBundle : IServiceBundle
    {
        public DesignServiceBundle()
        {
            DataService = new DataService();
            ColorPickerService = new ColorPickerService();
            DialogService = new DialogService();
        }

        public IDataService DataService { get; private set; }

        public IColorPickerService ColorPickerService { get; private set; }

        public IDialogService DialogService { get; private set; }
    }
}