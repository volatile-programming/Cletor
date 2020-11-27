using Cletor.Models;
using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System.Windows;

namespace Cletor.Resources.Styles
{
    public class QuoteFormat : TableAdv
    {
        public string Cite { get; set; }

        public static bool IsQuote(TableAdv table)
        {
            if (table.Rows.Count != 1)
                return false;

            var row = table.Rows[0];
            if (row.Cells.Count != 1)
                return false;

            var cell = row.Cells[0];
            var border = cell.CellFormat.Borders.Left;
            if (border == null || !border.LineWidth.Equals(5d))
                return false;

            return true;
        }

        public static QuoteFormat Create(Quote quote)
        {
            var content = CreateSpan(quote.Content);

            var paragraph = new ParagraphAdv
            {
                ParagraphFormat = new ParagraphFormat
                {
                    StyleName = Constants.NormalStyleName,
                    LeftIndent = 25d,
                    BeforeSpacing = 15d,
                    AfterSpacing = 15d
                }
            };
            paragraph.Inlines.Add(content);

            var cite = CreateSpan(quote.Cite);

            var citeParagraph = new ParagraphAdv
            {
                ParagraphFormat = new ParagraphFormat
                {
                    StyleName = Constants.NormalStyleName,
                    BeforeSpacing = 15d,
                    AfterSpacing = 15d,
                    TextAlignment = TextAlignment.Right
                }
            };
            citeParagraph.Inlines.Add(cite);

            if (!(Application.Current.MainWindow is MainWindow window))
                return null;

            var gray3 = window.FindResource(Constants.MahAppsColorsGray3);
            var gray9 = window.FindResource(Constants.MahAppsColorsGray9);
            var tableCell = new TableCellAdv
            {
                CellFormat = new CellFormat
                {
                    Borders = new Borders
                    {
                        Left = new Border()
                        {
                            LineStyle = LineStyle.Single,
                            LineWidth = 5d,
                            Color = (System.Windows.Media.Color)gray3
                        }
                    },
                    CellWidth = 640,
                    Shading = new Shading
                    {
                        BackgroundColor = (System.Windows.Media.Color)gray9
                    }
                }
            };
            tableCell.Blocks.Add(paragraph);
            tableCell.Blocks.Add(citeParagraph);

            var tableRow = new TableRowAdv();
            tableRow.Cells.Add(tableCell);

            var table = new QuoteFormat
            {
                Cite = quote.Cite
            };
            table.Rows.Add(tableRow);

            return table;
        }

        private static SpanAdv CreateSpan(string text) =>
            new SpanAdv
            {
                CharacterFormat = new CharacterFormat
                {
                    Italic = true
                },
                Text = text
            };
    }
}
