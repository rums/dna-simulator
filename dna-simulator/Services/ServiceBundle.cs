namespace dna_simulator.Services
{
    public class ServiceBundle : IServiceBundle
    {
        public ServiceBundle()
        {
            DataService = new DataService();
            ColorPickerService = new ColorPickerService();
        }

        public IDataService DataService { get; private set; }
        public IColorPickerService ColorPickerService { get; private set; }
    }
}
