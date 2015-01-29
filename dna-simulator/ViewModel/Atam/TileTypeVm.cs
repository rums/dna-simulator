using System.Collections.Generic;
using System.Collections.Specialized;
using dna_simulator.Model;
using dna_simulator.Model.Atam;
using dna_simulator.Services;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace dna_simulator.ViewModel.Atam
{
    public class TileTypeVm : ViewModelBase
    {
        // from model
        private string _label;

        private Color _displayColor;
        private AttachedGluesVm _topGlues;
        private AttachedGluesVm _bottomGlues;
        private AttachedGluesVm _leftGlues;
        private AttachedGluesVm _rightGlues;

        // viewmodel specific
        private bool _isSeed;

        private readonly TileType _tileType;
        private readonly IDataService _dataService;

        public TileTypeVm(TileType tileType, IDataService dataService)
        {
            _dataService = dataService;
            _tileType = tileType;
            var tileAssemblySystem = _dataService.TileAssemblySystem;

            DisplayColor = _tileType.DisplayColor;
            Label = _tileType.Label;
            TopGlues = new AttachedGluesVm(_tileType.TopGlues.Select(g => new GlueVm(_dataService.Glues[g]))) { FocusedTile = this, FocusedEdge = "Top" };
            BottomGlues = new AttachedGluesVm(_tileType.BottomGlues.Select(g => new GlueVm(_dataService.Glues[g]))) { FocusedTile = this, FocusedEdge = "Bottom" };
            LeftGlues = new AttachedGluesVm(_tileType.LeftGlues.Select(g => new GlueVm(_dataService.Glues[g]))) { FocusedTile = this, FocusedEdge = "Left" };
            RightGlues = new AttachedGluesVm(_tileType.RightGlues.Select(g => new GlueVm(_dataService.Glues[g]))) { FocusedTile = this, FocusedEdge = "Right" };
            IsSeed = tileAssemblySystem.Seed == null || Equals(_tileType.Label, tileAssemblySystem.Seed.Label);

            _tileType.PropertyChanged += TileTypeOnPropertyChanged;
            _onAttachedGluesOnCollectionChanged.Add(TopGlues, (s, e) => AttachedGluesOnCollectionChanged(e, TopGlues));
            _tileType.TopGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[TopGlues];
            _onAttachedGluesOnCollectionChanged.Add(BottomGlues, (s, e) => AttachedGluesOnCollectionChanged(e, BottomGlues));
            _tileType.BottomGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[BottomGlues];
            _onAttachedGluesOnCollectionChanged.Add(LeftGlues, (s, e) => AttachedGluesOnCollectionChanged(e, LeftGlues));
            _tileType.LeftGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[LeftGlues];
            _onAttachedGluesOnCollectionChanged.Add(RightGlues, (s, e) => AttachedGluesOnCollectionChanged(e, RightGlues));
            _tileType.RightGlues.CollectionChanged += _onAttachedGluesOnCollectionChanged[RightGlues];
        }

        // Using dictionary of handlers so we can pass additional arg and unregister later
        private readonly Dictionary<AttachedGluesVm, NotifyCollectionChangedEventHandler> _onAttachedGluesOnCollectionChanged = new Dictionary<AttachedGluesVm, NotifyCollectionChangedEventHandler>();

        private void AttachedGluesOnCollectionChanged(NotifyCollectionChangedEventArgs e, AttachedGluesVm glues)
        {
            if (e.NewItems != null)
            {
                if (e.NewItems.OfType<KeyValuePair<GlueLabel, Glue>>().Any())
                {
                    foreach (KeyValuePair<GlueLabel, Glue> item in e.NewItems)
                    {
                        glues.Add(new GlueVm(_dataService.Glues[item.Key]));
                        item.Value.PropertyChanged += glues.First(g => g.Label == item.Key.Label).GlueOnPropertyChanged;
                    }
                }
                else
                {
                    foreach (GlueLabel item in e.NewItems)
                    {
                        glues.Add(new GlueVm(_dataService.Glues[item]));
                        _dataService.Glues[item].PropertyChanged += glues.First(g => g.Label == item.Label).GlueOnPropertyChanged;
                    }
                }
            }
            if (e.OldItems != null)
            {
                if (e.OldItems.OfType<KeyValuePair<GlueLabel, Glue>>().Any())
                {
                    foreach (KeyValuePair<GlueLabel, Glue> item in e.OldItems)
                    {
                        item.Value.PropertyChanged -= glues.First(g => g.Label == item.Key.Label).GlueOnPropertyChanged;
                        glues.Remove(new GlueVm(item.Value));
                    }
                }
                else if (e.OldItems.OfType<GlueLabel>().Any())
                {
                    foreach (GlueLabel item in e.OldItems)
                    {
                        _dataService.Glues[item].PropertyChanged -= glues.First(g => g.Label == item.Label).GlueOnPropertyChanged;
                        glues.Remove(glues.First(g => g.Label == item.Label));
                    }
                }
            }
        }

        public string Label
        {
            get { return _label; }
            set
            {
                if (value == _label) return;
                _label = value;
                _tileType.Label = value;
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
                _tileType.DisplayColor = value;
                RaisePropertyChanged();
            }
        }

        public AttachedGluesVm TopGlues
        {
            get { return _topGlues; }
            set
            {
                if (Equals(value, _topGlues)) return;
                _topGlues = value;
                RaisePropertyChanged();
            }
        }

        public AttachedGluesVm BottomGlues
        {
            get { return _bottomGlues; }
            set
            {
                if (Equals(value, _bottomGlues)) return;
                _bottomGlues = value;
                RaisePropertyChanged();
            }
        }

        public AttachedGluesVm LeftGlues
        {
            get { return _leftGlues; }
            set
            {
                if (Equals(value, _leftGlues)) return;
                _leftGlues = value;
                RaisePropertyChanged();
            }
        }

        public AttachedGluesVm RightGlues
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

        public void TileTypeOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Label":
                    Label = _tileType.Label;
                    break;

                case "DisplayColor":
                    DisplayColor = _tileType.DisplayColor;
                    break;

                case "TopGlues":
                    TopGlues = new AttachedGluesVm(_tileType.TopGlues.Select(g => new GlueVm(_dataService.Glues[g]))) { FocusedTile = this, FocusedEdge = "Top" };
                    break;

                case "BottomGlues":
                    BottomGlues = new AttachedGluesVm(_tileType.BottomGlues.Select(g => new GlueVm(_dataService.Glues[g]))) { FocusedTile = this, FocusedEdge = "Bottom" };
                    break;

                case "LeftGlues":
                    LeftGlues = new AttachedGluesVm(_tileType.LeftGlues.Select(g => new GlueVm(_dataService.Glues[g]))) { FocusedTile = this, FocusedEdge = "Left" };
                    break;

                case "RightGlues":
                    RightGlues = new AttachedGluesVm(_tileType.RightGlues.Select(g => new GlueVm(_dataService.Glues[g]))) { FocusedTile = this, FocusedEdge = "Right" };
                    break;
            }
        }

        /// <summary>
        /// Convert a TileTypeVm to a TileType
        /// </summary>
        /// <param name="tile">TileTypeVm to be converted to TileType</param>
        /// <returns>new TileType</returns>
        public static TileType ToTileType(TileTypeVm tile)
        {
            return new TileType
            {
                DisplayColor = tile.DisplayColor,
                Label = tile.Label,
                TopGlues = new ObservableSet<GlueLabel>(tile.TopGlues.Select(g => new GlueLabel(g.Label))),
                BottomGlues = new ObservableSet<GlueLabel>(tile.BottomGlues.Select(g => new GlueLabel(g.Label))),
                LeftGlues = new ObservableSet<GlueLabel>(tile.LeftGlues.Select(g => new GlueLabel(g.Label))),
                RightGlues = new ObservableSet<GlueLabel>(tile.RightGlues.Select(g => new GlueLabel(g.Label))),
            };
        }
    }
}