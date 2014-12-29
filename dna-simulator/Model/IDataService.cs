using dna_simulator.Model.Atam;
using System;

namespace dna_simulator.Model
{
    public interface IDataService
    {
        void GetTileAssemblySystem(Action<TileAssemblySystem, Exception> callback);

        void SetTileAssemblySystem(TileAssemblySystem tileAssemblySystem);

        void SetTileType(TileType tile, int tileIndex);
    }
}