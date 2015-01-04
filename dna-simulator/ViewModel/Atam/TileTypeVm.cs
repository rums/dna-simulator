using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using dna_simulator.Model;
using dna_simulator.Model.Atam;

namespace dna_simulator.ViewModel.Atam
{
    public class TileTypeVm : ViewModelBase
    {
        // from model
        private int _id;
        private string _label;
        private Color _displayColor;
        private ObservableCollection<GlueVm> _topEdges;
        private ObservableCollection<GlueVm> _bottomEdges;
        private ObservableCollection<GlueVm> _leftEdges;
        private ObservableCollection<GlueVm> _rightEdges;

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

        public ObservableCollection<GlueVm> TopEdges
        {
            get { return _topEdges; }
            set
            {
                if (Equals(value, _topEdges)) return;
                _topEdges = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<GlueVm> BottomEdges
        {
            get { return _bottomEdges; }
            set
            {
                if (Equals(value, _bottomEdges)) return;
                _bottomEdges = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<GlueVm> LeftEdges
        {
            get { return _leftEdges; }
            set
            {
                if (Equals(value, _leftEdges)) return;
                _leftEdges = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<GlueVm> RightEdges
        {
            get { return _rightEdges; }
            set
            {
                if (Equals(value, _rightEdges)) return;
                _rightEdges = value;
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
            tilevm.IsSeed = Equals(tile.Id, tileAssemblySystem.Seed.Id);
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
                Id = tile.Id,
                DisplayColor = tile.DisplayColor,
                Label = tile.Label,
                TopEdges = new ObservableCollection<GlueVm>(tile.TopEdges.Select(GlueVm.ToGlueVm)),
                BottomEdges = new ObservableCollection<GlueVm>(tile.BottomEdges.Select(GlueVm.ToGlueVm)),
                LeftEdges = new ObservableCollection<GlueVm>(tile.LeftEdges.Select(GlueVm.ToGlueVm)),
                RightEdges = new ObservableCollection<GlueVm>(tile.RightEdges.Select(GlueVm.ToGlueVm)),
            };
        }

        /// <summary>
        /// Convert a TileTypeVm to a TileType
        /// </summary>
        /// <param name="tile">TileTypeVm to be converted to TileType</param>
        /// <returns>TileType</returns>
        public static TileType ToTileTypeBase(TileTypeVm tile)
        {
            return new TileType
            {
                Id = tile.Id,
                DisplayColor = tile.DisplayColor,
                Label = tile.Label,
                TopEdges = new ObservableCollection<Glue>(tile.TopEdges.Select(GlueVm.ToGlue)),
                BottomEdges = new ObservableCollection<Glue>(tile.BottomEdges.Select(GlueVm.ToGlue)),
                LeftEdges = new ObservableCollection<Glue>(tile.LeftEdges.Select(GlueVm.ToGlue)),
                RightEdges = new ObservableCollection<Glue>(tile.RightEdges.Select(GlueVm.ToGlue)),
            };
        }
    }
}