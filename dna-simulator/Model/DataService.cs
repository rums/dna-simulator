using dna_simulator.Model.Atam;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace dna_simulator.Model
{
    public class DataService : IDataService
    {
        //need to setup a database or use xml files or something
        private TileAssemblySystem _tileAssemblySystem;

        public void GetTileAssemblySystem(Action<TileAssemblySystem, Exception> callback)
        {
            // Use this to connect to the actual data service
            var tile = new TileType
            {
                Label = "Tile 0",
                Top = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Red },
                Bottom = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Blue },
                Left = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Green },
                Right = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Cyan },
                DisplayColor = Colors.Purple,
            };
            _tileAssemblySystem = new TileAssemblySystem
            {
                Seed = tile,
                Temperature = 0,
                TileTypes = new ObservableCollection<TileType> { tile }
            };
            callback(_tileAssemblySystem, null);
        }

        public void SetTileAssemblySystem(TileAssemblySystem tileAssemblySystem)
        {
            _tileAssemblySystem = tileAssemblySystem;
        }

        public void SetTileType(TileType tile, int tileIndex)
        {
            _tileAssemblySystem.TileTypes[tileIndex] = tile;
        }
    }
}