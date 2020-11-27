using HtmlAgilityPack;
using ReverseMarkdown;
using ReverseMarkdown.Converters;

namespace Cletor.Views.Helpers
{
    public class UnderlineConverter : ConverterBase
    {
        public UnderlineConverter(Converter converter) : base(converter) { }

        public override string Convert(HtmlNode node)
        {
            return $"__{node.InnerHtml}__ ";
        }
    }

}
