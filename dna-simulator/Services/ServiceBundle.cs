using GalaSoft.MvvmLight.Views;

namespace dna_simulator.Services
{
    public class ServiceBundle : IServiceBundle
    {
        public ServiceBundle()
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