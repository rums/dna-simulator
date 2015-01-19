using dna_simulator.Model;
using System.Collections.Generic;

namespace dna_simulator.ViewModel.Atam
{
    public class AttachedGluesVm : ObservableSet<GlueVm>
    {
        public AttachedGluesVm()
        {
        }

        public AttachedGluesVm(IEnumerable<GlueVm> glues)
            : base(glues)
        {
        }

        public TileTypeVm FocusedTile { get; set; }

        public string FocusedEdge { get; set; }

        public GlueVm FocusedGlue { get; set; }
    }
}