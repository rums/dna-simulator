using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Interactivity;
using dna_simulator.ViewModel;

namespace dna_simulator.Behaviors
{
    public class CollectionIsEmptyBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty DefaultDataContextProperty 
            = DependencyProperty.Register("DefaultDataContext", typeof(ViewModelBase), typeof(DefaultDataContextBehavior),
            new PropertyMetadata(OnDefaultDataContextPropertyChanged));

        private static void OnDefaultDataContextPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs args)
        {
            // This gets called _after_ OnAttached!
        }

        public ViewModelBase DefaultDataContext
        {
            get { return (ViewModelBase) GetValue(DefaultDataContextProperty); }
            set { SetValue(DefaultDataContextProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject.DataContext == null)
            {
                AssociatedObject.DataContext = (new ViewModelLocator()).Introduction;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetVisibility();
        }

        private void SetVisibility()
        {
            var collection = Collection as ICollection;
            AssociatedObject.Visibility =
              collection != null && collection.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}