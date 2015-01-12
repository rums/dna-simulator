using dna_simulator.Model;
using dna_simulator.Model.Atam;
using dna_simulator.Properties;
using dna_simulator.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace dna_simulator.Design
{
    public class DesignDataService : IDataService
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        //need to setup a database or use xml files or something
        private int _tileId;

        private int _glueId;

        private TileAssemblySystem _mockData;

        public DesignDataService()
        {
            var mockTile = new TileType();
            NewDefaultTile((type, exception) => mockTile = type);
            _mockData = new TileAssemblySystem
            {
                Seed = mockTile,
                Temperature = 0,
                TileTypes = new ObservableDictionary<string, TileType> { { mockTile.Label, mockTile } }
            };
            TileAssemblySystem = new TileAssemblySystem
            {
                Temperature = _mockData.Temperature,
                TileTypes = new ObservableDictionary<string, TileType>(_mockData.TileTypes),
                Seed = _mockData.Seed
            };
        }

        private TileAssemblySystem _tileAssemblySystem;

        public TileAssemblySystem TileAssemblySystem
        {
            get { return _tileAssemblySystem; }
            set
            {
                if (Equals(value, _tileAssemblySystem)) return;
                _tileAssemblySystem = value;
                OnPropertyChanged();
            }
        }

        public void NewDefaultTile(Action<TileType, Exception> callback)
        {
            var newTile = new TileType
            {
                Label = "Tile " + _tileId,
                DisplayColor = RandomColor(),
                TopEdges = new ObservableDictionary<string, Glue>(),
                BottomEdges = new ObservableDictionary<string, Glue>(),
                LeftEdges = new ObservableDictionary<string, Glue>(),
                RightEdges = new ObservableDictionary<string, Glue>()
            };
            ++_tileId;
            callback(newTile, null);
        }

        public void NewDefaultGlue(Action<Glue, Exception> callback)
        {
            var newGlue = new Glue
            {
                Label = "Label " + _glueId,
                DisplayColor = RandomColor(),
                Color = 0,
                Strength = 0
            };
            ++_glueId;
            callback(newGlue, null);
        }

        public void Commit()
        {
            _mockData = TileAssemblySystem;
            OnPropertyChanged("TileAssemblySystem");
        }

        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        private static Color RandomColor()
        {
            while (true)
            {
                var colorsType = typeof(Colors);

                var properties = colorsType.GetProperties();

                var random = Random.Next(properties.Length);
                var result = (Color)properties[random].GetValue(null, null);

                if (result == Colors.White || result == Colors.Transparent)
                    continue;
                return result;
            }
        }
    }
}