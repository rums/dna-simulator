using dna_simulator.Model.Atam;
using System;
using System.ComponentModel;

namespace dna_simulator.Services
{
    public interface IDataService : INotifyPropertyChanged
    {
        TileAssemblySystem TileAssemblySystem { get; set; }

        void NewDefaultTile(Action<TileType, Exception> callback);

        void NewDefaultGlue(Action<Glue, Exception> callback);

        void Commit();
    }
}