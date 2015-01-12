using dna_simulator.Services;

namespace dna_simulator.ViewModel.Simulation
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SimulatorViewModel : ViewModelBase
    {
        private IDataService _dataService;

        /// <summary>
        /// Initializes a new instance of the SimulatorViewModel class.
        /// </summary>
        public SimulatorViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }
    }
}