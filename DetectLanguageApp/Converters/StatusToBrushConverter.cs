using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DetectLanguageApp.Converters
{
    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as string;
            if (string.IsNullOrEmpty(status)) return Brushes.Gray;
            return status.ToUpperInvariant() switch
            {
                "ACTIVE" => Brushes.Green,
                "SUSPENDED" => Brushes.Red,
                _ => Brushes.Gray,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
