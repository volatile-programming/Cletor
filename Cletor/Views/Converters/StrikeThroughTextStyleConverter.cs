using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Cletor.Views.Converters
{
    public class StrikeThroughTextStyleConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is StrikeThrough strikeFormat && strikeFormat != StrikeThrough.None;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is true ? StrikeThrough.SingleStrike : StrikeThrough.None;

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
