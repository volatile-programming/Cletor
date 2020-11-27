using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Cletor.Views.Converters
{
    public class UnderlineTextStyleConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is Underline underlineFormat && underlineFormat != Underline.None;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is SfRichTextBoxAdv editor))
                return null;

            if (value is true)
            {
                var text = editor.Selection.Text;
                var isSingle = !string.IsNullOrWhiteSpace(text);
                var isWords = isSingle && text.Contains(' ');

                if (isWords)
                    return Underline.Words;
                else if (isSingle)
                    return Underline.Single;
                else
                    return Underline.None;
            }
            else
                return Underline.None;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
