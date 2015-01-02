/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:dna_simulator.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using dna_simulator.Model;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace dna_simulator.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<ColorPickerViewModel>();
            SimpleIoc.Default.Register<SingleTileViewModel>();
            SimpleIoc.Default.Register<MultiTileViewModel>();
        }

        /// <summary>
        /// Gets the ColorPickerViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ColorPickerViewModel ColorPickerViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ColorPickerViewModel>();
            }
        }

        /// <summary>
        /// Gets the SingleTileViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SingleTileViewModel SingleTileViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SingleTileViewModel>();
            }
        }

        /// <summary>
        /// Gets the MultiTileViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MultiTileViewModel MultiTileViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MultiTileViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}