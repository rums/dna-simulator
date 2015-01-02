﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using dna_simulator.Annotations;
using dna_simulator.Model;
using dna_simulator.Model.Atam;
using System.Windows.Media;

namespace dna_simulator.Design
{
    public class DesignDataService : IDataService
    {
        #region Implement INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Data source

        //need to setup a database or use xml files or something
        private int _tileId = 0;
        private TileAssemblySystem _mockData = new TileAssemblySystem
        {
            Seed = new TileType
            {
                Id = 0,
                Label = "Tile 100",
                Top = new Glue { Color = 0, Strength = 2, DisplayColor = Colors.Red },
                Bottom = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Blue },
                Left = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Green },
                Right = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Cyan },
                DisplayColor = Colors.Purple,
            },
            Temperature = 0,
            TileTypes = new ObservableDictionary<int, TileType>
            {
                { 0,
                    new TileType
                    {
                        Id = 0,
                        Label = "Tile 100",
                        Top = new Glue { Color = 0, Strength = 2, DisplayColor = Colors.Red },
                        Bottom = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Blue },
                        Left = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Green },
                        Right = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Cyan },
                        DisplayColor = Colors.Purple,
                    }
                }
            }
        };

        #endregion

        #region Constructors

        public DesignDataService()
        {
            TileAssemblySystem = new TileAssemblySystem
            {
                Temperature = _mockData.Temperature,
                TileTypes = new ObservableDictionary<int, TileType>(),
                Seed = _mockData.Seed
            };
            foreach (var tvm in _mockData.TileTypes.Select(t => new TileType
            {
                DisplayColor = t.Value.DisplayColor,
                Label = t.Value.Label,
                Top = new Glue { Color = t.Value.Top.Color, Strength = t.Value.Top.Strength, DisplayColor = t.Value.Top.DisplayColor },
                Bottom = new Glue { Color = t.Value.Bottom.Color, Strength = t.Value.Bottom.Strength, DisplayColor = t.Value.Bottom.DisplayColor },
                Left = new Glue { Color = t.Value.Left.Color, Strength = t.Value.Left.Strength, DisplayColor = t.Value.Left.DisplayColor },
                Right = new Glue { Color = t.Value.Right.Color, Strength = t.Value.Right.Strength, DisplayColor = t.Value.Right.DisplayColor },
            }))
            {
                TileAssemblySystem.TileTypes[tvm.Id] = tvm;
            }
        }

        #endregion

        #region Implement IDataService (sans INotifyPropertyChanged)

        private TileAssemblySystem _tileAssemblySystem;

        public TileAssemblySystem TileAssemblySystem
        {
            get { return _tileAssemblySystem; }
            set
            {
                if (Equals(value, _tileAssemblySystem)) return;
                _tileAssemblySystem = value;
                OnPropertyChanged();
            }
        }

        public void NewDefaultTile(Action<TileType, Exception> callback)
        {
            var tileLabel = RandomTileLabel(4);
            var newTile = new TileType
            {
                Id = ++_tileId,
                Label = tileLabel,
                Top = new Glue { Color = 0, Strength = 2, DisplayColor = Colors.Red },
                Bottom = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Blue },
                Left = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Green },
                Right = new Glue { Color = 0, Strength = 0, DisplayColor = Colors.Cyan },
                DisplayColor = Colors.Purple,
            };
            TileAssemblySystem.TileTypes.Add(_tileId, newTile);
            callback(newTile, null);
        }

        public void Commit()
        {
            _mockData = TileAssemblySystem;
            OnPropertyChanged("TileAssemblySystem");
        }

        #endregion

        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);
        private string RandomTileLabel(int size)
        {
            var builder = new StringBuilder();
            builder.Append("Label ");
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}