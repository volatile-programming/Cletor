using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Cletor.Views.Converters
{
    public class StrikeThroughCommandConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is true ? StrikeThrough.None : StrikeThrough.SingleStrike;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value;

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
