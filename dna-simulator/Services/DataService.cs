using dna_simulator.Exceptions;
using dna_simulator.Model;
using dna_simulator.Model.Atam;
using dna_simulator.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Xml.Linq;

namespace dna_simulator.Services
{
    public class DataService : IDataService
    {
        private const string CurrentTas = "CurrentTileAssemblySystem.xml";
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        private ObservableDictionary<GlueLabel, Glue> _glues;
        private TileAssemblySystem _tileAssemblySystem;

        public DataService()
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(CurrentTas)) store.DeleteFile(CurrentTas);
                if (!store.FileExists(CurrentTas))
                {
                    TileAssemblySystem = new TileAssemblySystem
                    {
                        Seed = null,
                        Temperature = 0,
                        TileTypes = new ObservableDictionary<string, TileType>()
                    };
                    Glues = new ObservableDictionary<GlueLabel, Glue>();
                    SaveAllToStorage();
                }
                else
                {
                    using (
                        var tasFile = store.OpenFile(CurrentTas, FileMode.Open, FileAccess.Read))
                    {
                        XDocument data = XDocument.Load(tasFile);
                        var glues =
                            new List<Glue>(
                                from x in data.Elements("TileAssemblySystem").Elements("Glues").Elements("Glue")
                                select new Glue
                                {
                                    Label = x.Attribute("Label").Value,
                                    DisplayColor = ToColor(x.Attribute("DisplayColor").Value),
                                    Color = int.Parse(x.Attribute("Color").Value),
                                    Strength = int.Parse(x.Attribute("Strength").Value)
                                });
                        var tileTypes =
                            new List<TileType>(
                                from x in data.Elements("TileAssemblySystem").Elements("TileTypes").Elements("TileType")
                                let topGlues = new ObservableSet<GlueLabel>(
                                    x.Elements("Top").Elements("Glue").Select(t => new GlueLabel(t.Attribute("Label").Value)))
                                let bottomGlues = new ObservableSet<GlueLabel>(
                                    x.Elements("Bottom").Elements("Glue").Select(t => new GlueLabel(t.Attribute("Label").Value)))
                                let leftGlues = new ObservableSet<GlueLabel>(
                                    x.Elements("Left").Elements("Glue").Select(t => new GlueLabel(t.Attribute("Label").Value)))
                                let rightGlues = new ObservableSet<GlueLabel>(
                                    x.Elements("Right").Elements("Glue").Select(t => new GlueLabel(t.Attribute("Label").Value)))
                                select new TileType
                                {
                                    Label = x.Attribute("Label").Value,
                                    DisplayColor = ToColor(x.Attribute("DisplayColor").Value),
                                    TopGlues = topGlues,
                                    BottomGlues = bottomGlues,
                                    LeftGlues = leftGlues,
                                    RightGlues = rightGlues
                                });
                        TileAssemblySystem = new TileAssemblySystem
                        {
                            TileTypes =
                                new ObservableDictionary<string, TileType>(tileTypes.ToDictionary(t => t.Label)),
                            Seed =
                                tileTypes.First(
                                    t =>
                                        t.Label == data.Elements("TileAssemblySystem").Attributes("Seed").First().Value),
                            Temperature =
                                int.Parse(data.Elements("TileAssemblySystem").Attributes("Temperature").First().Value)
                        };
                        Glues = new ObservableDictionary<GlueLabel, Glue>(glues.ToDictionary(g => new GlueLabel(g.Label)));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public ObservableDictionary<GlueLabel, Glue> Glues
        {
            get { return _glues; }
            set
            {
                if (Equals(value, _glues)) return;
                _glues = value;
                OnPropertyChanged();
            }
        }

        public TileAssemblySystem GetTileAssemblySystem()
        {
            return _tileAssemblySystem;
        }

        public void SetTemperature(int temperature)
        {
            _tileAssemblySystem.Temperature = temperature;
            // TODO: Update temperature in XML
        }

        public void SetSeed(TileType tile)
        {
            TileAssemblySystem.Seed = tile;
            // TODO: Update seed in XML
        }

        public TileType AddTile()
        {
            int tileId = 0;
            string label = "Tile " + tileId;
            while (TileAssemblySystem.TileTypes.ContainsKey(label))
            {
                label = "Tile " + ++tileId;
            }
            var newTile = new TileType
            {
                Label = label,
                DisplayColor = RandomColor(),
                TopGlues = new ObservableSet<GlueLabel>(),
                BottomGlues = new ObservableSet<GlueLabel>(),
                LeftGlues = new ObservableSet<GlueLabel>(),
                RightGlues = new ObservableSet<GlueLabel>()
            };
            return AddTile(newTile);
        }

        public TileType AddTile(TileType tile)
        {
            _tileAssemblySystem.TileTypes.Add(tile.Label, tile);
            AddTileToStorage(tile);
            return tile;
        }

        private void AddTileToStorage(TileType tile)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                XDocument root;
                using (var file = store.OpenFile(CurrentTas, FileMode.Open, FileAccess.ReadWrite))
                {
                    root = XDocument.Load(file);
                    root.Elements("TileAssemblySystem").Elements("TileTypes").First().Add(
                        new XElement("TileType",
                            new XAttribute("Label", tile.Label),
                            new XAttribute("DisplayColor", tile.DisplayColor),
                            new XElement("Top", tile.TopGlues.Select(g => new XElement("Glue", g))),
                            new XElement("Bottom", tile.BottomGlues.Select(g => new XElement("Glue", g))),
                            new XElement("Left", tile.LeftGlues.Select(g => new XElement("Glue", g))),
                            new XElement("Right", tile.RightGlues.Select(g => new XElement("Glue", g)))
                            ));
                }
                // TODO: It appears that XDocument.Save acts strangely with isolated storage; easiest, slowest solution is to recreate:
                if (store.FileExists(CurrentTas))
                {
                    store.DeleteFile(CurrentTas);
                }
                using (var file = store.CreateFile(CurrentTas))
                {
                    root.Save(file);
                }
            }
        }

        public void RemoveTiles(List<TileType> tiles)
        {
            throw new NotImplementedException();
        }

        public Glue AddGlue()
        {
            var glue = DefaultGlue();
            Glues.Add(new GlueLabel(glue.Label), glue);
            return glue;
        }

        public Glue AddGlue(Glue glue)
        {
            Glues.Add(new GlueLabel(glue.Label), glue);
            return glue;
        }

        public Glue AddGlue(string tileLabel, string edge)
        {
            var glue = DefaultGlue();
            AddGlue(glue);
            return AddGlue(glue, tileLabel, edge);
        }

        public Glue AddGlue(Glue glue, string tileLabel, string edge)
        {
            var glueLabel = new GlueLabel(glue.Label);
            switch (edge)
            {
                case "Top":
                    if (TileAssemblySystem.TileTypes[tileLabel] == null)
                        throw new InvalidTileTypeException("No tile type found with label " + tileLabel);
                    TileAssemblySystem.TileTypes[tileLabel].TopGlues.Add(glueLabel);
                    break;

                case "Bottom":
                    if (TileAssemblySystem.TileTypes[tileLabel] == null)
                        throw new InvalidTileTypeException("No tile type found with label " + tileLabel);
                    TileAssemblySystem.TileTypes[tileLabel].BottomGlues.Add(glueLabel);
                    break;

                case "Left":
                    if (TileAssemblySystem.TileTypes[tileLabel] == null)
                        throw new InvalidTileTypeException("No tile type found with label " + tileLabel);
                    TileAssemblySystem.TileTypes[tileLabel].LeftGlues.Add(glueLabel);
                    break;

                case "Right":
                    if (TileAssemblySystem.TileTypes[tileLabel] == null)
                        throw new InvalidTileTypeException("No tile type found with label " + tileLabel);
                    TileAssemblySystem.TileTypes[tileLabel].RightGlues.Add(glueLabel);
                    break;
            }
            // TODO: store in XML file
            return glue;
        }

        public void RemoveGlues(List<Glue> glues)
        {
            foreach (var glue in glues)
            {
                Glues.Remove(new GlueLabel(glue.Label));
                foreach (var tile in GetTileAssemblySystem().TileTypes.Values)
                {
                    tile.TopGlues.Remove(new GlueLabel(glue.Label));
                    tile.BottomGlues.Remove(new GlueLabel(glue.Label));
                    tile.LeftGlues.Remove(new GlueLabel(glue.Label));
                    tile.RightGlues.Remove(new GlueLabel(glue.Label));
                }
            }
        }

        public void RemoveGlues(List<Glue> glues, string tileLabel, string edge)
        {
            foreach (var glue in glues)
            {
                var glueLabel = new GlueLabel(glue.Label);
                switch (edge)
                {
                    case "Top":
                        if (TileAssemblySystem.TileTypes[tileLabel] == null)
                            throw new InvalidTileTypeException("No tile type found with label " + tileLabel);
                        TileAssemblySystem.TileTypes[tileLabel].TopGlues.Remove(glueLabel);
                        break;

                    case "Bottom":
                        if (TileAssemblySystem.TileTypes[tileLabel] == null)
                            throw new InvalidTileTypeException("No tile type found with label " + tileLabel);
                        TileAssemblySystem.TileTypes[tileLabel].BottomGlues.Remove(glueLabel);
                        break;

                    case "Left":
                        if (TileAssemblySystem.TileTypes[tileLabel] == null)
                            throw new InvalidTileTypeException("No tile type found with label " + tileLabel);
                        TileAssemblySystem.TileTypes[tileLabel].LeftGlues.Remove(glueLabel);
                        break;

                    case "Right":
                        if (TileAssemblySystem.TileTypes[tileLabel] == null)
                            throw new InvalidTileTypeException("No tile type found with label " + tileLabel);
                        TileAssemblySystem.TileTypes[tileLabel].RightGlues.Remove(glueLabel);
                        break;
                }
            }            // TODO: remove from XML file
        }

        public void Commit()
        {
            SaveAllToStorage();
        }

        private void SaveAllToStorage()
        {
            List<XElement> glues = (from glue in Glues.Values
                                    select
                                        new XElement("Glue", new XAttribute("Label", glue.Label),
                                            new XAttribute("DisplayColor", glue.DisplayColor), new XAttribute("Color", glue.Color),
                                            new XAttribute("Strength", glue.Strength))).ToList();
            List<XElement> tileTypes = (from tile in TileAssemblySystem.TileTypes.Values
                                        let topGlues =
                                            tile.TopGlues.Select(
                                                glue =>
                                                    new XElement("Glue", new XAttribute("Label", glue)))
                                        let bottomGlues =
                                            tile.BottomGlues.Select(
                                                glue =>
                                                    new XElement("Glue", new XAttribute("Label", glue)))
                                        let leftGlues =
                                            tile.LeftGlues.Select(
                                                glue =>
                                                    new XElement("Glue", new XAttribute("Label", glue)))
                                        let rightGlues =
                                            tile.RightGlues.Select(
                                                glue =>
                                                    new XElement("Glue", new XAttribute("Label", glue)))
                                        select
                                            new XElement("TileType", new XAttribute("Label", tile.Label),
                                                new XAttribute("DisplayColor", tile.DisplayColor), new XElement("Top", topGlues),
                                                new XElement("Bottom", bottomGlues), new XElement("Left", leftGlues),
                                                new XElement("Right", rightGlues))).ToList();
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(CurrentTas))
                {
                    store.DeleteFile(CurrentTas);
                }
                using (
                    IsolatedStorageFileStream file = store.CreateFile(CurrentTas)
                    )
                {
                    new XElement("TileAssemblySystem",
                        new XAttribute("Seed", TileAssemblySystem.Seed == null ? "" : TileAssemblySystem.Seed.Label),
                        new XAttribute("Temperature", TileAssemblySystem.Temperature),
                        new XElement("TileTypes", tileTypes),
                        new XElement("Glues", glues)).Save(file);
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private Glue DefaultGlue()
        {
            int glueId = 0;
            string label = "Label " + glueId;
            while (Glues.Any(g => g.Key.Label == label))
            {
                label = "Label " + ++glueId;
            }
            return new Glue
            {
                Label = label,
                DisplayColor = RandomColor(),
                Color = 0,
                Strength = 0
            };
        }

        private static Color RandomColor()
        {
            while (true)
            {
                Type colorsType = typeof(Colors);

                PropertyInfo[] properties = colorsType.GetProperties();

                int random = Random.Next(properties.Length);
                var result = (Color)properties[random].GetValue(null, null);

                if (result == Colors.White || result == Colors.Transparent)
                    continue;
                return result;
            }
        }

        private static Color ToColor(string hex)
        {
            if (hex.First() == '#')
            {
                hex = hex.Remove(0, 1);
            }

            switch (hex.Length)
            {
                case 6:
                    return new Color
                    {
                        R = Byte.Parse(hex.Remove(2), NumberStyles.HexNumber),
                        G = Byte.Parse(hex.Remove(0, 2).Remove(2), NumberStyles.HexNumber),
                        B = Byte.Parse(hex.Remove(0, 4), NumberStyles.HexNumber)
                    };

                case 8:
                    return new Color
                    {
                        A = Byte.Parse(hex.Remove(2), NumberStyles.HexNumber),
                        R = Byte.Parse(hex.Remove(0, 2).Remove(2), NumberStyles.HexNumber),
                        G = Byte.Parse(hex.Remove(0, 4).Remove(2), NumberStyles.HexNumber),
                        B = Byte.Parse(hex.Remove(0, 6), NumberStyles.HexNumber)
                    };

                default:
                    throw new FormatException("Expected either RGB or ARGB hex value.");
            }
        }
    }
}