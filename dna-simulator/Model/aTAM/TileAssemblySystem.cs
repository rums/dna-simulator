using System;
using System.Collections.ObjectModel;

namespace dna_simulator.Model.Atam
{
    public class TileAssemblySystem
    {
        public ObservableCollection<TileType> TileTypes { get; set; }

        public TileType Seed { get; set; }

        private int _temperature;

        public int Temperature
        {
            get { return _temperature; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Temperature cannot be less than 0.");
                }
                _temperature = value;
            }
        }
    }
}