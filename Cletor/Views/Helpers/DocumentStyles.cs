using Cletor.Resources;
using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System.Windows.Media;

namespace Cletor.Views.Helpers
{
    public static class DocumentStyles
    {
        public static void Update(StyleCollection styles)
        {
            foreach (DocumentStyle style in styles)
            {
                switch (style.Name)
                {
                    case Constants.NormalStyleName:
                        ApplyNormalStyle(style as ParagraphStyle);
                        break;
                    case Constants.Heading1StyleName:
                        ApplyHeading1Style(style as ParagraphStyle);
                        break;
                    case Constants.Heading2StyleName:
                        ApplyHeading2Style(style as ParagraphStyle);
                        break;
                    case Constants.Heading3StyleName:
                        ApplyHeading3Style(style as ParagraphStyle);
                        break;
                    case Constants.Heading4StyleName:
                        ApplyHeading4Style(style as ParagraphStyle);
                        break;
                    case Constants.Heading5StyleName:
                        ApplyHeading5Style(style as ParagraphStyle);
                        break;
                    case Constants.Heading6StyleName:
                        ApplyHeading6Style(style as ParagraphStyle);
                        break;
                    case Constants.HyperlinkStyleName:
                        ApplyHyperlinkStyle(style as CharacterStyle);
                        break;
                }
            }
        }

        private static void ApplyNormalStyle(ParagraphStyle style) =>
            style.CharacterFormat.FontSize = 20d;

        private static void ApplyHeading1Style(ParagraphStyle style)
        {
            style.CharacterFormat.FontSize = 30d;
            style.CharacterFormat.FontColor = Colors.DeepSkyBlue;
            style.CharacterFormat.Underline = Underline.Single;
        }

        private static void ApplyHeading2Style(ParagraphStyle style)
        {
            style.CharacterFormat.FontSize = 26d;
            style.CharacterFormat.FontColor = Colors.DodgerBlue;
        }

        private static void ApplyHeading3Style(ParagraphStyle style)
        {
            style.CharacterFormat.FontSize = 22d;
            style.CharacterFormat.FontColor = Colors.DodgerBlue;
        }

        private static void ApplyHeading4Style(ParagraphStyle style)
        {
            style.CharacterFormat.FontSize = 20d;
            style.CharacterFormat.FontColor = Colors.DodgerBlue;
        }

        private static void ApplyHeading5Style(ParagraphStyle style) =>
            style.CharacterFormat.FontSize = 18d;

        private static void ApplyHeading6Style(ParagraphStyle style) =>
            style.CharacterFormat.FontSize = 16d;

        private static void ApplyHyperlinkStyle(CharacterStyle style) =>
            style.CharacterFormat.FontSize = 20d;
    }
}
