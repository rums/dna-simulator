using System;
using System.ComponentModel;
using System.Windows.Media;
using dna_simulator.Model.Atam;

namespace dna_simulator.ViewModel.Atam
{
    public class TileTypeVm : ViewModelBase
    {
        // from model
        private Color _displayColor;
        private string _label;
        private GlueVm _top;
        private GlueVm _bottom;
        private GlueVm _left;
        private GlueVm _right;

        // viewmodel specific
        private bool _isSeed;

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

        public GlueVm Top
        {
            get { return _top; }
            set
            {
                if (Equals(value, _top)) return;
                _top = value;
                RaisePropertyChanged();
            }
        }

        public GlueVm Bottom
        {
            get { return _bottom; }
            set
            {
                if (Equals(value, _bottom)) return;
                _bottom = value;
                RaisePropertyChanged();
            }
        }

        public GlueVm Left
        {
            get { return _left; }
            set
            {
                if (Equals(value, _left)) return;
                _left = value;
                RaisePropertyChanged();
            }
        }

        public GlueVm Right
        {
            get { return _right; }
            set
            {
                if (Equals(value, _right)) return;
                _right = value;
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

        #region Static helper methods

        /// <summary>
        /// Convert a TileTypeVm to a TileType
        /// </summary>
        /// <param name="tile">TileTypeVm to be converted to TileType</param>
        /// <returns></returns>
        public static TileType ToTileType(TileTypeVm tile)
        {
            return new TileType
            {
                DisplayColor = tile.DisplayColor,
                Label = tile.Label,
                Top = new Glue { Color = tile.Top.Color, Strength = tile.Top.Strength, DisplayColor = tile.Top.DisplayColor },
                Bottom = new Glue { Color = tile.Bottom.Color, Strength = tile.Bottom.Strength, DisplayColor = tile.Bottom.DisplayColor },
                Left = new Glue { Color = tile.Left.Color, Strength = tile.Left.Strength, DisplayColor = tile.Left.DisplayColor },
                Right = new Glue { Color = tile.Right.Color, Strength = tile.Right.Strength, DisplayColor = tile.Right.DisplayColor },
            };
        }

        /// <summary>
        /// Convert a TileType to a TileTypeVm
        /// </summary>
        /// <param name="tile">TileType to be converted to TileTypeVm</param>
        /// <param name="tileAssemblySystem">TileAssemblySystem in which this TileType resides</param>
        /// <returns></returns>
        public static TileTypeVm ToTileTypeVm(TileType tile, TileAssemblySystem tileAssemblySystem)
        {
            var tileWithSeed = ToTileTypeVm(tile);
            tileWithSeed.IsSeed = Equals(tile.Label, tileAssemblySystem.Seed.Label);
            return tileWithSeed;
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
                Top = new GlueVm { Label = "Top", Color = tile.Top.Color, Strength = tile.Top.Strength, DisplayColor = tile.Top.DisplayColor },
                Bottom = new GlueVm { Label = "Bottom", Color = tile.Bottom.Color, Strength = tile.Bottom.Strength, DisplayColor = tile.Bottom.DisplayColor },
                Left = new GlueVm { Label = "Left", Color = tile.Left.Color, Strength = tile.Left.Strength, DisplayColor = tile.Left.DisplayColor },
                Right = new GlueVm { Label = "Right", Color = tile.Right.Color, Strength = tile.Right.Strength, DisplayColor = tile.Right.DisplayColor },
            };
        }

        #endregion
    }
}
