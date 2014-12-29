using System.Windows.Media;

namespace dna_simulator.Model.Atam
{
    public class TileType
    {
        public Color DisplayColor;

        public string Label { get; set; }

        public Glue Top { get; set; }

        public Glue Bottom { get; set; }

        public Glue Left { get; set; }

        public Glue Right { get; set; }
    }
}