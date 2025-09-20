using System;
using System.Globalization;
using System.Windows.Data;

namespace Donateurs.Converters
{
    public class IllegalContributionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isIllegal)
            {
                if (isIllegal)
                {
                    return Donateurs.Properties.translation.Illegal;

                }
                return Donateurs.Properties.translation.Legal;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
