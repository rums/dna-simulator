using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace dna_simulator.Model.aTAM
{
    public class TileAssemblySystem
    {
        public List<TileType> TileTypes { get; set; }
        public Dictionary<Point, TileType> Seed { get; set; }
        private int _temperature;

        public int Temperature
        {
            get { return _temperature; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Temperature cannot be less than 0.");
                }
                _temperature = value;
            }
        }
    }
}
