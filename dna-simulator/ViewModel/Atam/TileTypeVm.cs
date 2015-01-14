using System.Collections.ObjectModel;
using dna_simulator.Model;
using dna_simulator.Model.Atam;
using System.Linq;
using System.Windows.Media;

namespace dna_simulator.ViewModel.Atam
{
    public class TileTypeVm : ViewModelBase
    {
        // from model
        private int _id;

        private string _label;
        private Color _displayColor;
        private GlueVms _topGlues;
        private GlueVms _bottomGlues;
        private GlueVms _leftGlues;
        private GlueVms _rightGlues;

        // viewmodel specific
        private bool _isSeed;

        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                RaisePropertyChanged();
            }
        }

        public string Label
        {
            get { return _label; }
            set
            {
                if (value == _label) return;
                _label = value;
                RaisePropertyChanged();
            }
        }

        public Color DisplayColor
        {
            get { return _displayColor; }
            set
            {
                if (value.Equals(_displayColor)) return;
                _displayColor = value;
                RaisePropertyChanged();
            }
        }

        public GlueVms TopGlues
        {
            get { return _topGlues; }
            set
            {
                if (Equals(value, _topGlues)) return;
                _topGlues = value;
                RaisePropertyChanged();
            }
        }

        public GlueVms BottomGlues
        {
            get { return _bottomGlues; }
            set
            {
                if (Equals(value, _bottomGlues)) return;
                _bottomGlues = value;
                RaisePropertyChanged();
            }
        }

        public GlueVms LeftGlues
        {
            get { return _leftGlues; }
            set
            {
                if (Equals(value, _leftGlues)) return;
                _leftGlues = value;
                RaisePropertyChanged();
            }
        }

        public GlueVms RightGlues
        {
            get { return _rightGlues; }
            set
            {
                if (Equals(value, _rightGlues)) return;
                _rightGlues = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSeed
        {
            get { return _isSeed; }
            set
            {
                if (value.Equals(_isSeed)) return;
                _isSeed = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Convert a TileType to a TileTypeVm
        /// </summary>
        /// <param name="tile">TileType to be converted to TileTypeVm</param>
        /// <param name="tileAssemblySystem">TileAssemblySystem in which this TileType resides</param>
        /// <returns></returns>
        public static TileTypeVm ToTileTypeVm(TileType tile, TileAssemblySystem tileAssemblySystem)
        {
            var tilevm = ToTileTypeVm(tile);
            tilevm.IsSeed = Equals(tile.Label, tileAssemblySystem.Seed.Label);
            return tilevm;
        }

        /// <summary>
        /// Convert a TileType to a TileTypeVm
        /// </summary>
        /// <param name="tile">TileType to be converted to TileTypeVm</param>
        /// <returns></returns>
        public static TileTypeVm ToTileTypeVm(TileType tile)
        {
            return new TileTypeVm
            {
                DisplayColor = tile.DisplayColor,
                Label = tile.Label,
                TopGlues = new GlueVms(tile.TopGlues.Values.Select(GlueVm.ToGlueVm)),
                BottomGlues = new GlueVms(tile.BottomGlues.Values.Select(GlueVm.ToGlueVm)),
                LeftGlues = new GlueVms(tile.LeftGlues.Values.Select(GlueVm.ToGlueVm)),
                RightGlues = new GlueVms(tile.RightGlues.Values.Select(GlueVm.ToGlueVm)),
            };
        }

        /// <summary>
        /// Convert a TileTypeVm to a TileType
        /// </summary>
        /// <param name="tile">TileTypeVm to be converted to TileType</param>
        /// <returns>TileType</returns>
        public static TileType ToTileType(TileTypeVm tile)
        {
            return new TileType
            {
                DisplayColor = tile.DisplayColor,
                Label = tile.Label,
                TopGlues = new ObservableDictionary<string, Glue>(tile.TopGlues.ToDictionary(vm => vm.Label, GlueVm.ToGlue)),
                BottomGlues = new ObservableDictionary<string, Glue>(tile.BottomGlues.ToDictionary(vm => vm.Label, GlueVm.ToGlue)),
                LeftGlues = new ObservableDictionary<string, Glue>(tile.LeftGlues.ToDictionary(vm => vm.Label, GlueVm.ToGlue)),
                RightGlues = new ObservableDictionary<string, Glue>(tile.RightGlues.ToDictionary(vm => vm.Label, GlueVm.ToGlue)),
            };
        }
    }
}