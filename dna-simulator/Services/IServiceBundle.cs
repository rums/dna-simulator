namespace dna_simulator.Services
{
    public interface IServiceBundle
    {
        IDataService DataService { get; }

        IColorPickerService ColorPickerService { get; }
    }
}