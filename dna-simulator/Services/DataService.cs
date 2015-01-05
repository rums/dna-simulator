using dna_simulator.Model;
using dna_simulator.Model.Atam;
using dna_simulator.Properties;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;

namespace dna_simulator.Services
{
    public class DataService : IDataService
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

        private TileAssemblySystem _mockData;

        public DataService()
        {
            var mockTile = new TileType();
            NewDefaultTile((type, exception) => mockTile = type);
            _mockData = new TileAssemblySystem
            {
                Seed = mockTile,
                Temperature = 0,
                TileTypes = new ObservableDictionary<int, TileType> { { 0, mockTile } }
            };
            TileAssemblySystem = new TileAssemblySystem
            {
                Temperature = _mockData.Temperature,
                TileTypes = new ObservableDictionary<int, TileType>(_mockData.TileTypes),
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
            var tileLabel = RandomString(4);
            var newTile = new TileType
            {
                Id = ++_tileId,
                Label = tileLabel,
                DisplayColor = RandomColor(),
                TopEdges = new ObservableCollection<Glue>
                {
                    new Glue { Label = tileLabel + ".Top", Color = 0, Strength = 0, DisplayColor = RandomColor() },
                    new Glue { Label = tileLabel + ".Top", Color = 0, Strength = 0, DisplayColor = RandomColor() },
                    new Glue { Label = tileLabel + ".Top", Color = 0, Strength = 0, DisplayColor = RandomColor() },
                    new Glue { Label = tileLabel + ".Top", Color = 0, Strength = 0, DisplayColor = RandomColor() },
                    new Glue { Label = tileLabel + ".Top", Color = 0, Strength = 0, DisplayColor = RandomColor() },
                    new Glue { Label = tileLabel + ".Top", Color = 0, Strength = 0, DisplayColor = RandomColor() },
                    new Glue { Label = tileLabel + ".Another glue!", Color = 0, Strength = 0, DisplayColor = RandomColor() }
                },
                BottomEdges = new ObservableCollection<Glue> { new Glue { Label = tileLabel + ".Bottom", Color = 0, Strength = 0, DisplayColor = RandomColor() } },
                LeftEdges = new ObservableCollection<Glue> { new Glue { Label = tileLabel + ".Left", Color = 0, Strength = 0, DisplayColor = RandomColor() } },
                RightEdges = new ObservableCollection<Glue> { new Glue { Label = tileLabel + ".Right", Color = 0, Strength = 0, DisplayColor = RandomColor() } }
            };
            callback(newTile, null);
        }

        public void Commit()
        {
            _mockData = TileAssemblySystem;
            OnPropertyChanged("TileAssemblySystem");
        }

        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        private static string RandomString(int size)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        private static Color RandomColor()
        {
            while (true)
            {
                var colorsType = typeof (Colors);

                var properties = colorsType.GetProperties();

                var random = Random.Next(properties.Length);
                var result = (Color) properties[random].GetValue(null, null);

                if (result == Colors.White || result == Colors.Transparent)
                    continue;
                return result;
            }
        }
    }
}