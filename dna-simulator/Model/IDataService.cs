using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using dna_simulator.Model.Atam;

namespace dna_simulator.Model
{
    public interface IDataService : INotifyPropertyChanged
    {
        TileAssemblySystem TileAssemblySystem { get; set; }

        void NewDefaultTile(Action<TileType, Exception> callback);

        void Commit();
    }
}