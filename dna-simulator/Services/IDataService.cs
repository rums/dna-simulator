using System.Collections.Generic;
using dna_simulator.Model;
using dna_simulator.Model.Atam;
using System;
using System.ComponentModel;

namespace dna_simulator.Services
{
    public interface IDataService : INotifyPropertyChanged
    {
        // TODO: Properties should only be used for subscribing to property changed events. How to reflect this?
        TileAssemblySystem TileAssemblySystem { get; set; }

        ObservableDictionary<GlueLabel, Glue> Glues { get; set; } 

        TileAssemblySystem GetTileAssemblySystem();

        void SetTemperature(int temperature);

        void SetSeed(TileType tile);

        TileType AddTile();

        TileType AddTile(TileType tile);

        void RemoveTiles(List<TileType> tiles);

        Glue AddGlue();

        Glue AddGlue(Glue glue);

        Glue AddGlue(string tileLabel, string edge);

        Glue AddGlue(Glue glue, string tileLabel, string edge);

        void RemoveGlues(List<Glue> glues);

        void RemoveGlues(List<Glue> glues, string tileLabel, string edge);

        void Commit();
    }
}