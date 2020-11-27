using HtmlAgilityPack;
using ReverseMarkdown;
using ReverseMarkdown.Converters;

namespace Cletor.Views.Helpers
{
    public class StrikeThroughConverter : ConverterBase
    {
        public StrikeThroughConverter(Converter converter) : base(converter) { }

        public override string Convert(HtmlNode node)
        {
            return $"~~{node.InnerHtml}~~ ";
        }
    }

}
