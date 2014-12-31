using System;

namespace dna_simulator.Model.Atam
{
    public class TileAssemblySystem : ModelBase
    {
        private int _temperature;
        private ObservableDictionary<string, TileType> _tileTypes;
        private TileType _seed;

        public int Temperature
        {
            get { return _temperature; }
            set
            {
                if (value == _temperature) return;
                if (value < 0) { throw new ArgumentOutOfRangeException("Temperature cannot be less than 0."); }
                _temperature = value;
                OnPropertyChanged();
            }
        }

        public ObservableDictionary<string, TileType> TileTypes
        {
            get { return _tileTypes; }
            set
            {
                if (Equals(value, _tileTypes)) return;
                _tileTypes = value;
                OnPropertyChanged();
            }
        }

        public TileType Seed
        {
            get { return _seed; }
            set
            {
                if (Equals(value, _seed)) return;
                _seed = value;
                OnPropertyChanged();
            }
        }
    }
}