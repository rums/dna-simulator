using dna_simulator.Services;

namespace dna_simulator.Design
{
    public class DesignServiceBundle : IServiceBundle
    {
        public DesignServiceBundle()
        {
            DataService = new DataService();
            ColorPickerService = new ColorPickerService();
        }

        public IDataService DataService { get; private set; }
        public IColorPickerService ColorPickerService { get; private set; }
    }
}
