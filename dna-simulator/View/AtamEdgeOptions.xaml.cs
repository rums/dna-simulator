using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using dna_simulator.ViewModel.Atam;

namespace dna_simulator.View
{
    public partial class AtamEdgeOptions
    {
        public AtamEdgeOptions()
        {
            // Required to initialize variables
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var glue = DataContext as GlueVm;
            if (glue != null) glue.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var glue = DataContext as GlueVm;
            if (glue != null) OnGluePropertyChanged(glue);
        }

        public static readonly DependencyProperty GlueProperty = DependencyProperty.Register("Glue",
            typeof (GlueVm),
            typeof (AtamEdgeOptions),
            new PropertyMetadata(new GlueVm { Color = 0, DisplayColor = Colors.Red, Strength = 0}, OnGluePropertyChanged));

        private static void OnGluePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {  
            var atamEdgeOptions = dependencyObject as AtamEdgeOptions;
            if (atamEdgeOptions == null) return;
            atamEdgeOptions.OnGluePropertyChanged((GlueVm)e.NewValue);
        }

        private void OnGluePropertyChanged(GlueVm newValue)
        {
            EdgeNameCaption.Text = newValue.Label;
            ColorInput.Text = newValue.Color.ToString(CultureInfo.CurrentCulture);
            StrengthInput.Text = newValue.Strength.ToString(CultureInfo.CurrentCulture);
            GlueDisplayColor.Fill = new SolidColorBrush(newValue.DisplayColor);
        }

        public GlueVm Glue
        {
            get { return (GlueVm)GetValue(GlueProperty); }
            set { SetValue(GlueProperty, value); }
        }
    }
}