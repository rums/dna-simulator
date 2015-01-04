using System;
using System.ComponentModel;
using dna_simulator.Model;
using dna_simulator.Model.Atam;

namespace dna_simulator.Services
{
    public interface IDataService : INotifyPropertyChanged
    {
        TileAssemblySystem TileAssemblySystem { get; set; }

        void NewDefaultTile(Action<TileType, Exception> callback);

        void Commit();
    }
}