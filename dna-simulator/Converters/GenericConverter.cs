using System;
using System.Globalization;
using System.Windows.Data;

namespace dna_simulator.Converters
{
    public class GenericConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new object();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new object();
        }
    }
}
