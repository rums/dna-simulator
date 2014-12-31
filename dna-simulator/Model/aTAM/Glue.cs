﻿using System.Windows.Media;

namespace dna_simulator.Model.Atam
{
    public class Glue : ModelBase
    {
        private Color _displayColor;
        private int _color;
        private int _strength;

        public Color DisplayColor
        {
            get { return _displayColor; }
            set
            {
                if (value.Equals(_displayColor)) return;
                _displayColor = value;
                OnPropertyChanged();
            }
        }

        public int Color
        {
            get { return _color; }
            set
            {
                if (value == _color) return;
                _color = value;
                OnPropertyChanged();
            }
        }

        public int Strength
        {
            get { return _strength; }
            set
            {
                if (value == _strength) return;
                _strength = value;
                OnPropertyChanged();
            }
        }
    }
}