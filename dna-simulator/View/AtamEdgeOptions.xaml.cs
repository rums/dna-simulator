using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using dna_simulator.Annotations;
using dna_simulator.ViewModel.Atam;

namespace dna_simulator.View
{
    public partial class AtamEdgeOptions : INotifyPropertyChanged
    {
        public AtamEdgeOptions()
        {
            // Required to initialize variables
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public static readonly DependencyProperty GlueProperty = DependencyProperty.Register("Glue",
            typeof (GlueVm),
            typeof (AtamEdgeOptions),
            new PropertyMetadata(new GlueVm { Color = 0, DisplayColor = Colors.Red, Strength = 0}, OnGluePropertyChanged));

        private static void OnGluePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {  
            var atamEdgeOptions = dependencyObject as AtamEdgeOptions;
            if (atamEdgeOptions == null) return;
            atamEdgeOptions.OnPropertyChanged("Glue");
            atamEdgeOptions.OnGluePropertyChanged((GlueVm)e.OldValue, (GlueVm)e.NewValue);
        }

        private void OnGluePropertyChanged(GlueVm oldValue, GlueVm newValue)
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
            atamEdgeOptions.OnPropertyChanged("EdgeName");
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