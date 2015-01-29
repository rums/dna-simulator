using System.Windows.Media;
using dna_simulator.Model;
using dna_simulator.Model.Atam;
using System.Collections.Generic;
using System.ComponentModel;

namespace dna_simulator.Services
{
    public interface IDataService : INotifyPropertyChanged
    {
        TileAssemblySystem TileAssemblySystem { get; }

        ObservableDictionary<GlueLabel, Glue> Glues { get; }

        void SetTileAssemblySystem(TileAssemblySystem tileAssemblySystem);

        void SetTemperature(int temperature);

        void SetSeed(TileType tile);

        TileType AddTile();

        TileType AddTile(TileType tile);

        void SetTileLabel(TileType tile, string label);

        void SetTileDisplayColor(TileType tile, Color displayColor);

        void RemoveTiles(List<TileType> tiles);

        Glue AddGlue();
        
        Glue AddGlue(Glue glue);

        Glue AddGlue(string tileLabel, string edge);

        Glue AddGlue(Glue glue, string tileLabel, string edge);

        void RemoveGlues(List<Glue> glues);

        void RemoveGlues(List<Glue> glues, string tileLabel, string edge);

        void SetGlueLabel(Glue glue, string label);

        void SetGlueDisplayColor(Glue glue, Color displayColor);

        void SetGlueColor(Glue glue, int color);

        void SetGlueStrength(Glue glue, int strength);

        void Commit();
    }
}