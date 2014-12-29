using dna_simulator.Model;
using dna_simulator.Model.Atam;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace dna_simulator.Design
{
    public class DesignDataService : IDataService
    {
        public void GetTileAssemblySystem(Action<TileAssemblySystem, Exception> callback)
        {
            // Use this to create design time data

            var currentTileType = new TileType
                              {
                                  Label = "Tile 0",
                                  Top = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Red },
                                  Bottom = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Blue },
                                  Left = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Green },
                                  Right = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Cyan },
                                  DisplayColor = Colors.Purple
                              };
            var tileAssemblySystem = new TileAssemblySystem
            {
                Seed = currentTileType,
                Temperature = 0,
                TileTypes = new ObservableCollection<TileType>()
            };
            tileAssemblySystem.TileTypes.Add(currentTileType);
            callback(tileAssemblySystem, null);
        }

        public void SetTileAssemblySystem(TileAssemblySystem tileAssemblySystem)
        {
        }

        public void SetTileType(TileType tile, int tileIndex)
        {
        }
    }
}