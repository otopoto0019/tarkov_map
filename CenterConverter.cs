using System.Globalization;
using System.Windows.Data;

namespace TarkovMap
{
    public class CenterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double containerSize && double.TryParse(parameter.ToString(), out double elementSize))
            {
                return (containerSize - elementSize) / 2;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}