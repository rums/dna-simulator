using System;

namespace dna_simulator.Exceptions
{
    public class InvalidTileTypeException : Exception
    {
        public InvalidTileTypeException(string error)
            : base(error)
        {
        }
    }
}