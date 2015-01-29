using System.Collections;
using System.Collections.Generic;
using System.Linq;
using dna_simulator.Exceptions;
using dna_simulator.Model.Atam;
using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace dna_simulator.ViewModel.Configuration
{
    public class SingleTileViewModel : ViewModelBase
    {
        private readonly IColorPickerService _colorPickerService;
        private readonly IDataService _dataService;
        private ViewModelBase _currentEditorModel;
        private TileTypeVm _currentTileTypeVm;
        private GlueEditorViewModel _glueEditorViewModel;

        public SingleTileViewModel(IServiceBundle serviceBundle)
        {
            _dataService = serviceBundle.DataService;
            _colorPickerService = serviceBundle.ColorPickerService;

            // initialize properties
            CurrentTileTypeVm = null;
            CurrentEditorModel = null;
            _glueEditorViewModel = new GlueEditorViewModel();

            // initialize commands
            ChangeGlueDisplayColorCommand = new RelayCommand<string>(ChangeGlueDisplayColor);
            ConfigureGlueCommand = new RelayCommand<GlueVm>(ConfigureGlue);
            ConfigureTileCommand = new RelayCommand(ConfigureTile);
            ChangeTileDisplayColorCommand = new RelayCommand<string>(ChangeTileDisplayColor);
            DisplayTileTypeCommand = new RelayCommand<TileTypeVm>(DisplayTileType);
            AddGlueToTileCommand = new RelayCommand<object>(AddGlueToTile);
            RemoveGluesFromTopCommand = new RelayCommand<object>(RemoveGluesFromTop);
            RemoveGluesFromBottomCommand = new RelayCommand<object>(RemoveGluesFromBottom);
            RemoveGluesFromLeftCommand = new RelayCommand<object>(RemoveGluesFromLeft);
            RemoveGluesFromRightCommand = new RelayCommand<object>(RemoveGluesFromRight);

            // register listeners
            Messenger.Default.Register<NotificationMessage<TileTypeVm>>(this, message =>
            {
                TileTypeVm tile = message.Content;
                switch (message.Notification)
                {
                    case "Focus tile":
                        DisplayTileType(tile);
                        break;
                }
            });
        }

        public TileTypeVm CurrentTileTypeVm
        {
            get { return _currentTileTypeVm; }
            set
            {
                if (Equals(value, _currentTileTypeVm)) return;
                _currentTileTypeVm = value;
                RaisePropertyChanged();
            }
        }

        public ViewModelBase CurrentEditorModel
        {
            get { return _currentEditorModel; }
            set
            {
                if (Equals(value, _currentEditorModel)) return;
                _currentEditorModel = value;
                RaisePropertyChanged();
            }
        }

        public GlueEditorViewModel GlueEditorViewModel
        {
            get { return _glueEditorViewModel; }
            set
            {
                if (Equals(value, _glueEditorViewModel)) return;
                _glueEditorViewModel = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<string> ChangeGlueDisplayColorCommand { get; private set; }

        public RelayCommand<GlueVm> ConfigureGlueCommand { get; private set; }

        public RelayCommand ConfigureTileCommand { get; private set; }

        public RelayCommand<string> ChangeTileDisplayColorCommand { get; private set; }

        public RelayCommand<TileTypeVm> DisplayTileTypeCommand { get; private set; }

        public RelayCommand<object> AddGlueToTileCommand { get; private set; }

        public RelayCommand<object> RemoveGluesFromTopCommand { get; private set; }

        public RelayCommand<object> RemoveGluesFromBottomCommand { get; private set; }

        public RelayCommand<object> RemoveGluesFromLeftCommand { get; private set; }

        public RelayCommand<object> RemoveGluesFromRightCommand { get; private set; }

        private void ChangeGlueDisplayColor(string label)
        {
            _colorPickerService.ShowColorPicker(c => { _dataService.Glues[new GlueLabel(label)].DisplayColor = c; });
        }

        private void ConfigureGlue(GlueVm glue)
        {
            if (GlueEditorViewModel.Glues.Contains(glue)) return;
            GlueEditorViewModel.Glues.Add(glue);
            CurrentEditorModel = GlueEditorViewModel;
        }

        private void ConfigureTile()
        {
            GlueEditorViewModel.Glues.Clear();
            CurrentEditorModel = CurrentTileTypeVm;
        }

        private void ChangeTileDisplayColor(string label)
        {
            _colorPickerService.ShowColorPicker(
                c => { _dataService.TileAssemblySystem.TileTypes[label].DisplayColor = c; });
        }

        public void DisplayTileType(TileTypeVm tile)
        {
            // switch tile context
            CurrentTileTypeVm = tile;

            // we should be configuring the new tile
            ConfigureTile();
        }

        public void AddGlueToTile(object o)
        {
            try
            {
                var attachedGlues = o as AttachedGluesVm;
                if (attachedGlues == null) return;
                if (attachedGlues.FocusedGlue == null)
                    _dataService.AddGlue(attachedGlues.FocusedTile.Label, attachedGlues.FocusedEdge);
                else
                {
                    _dataService.AddGlue(GlueVm.ToGlue(attachedGlues.FocusedGlue), attachedGlues.FocusedTile.Label,
                        attachedGlues.FocusedEdge);
                }
            }
            catch (InvalidTileTypeException)
            {
            }
        }

        public void RemoveGluesFromTop(object glues)
        {
            List<GlueVm> toRemove = (glues as IList).Cast<GlueVm>().ToList();
            if (toRemove == null) return;
            _dataService.RemoveGlues(toRemove.Select(GlueVm.ToGlue).ToList(), CurrentTileTypeVm.Label, "Top");

            // TODO: Should probably use a converter to hide the editor when empty
            HideGlueEditorIfEmpty();
        }

        public void RemoveGluesFromBottom(object glues)
        {
            List<GlueVm> toRemove = (glues as IList).Cast<GlueVm>().ToList();
            if (toRemove == null) return;
            _dataService.RemoveGlues(toRemove.Select(GlueVm.ToGlue).ToList(), CurrentTileTypeVm.Label, "Bottom");

            HideGlueEditorIfEmpty();
        }

        public void RemoveGluesFromLeft(object glues)
        {
            List<GlueVm> toRemove = (glues as IList).Cast<GlueVm>().ToList();
            if (toRemove == null) return;
            _dataService.RemoveGlues(toRemove.Select(GlueVm.ToGlue).ToList(), CurrentTileTypeVm.Label, "Left");

            HideGlueEditorIfEmpty();
        }

        public void RemoveGluesFromRight(object glues)
        {
            List<GlueVm> toRemove = (glues as IList).Cast<GlueVm>().ToList();
            if (toRemove == null) return;
            _dataService.RemoveGlues(toRemove.Select(GlueVm.ToGlue).ToList(), CurrentTileTypeVm.Label, "Right");

            HideGlueEditorIfEmpty();
        }

        private void HideGlueEditorIfEmpty()
        {
            if (!(CurrentEditorModel is GlueEditorViewModel)) return;
            if (GlueEditorViewModel.Glues.Count == 0)
            {
                ConfigureTile();
            }
        }
    }
}