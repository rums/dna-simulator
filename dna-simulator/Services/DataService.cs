using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;
using dna_simulator.Model;
using dna_simulator.Model.Atam;
using dna_simulator.Properties;

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
            var mockTile = new TileType
            {
                Id = 0,
                Label = "Tile 0",
                DisplayColor = Colors.Magenta,
                TopEdges = new ObservableCollection<Glue> { new Glue { Label = "Top", Color = 0, Strength = 0, DisplayColor = Colors.Red }, new Glue { Label = "More glue!", Color = 0, Strength = 0, DisplayColor = Colors.Brown } },
                BottomEdges = new ObservableCollection<Glue> { new Glue {Label = "Bottom", Color = 0, Strength = 0, DisplayColor = Colors.Blue}},
                LeftEdges = new ObservableCollection<Glue> { new Glue {Label = "Left", Color = 0, Strength = 0, DisplayColor = Colors.Green}},
                RightEdges = new ObservableCollection<Glue> { new Glue {Label = "Right", Color = 0, Strength = 0, DisplayColor = Colors.Cyan}}
            };
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
            var tileLabel = RandomTileLabel(4);
            var newTile = new TileType
            {
                Id = ++_tileId,
                Label = tileLabel,
                DisplayColor = Colors.Magenta,
                TopEdges = new ObservableCollection<Glue> { new Glue { Label = "Top", Color = 0, Strength = 0, DisplayColor = Colors.Red } },
                BottomEdges = new ObservableCollection<Glue> { new Glue { Label = "Bottom", Color = 0, Strength = 0, DisplayColor = Colors.Blue } },
                LeftEdges = new ObservableCollection<Glue> { new Glue { Label = "Left", Color = 0, Strength = 0, DisplayColor = Colors.Green } },
                RightEdges = new ObservableCollection<Glue> { new Glue { Label = "Right", Color = 0, Strength = 0, DisplayColor = Colors.Cyan } }
            };
            TileAssemblySystem.TileTypes.Add(_tileId, newTile);
            callback(newTile, null);
        }

        public void Commit()
        {
            _mockData = TileAssemblySystem;
            OnPropertyChanged("TileAssemblySystem");
        }

        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        private string RandomTileLabel(int size)
        {
            var builder = new StringBuilder();
            builder.Append("Label ");
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}