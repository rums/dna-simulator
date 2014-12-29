﻿using dna_simulator.Model.Atam;
using GalaSoft.MvvmLight;
using System.Windows.Media;

namespace dna_simulator.ViewModel.Atam
{
    public class SingleTileViewModel : ViewModelBase
    {
        #region Properties

        // From model
        private Color _displayColor;

        private string _label;
        private Glue _topGlue;
        private Glue _bottomGlue;
        private Glue _leftGlue;
        private Glue _rightGlue;

        // ViewModel specific
        private bool _isSeed;

        private Brush _displayColorBrush;
        private Brush _topGlueBrush;
        private Brush _bottomGlueBrush;
        private Brush _leftGlueBrush;
        private Brush _rightGlueBrush;

        public Color DisplayColor
        {
            get { return _displayColor; }
            set
            {
                if (Equals(value, _displayColor)) return;
                _displayColor = value;
                RaisePropertyChanged("DisplayColor");
            }
        }

        public string Label
        {
            get { return _label; }
            set
            {
                if (Equals(value, _label)) return;
                _label = value;
                RaisePropertyChanged("Label");
            }
        }

        public Glue TopGlue
        {
            get { return _topGlue; }
            set
            {
                if (Equals(value, _topGlue)) return;
                _topGlue = value;
                RaisePropertyChanged("TopGlue");
            }
        }

        public Glue BottomGlue
        {
            get { return _bottomGlue; }
            set
            {
                if (Equals(value, _bottomGlue)) return;
                _bottomGlue = value;
                RaisePropertyChanged("BottomGlue");
            }
        }

        public Glue LeftGlue
        {
            get { return _leftGlue; }
            set
            {
                if (Equals(value, _leftGlue)) return;
                _leftGlue = value;
                RaisePropertyChanged("LeftGlue");
            }
        }

        public Glue RightGlue
        {
            get { return _rightGlue; }
            set
            {
                if (Equals(value, _rightGlue)) return;
                _rightGlue = value;
                RaisePropertyChanged("RightGlue");
            }
        }

        public bool IsSeed
        {
            get { return _isSeed; }
            set
            {
                if (Equals(value, _isSeed)) return;
                _isSeed = value;
                RaisePropertyChanged("IsSeed");
            }
        }

        public Brush DisplayColorBrush
        {
            get { return _displayColorBrush; }
            set
            {
                if (Equals(value, _displayColorBrush)) return;
                _displayColorBrush = value;
                RaisePropertyChanged("DisplayColorBrush");
            }
        }

        public Brush TopGlueBrush
        {
            get { return _topGlueBrush; }
            set
            {
                if (Equals(value, _topGlueBrush)) return;
                _topGlueBrush = value;
                RaisePropertyChanged("TopGlueBrush");
            }
        }

        public Brush BottomGlueBrush
        {
            get { return _bottomGlueBrush; }
            set
            {
                if (Equals(value, _bottomGlueBrush)) return;
                _bottomGlueBrush = value;
                RaisePropertyChanged("BottomGlueBrush");
            }
        }

        public Brush LeftGlueBrush
        {
            get { return _leftGlueBrush; }
            set
            {
                if (Equals(value, _leftGlueBrush)) return;
                _leftGlueBrush = value;
                RaisePropertyChanged("LeftGlueBrush");
            }
        }

        public Brush RightGlueBrush
        {
            get { return _rightGlueBrush; }
            set
            {
                if (Equals(value, _rightGlueBrush)) return;
                _rightGlueBrush = value;
                RaisePropertyChanged("RightGlueBrush");
            }
        }

        #endregion Properties
    }
}