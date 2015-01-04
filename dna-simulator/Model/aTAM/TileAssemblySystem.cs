using System;

namespace dna_simulator.Model.Atam
{
    public class TileAssemblySystem : ModelBase
    {
        private int _temperature;
        private TileType _seed;
        private ObservableDictionary<int, TileType> _tileTypes;

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

        public ObservableDictionary<int, TileType> TileTypes
        {
            get { return _tileTypes; }
            set
            {
                if (Equals(value, _tileTypes)) return;
                _tileTypes = value;
                OnPropertyChanged();
            }
        }
    }
}