namespace Logix.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class AgeToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Transparent);
            if (int.TryParse(value.ToString(), out int transformed))
                return transformed < 18 ? new SolidColorBrush(Color.FromRgb(255, 0, 0)) : new SolidColorBrush(Color.FromRgb(0, 255, 0));
            throw new ArgumentException("Value isn't valid");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
