using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Cletor.Views.Converters
{
    public class StringToNavigationUriConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toConvert = (string)value;

            var converted = new Uri(toConvert);

            return converted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toConvert = (Uri)value;

            var converted = toConvert.AbsoluteUri;

            return converted;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
