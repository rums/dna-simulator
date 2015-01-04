using System.Collections.Generic;
using System.Linq;
using dna_simulator.Services;
using dna_simulator.ViewModel.Atam;
using GalaSoft.MvvmLight.Command;

namespace dna_simulator.ViewModel.Configuration
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// </summary>
    public class ConfigViewModel : ViewModelBase
    {
        private IServiceBundle _serviceBundle;
        private IColorPickerService _colorPickerService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ConfigViewModel(IServiceBundle serviceBundle)
        {
            _serviceBundle = serviceBundle;
            _colorPickerService = _serviceBundle.ColorPickerService;

            CurrentMultiTileViewModel = new MultiTileViewModel(_serviceBundle);
            CurrentSingleTileViewModel = new SingleTileViewModel(_serviceBundle);

            // initialize commands
            ChangeGlueDisplayColorCommand = new RelayCommand<string>(ChangeGlueDisplayColor, CanChangeGlueDisplayColor);
            ConfigureEdgeCommand = new RelayCommand<GlueVm>(ConfigureEdge, CanConfigureEdge);
            ConfigureTileCommand = new RelayCommand(ConfigureTile, CanConfigureTile);
        }

        private SingleTileViewModel _currentSingleTileViewModel;
        private MultiTileViewModel _currentMultiTileViewModel;

        public SingleTileViewModel CurrentSingleTileViewModel
        {
            get { return _currentSingleTileViewModel; }
            set
            {
                if (Equals(value, _currentSingleTileViewModel)) return;
                _currentSingleTileViewModel = value;
                RaisePropertyChanged();
            }
        }

        public MultiTileViewModel CurrentMultiTileViewModel
        {
            get { return _currentMultiTileViewModel; }
            set
            {
                if (Equals(value, _currentMultiTileViewModel)) return;
                _currentMultiTileViewModel = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<string> ChangeGlueDisplayColorCommand { get; private set; }

        private bool CanChangeGlueDisplayColor(string label)
        {
            return true;
        }

        private void ChangeGlueDisplayColor(string label)
        {
            _colorPickerService.ShowColorPicker(c =>
            {
                foreach (var glue in CurrentSingleTileViewModel.CurrentTileTypeVm.TopEdges.Where(e => e.Label == label))
                {
                    glue.DisplayColor = c;
                }
                foreach (var glue in CurrentSingleTileViewModel.CurrentTileTypeVm.BottomEdges.Where(e => e.Label == label))
                {
                    glue.DisplayColor = c;
                }
                foreach (var glue in CurrentSingleTileViewModel.CurrentTileTypeVm.LeftEdges.Where(e => e.Label == label))
                {
                    glue.DisplayColor = c;
                }
                foreach (var glue in CurrentSingleTileViewModel.CurrentTileTypeVm.RightEdges.Where(e => e.Label == label))
                {
                    glue.DisplayColor = c;
                }
            });
        }

        public RelayCommand<GlueVm> ConfigureEdgeCommand { get; private set; }

        private bool CanConfigureEdge(GlueVm glue)
        {
            return true;
        }

        private void ConfigureEdge(GlueVm glue)
        {
            CurrentSingleTileViewModel.CurrentEditorModel = glue;
        }

        public RelayCommand ConfigureTileCommand { get; private set; }

        private bool CanConfigureTile()
        {
            return true;
        }

        private void ConfigureTile()
        {
            CurrentSingleTileViewModel.CurrentEditorModel = CurrentSingleTileViewModel.CurrentTileTypeVm;
        }
    }
}