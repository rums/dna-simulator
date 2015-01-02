using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
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
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var glue = DataContext as GlueVm;
            if (glue != null) glue.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var glue = DataContext as GlueVm;
            if (glue == null)
            StrengthInput.Text = glue.Strength.ToString(CultureInfo.CurrentCulture);
            GlueDisplayColor.Fill = new SolidColorBrush(glue.DisplayColor);
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
            StrengthInput.Text = newValue.Strength.ToString(CultureInfo.CurrentCulture);
            GlueDisplayColor.Fill = new SolidColorBrush(newValue.DisplayColor);
        }

        public GlueVm Glue
        {
            get { return (GlueVm)GetValue(GlueProperty); }
            set { SetValue(GlueProperty, value); }
        }

        public static readonly DependencyProperty EdgeNameProperty = DependencyProperty.Register("EdgeName",
            typeof(string),
            typeof(AtamEdgeOptions),
            new PropertyMetadata(string.Empty, OnEdgeNamePropertyChanged));

        private static void OnEdgeNamePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var atamEdgeOptions = dependencyObject as AtamEdgeOptions;
            if (atamEdgeOptions == null) return;
            atamEdgeOptions.OnEdgeNamePropertyChanged((string)e.NewValue);
        }

        private void OnEdgeNamePropertyChanged(string newValue)
        {
            EdgeNameTextBlock.Text = newValue;
        }

        public string EdgeName
        {
            get { return GetValue(EdgeNameProperty).ToString(); }
            set { SetValue(EdgeNameProperty, value); }
        }
    }
}