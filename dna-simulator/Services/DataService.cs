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
using dna_simulator.Model;
using dna_simulator.Model.Atam;
using dna_simulator.Properties;

namespace dna_simulator.Services
{
    public class DataService : IDataService
    {
        private const string CurrentTas = "CurrentTileAssemblySystem.xml";
        private static readonly Random Random = new Random((int) DateTime.Now.Ticks);
        private int _glueId;

        private TileAssemblySystem _tileAssemblySystem;
        private int _tileId;

        public DataService()
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.FileExists(CurrentTas)) return;
                using (
                    var tasFile = new IsolatedStorageFileStream(CurrentTas, FileMode.Open, FileAccess.ReadWrite,
                        store))
                {
                    XDocument data = XDocument.Load(tasFile);
                    var tileTypes =
                        new List<TileType>(
                            from x in data.Elements("TileAssemblySystem").Elements("TileTypes").Elements("TileType")
                            let topGlues = new ObservableDictionary<string, Glue>(
                                x.Elements("Top").Elements("Glue").ToDictionary(t => t.Attribute("Label").Value,
                                    t => new Glue
                                    {
                                        Label = t.Attribute("Label").Value,
                                        DisplayColor = ToColor(t.Attribute("DisplayColor").Value),
                                        Color = int.Parse(t.Attribute("Color").Value),
                                        Strength = int.Parse(t.Attribute("Strength").Value)
                                    }))
                            let bottomGlues = new ObservableDictionary<string, Glue>(
                                x.Elements("Bottom").Elements("Glue").ToDictionary(t => t.Attribute("Label").Value,
                                    t => new Glue
                                    {
                                        Label = t.Attribute("Label").Value,
                                        DisplayColor = ToColor(t.Attribute("DisplayColor").Value),
                                        Color = int.Parse(t.Attribute("Color").Value),
                                        Strength = int.Parse(t.Attribute("Strength").Value)
                                    }))
                            let leftGlues = new ObservableDictionary<string, Glue>(
                                x.Elements("Left").Elements("Glue").ToDictionary(t => t.Attribute("Label").Value,
                                    t => new Glue
                                    {
                                        Label = t.Attribute("Label").Value,
                                        DisplayColor = ToColor(t.Attribute("DisplayColor").Value),
                                        Color = int.Parse(t.Attribute("Color").Value),
                                        Strength = int.Parse(t.Attribute("Strength").Value)
                                    }))
                            let rightGlues = new ObservableDictionary<string, Glue>(
                                x.Elements("Right").Elements("Glue").ToDictionary(t => t.Attribute("Label").Value,
                                    t => new Glue
                                    {
                                        Label = t.Attribute("Label").Value,
                                        DisplayColor = ToColor(t.Attribute("DisplayColor").Value),
                                        Color = int.Parse(t.Attribute("Color").Value),
                                        Strength = int.Parse(t.Attribute("Strength").Value)
                                    }))
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

        public void NewDefaultTile(Action<TileType, Exception> callback)
        {
            var label = "Tile " + _tileId;
            while (TileAssemblySystem.TileTypes.ContainsKey(label))
            {
                label = "Tile " + ++_tileId;
            }
            var newTile = new TileType
            {
                Label = label,
                DisplayColor = RandomColor(),
                TopGlues = new ObservableDictionary<string, Glue>(),
                BottomGlues = new ObservableDictionary<string, Glue>(),
                LeftGlues = new ObservableDictionary<string, Glue>(),
                RightGlues = new ObservableDictionary<string, Glue>()
            };
            ++_tileId;
            callback(newTile, null);
        }

        public void NewDefaultGlue(Action<Glue, Exception> callback)
        {
            var label = "Label " + _glueId;
            while (TileAssemblySystem.TileTypes.Values.SelectMany(
                        t => t.TopGlues.Keys.Union(t.BottomGlues.Keys.Union(t.LeftGlues.Keys.Union(t.RightGlues.Keys)))).Contains(label))
            {
                label = "Label " + ++_glueId;
            }
            var newGlue = new Glue
            {
                Label = label,
                DisplayColor = RandomColor(),
                Color = 0,
                Strength = 0
            };
            ++_glueId;
            callback(newGlue, null);
        }

        public void Commit()
        {
            //_mockData = TileAssemblySystem;
            //OnPropertyChanged("TileAssemblySystem");
            List<XElement> tileTypes = (from tile in TileAssemblySystem.TileTypes.Values
                let topGlues =
                    tile.TopGlues.Values.Select(
                        glue =>
                            new XElement("Glue", new XAttribute("Label", glue.Label),
                                new XAttribute("DisplayColor", glue.DisplayColor), new XAttribute("Color", glue.Color),
                                new XAttribute("Strength", glue.Strength))).ToList()
                let bottomGlues =
                    tile.BottomGlues.Values.Select(
                        glue =>
                            new XElement("Glue", new XAttribute("Label", glue.Label),
                                new XAttribute("DisplayColor", glue.DisplayColor), new XAttribute("Color", glue.Color),
                                new XAttribute("Strength", glue.Strength))).ToList()
                let leftGlues =
                    tile.LeftGlues.Values.Select(
                        glue =>
                            new XElement("Glue", new XAttribute("Label", glue.Label),
                                new XAttribute("DisplayColor", glue.DisplayColor), new XAttribute("Color", glue.Color),
                                new XAttribute("Strength", glue.Strength))).ToList()
                let rightGlues =
                    tile.RightGlues.Values.Select(
                        glue =>
                            new XElement("Glue", new XAttribute("Label", glue.Label),
                                new XAttribute("DisplayColor", glue.DisplayColor), new XAttribute("Color", glue.Color),
                                new XAttribute("Strength", glue.Strength))).ToList()
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
                    IsolatedStorageFileStream file = store.OpenFile(CurrentTas, FileMode.OpenOrCreate, FileAccess.Write)
                    )
                {
                    new XElement("TileAssemblySystem",
                        new XAttribute("Seed", TileAssemblySystem.Seed.Label),
                        new XAttribute("Temperature", TileAssemblySystem.Temperature),
                        new XElement("TileTypes", tileTypes)).Save(file);
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

        private static Color RandomColor()
        {
            while (true)
            {
                Type colorsType = typeof (Colors);

                PropertyInfo[] properties = colorsType.GetProperties();

                int random = Random.Next(properties.Length);
                var result = (Color) properties[random].GetValue(null, null);

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