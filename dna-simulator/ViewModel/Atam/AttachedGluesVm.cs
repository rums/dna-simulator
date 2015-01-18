using System.Collections.Generic;
using dna_simulator.Model;

namespace dna_simulator.ViewModel.Atam
{
    public class AttachedGluesVm : ObservableSet<GlueVm>
    {
        public AttachedGluesVm() { }
        public AttachedGluesVm(IEnumerable<GlueVm> glues) : base(glues) { }
        public TileTypeVm FocusedTile { get; set; }
        public string FocusedEdge { get; set; }
        public GlueVm FocusedGlue { get; set; }
    }
}
