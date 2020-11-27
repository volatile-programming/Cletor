using Cletor.Resources;
using Cletor.Resources.Styles;
using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cletor.Views.Helpers
{
    public class SfDocumentToHtml
    {
        public string Convert(DocumentAdv document)
        {
            var htmlDocument = string.Empty;
            foreach (var section in document.Sections)
                htmlDocument += ConvertSection(section as SectionAdv);

            return htmlDocument;
        }

        private string ConvertSection(SectionAdv section)
        {
            var htmlBlocks = string.Empty;
            for (int i = 0; i < section.Blocks.Count; i++)
            {
                var block = section.Blocks[i];
                var isList = IsList(block);
                if (!(block is ParagraphAdv paragraph && isList))
                    htmlBlocks += ConvertBlock(block);
                else
                {
                    var pattern = paragraph.ParagraphFormat.ListFormat.ListLevel.ListLevelPattern;
                    var isUnorderedList = IsUnorderedList(pattern);

                    htmlBlocks += isUnorderedList ? "<ul>\n" : "<ol>\n";
                    htmlBlocks += ConvertList(paragraph, i, out i, out _);
                    htmlBlocks += isUnorderedList ? "</ul>\n" : "</ol>\n";
                }
            }

            return htmlBlocks;
        }

        private bool IsList(BlockAdv block)
        {
            if (!(block is ParagraphAdv paragraph))
                return false;
            return paragraph.ParagraphFormat.ListFormat.List != null;
        }

        private string ConvertBlock(Node block, bool isInline = false)
        {
            var htmlNode = string.Empty;
            switch (block)
            {
                case ParagraphAdv paragraph:
                    for (var i = 0; i < paragraph.Inlines.Count; i++)
                    {
                        var inline = paragraph.Inlines[i];
                        var result = ConvertInline(inline);
                        if (result.Contains("HYPERLINK \""))
                        {
                            i += 2;
                            if (!(paragraph.Inlines[i] is SpanAdv span))
                                continue;
                            result = WrapLink(result, span.Text);
                        }

                        if (!isInline)
                            result = WrapBlock(result, paragraph.ParagraphFormat);

                        htmlNode += result;
                    }
                    break;
                case TableAdv table:
                    if (QuoteFormat.IsQuote(table))
                        htmlNode = ConvertQuote(table);
                    else
                    {
                        htmlNode += "<table border=\"1\">\n";
                        foreach (var raw in table.Rows.Select((row, index) => new { row, index }))
                            htmlNode += ConvertRow(raw.row as TableRowAdv, raw.index);

                        htmlNode += "</table>\n";
                    }
                    break;
            }

            return htmlNode;
        }

        private string ConvertInline(Node inline)
        {
            var htmlInline = string.Empty;
            switch (inline)
            {
                case SpanAdv span:
                    htmlInline = WrapSpan(span);
                    break;
                case ImageContainerAdv image:
                    htmlInline = $"<img src=\"{image.ImageSource}\" alt=\"{image.ImageSource}\">";
                    break;
            }

            return htmlInline;
        }

        private string WrapLink(string result, string text)
        {
            var regex = new Regex("\".*?\"");
            result = regex.Match(result).ToString();
            result = $"\n\t<a href={result}>{text}</a>\n";

            return result;
        }

        private string WrapSpan(SpanAdv span)
        {
            var htmlSpan = span.Text;
            var format = span.CharacterFormat;
            if (format.StrikeThrough != StrikeThrough.None)
                htmlSpan = $"<del>{htmlSpan}</del>";
            if (format.Underline != Underline.None)
                htmlSpan = $"<ins>{htmlSpan}</ins>";
            if (format.Italic)
                htmlSpan = $"<i>{htmlSpan}</i>";
            if (format.Bold)
                htmlSpan = $"<b>{htmlSpan}</b>";
            return htmlSpan;
        }

        private string WrapBlock(string content, ParagraphFormat paragraphFormat)
        {
            switch (paragraphFormat.StyleName)
            {
                case Constants.NormalStyleName when !string.IsNullOrWhiteSpace(content):
                    return $"<p>{content}</p>\n";
                case Constants.Heading1StyleName when !string.IsNullOrWhiteSpace(content):
                    return $"<h1>{content}</h1>\n";
                case Constants.Heading2StyleName when !string.IsNullOrWhiteSpace(content):
                    return $"<h2>{content}</h2>\n";
                case Constants.Heading3StyleName when !string.IsNullOrWhiteSpace(content):
                    return $"<h3>{content}</h3>\n";
                case Constants.Heading4StyleName when !string.IsNullOrWhiteSpace(content):
                    return $"<h4>{content}</h4>\n";
                case Constants.Heading5StyleName when !string.IsNullOrWhiteSpace(content):
                    return $"<h5>{content}</h5>\n";
                case Constants.Heading6StyleName when !string.IsNullOrWhiteSpace(content):
                    return $"<h6>{content}</h6>\n";
                default:
                    return content;
            }
        }

        private string ConvertQuote(TableAdv quote)
        {
            var row = quote.Rows[0];
            var cell = row.Cells[0];
            if (cell.Blocks.Count < 2)
                return "";

            if (!(cell.Blocks[1] is ParagraphAdv citeParagraph) ||
                !citeParagraph.Inlines.Any())
                return "";

            var cite = citeParagraph.Inlines[0] as SpanAdv;
            var result = $"<blockquote cite=\"{cite.Text}\">\n";

            var content = cell.Blocks[0];
            result += '\t' + ConvertBlock(content);
            result += '\t' + ConvertBlock(citeParagraph);

            result += "</blockquote>\n";

            return result;
        }

        private string ConvertRow(TableRowAdv row, int index)
        {
            var htmlBlocks = (index == 0) ? "\t<thead>\n\t\t<tr>\n" : "\t<tbody>\n\t\t<tr>\n";
            foreach (var node in row.Cells)
            {
                if (!(node is TableCellAdv cell))
                    continue;
                foreach (var block in cell.Blocks)
                {
                    var result = ConvertBlock(block, true);
                    result = WrapRow(result, index);
                    htmlBlocks += result;
                }
            }
            htmlBlocks += (index == 0) ? "\t\t</tr>\n\t</thead>\n" : "\t\t</tr>\n\t</tbody>\n";

            return htmlBlocks;
        }

        private string WrapRow(string content, int index)
        {
            if (index == 0)
                return $"\t\t\t<th>{content}</th>\n";

            return $"\t\t\t<td>{content}</td>\n";
        }

        private string ConvertList(ParagraphAdv paragraph,
            int startIndex,
            out int lastIndex,
            out ParagraphAdv nexNode)
        {
            nexNode = null;
            var htmlLevels = string.Empty;
            var isNextNodeChild = false;
            var isNextNodeSibling = true;
            var isNextNodeList = true;
            var currentIndex = startIndex;

            var format = paragraph.ParagraphFormat;
            var pattern = format.ListFormat.ListLevel.ListLevelPattern;
            var level = format.ListFormat.ListLevel.LevelNumber;
            var indent = string.Empty;
            for (int i = -1; i < level; i++)
                indent += '\t';

            while (paragraph.NextNode is ParagraphAdv block &&
                   isNextNodeList &&
                   !isNextNodeChild &&
                   isNextNodeSibling)
            {
                foreach (var inline in paragraph.Inlines)
                    htmlLevels += $"{indent}<li>{ConvertInline(inline)}</li>\n";

                nexNode = block;
                isNextNodeChild = IsNextNodeChild(level, nexNode);
                isNextNodeList = IsList(nexNode);
                currentIndex++;

                if (isNextNodeChild && isNextNodeList)
                {
                    var isUnorderedList = IsUnorderedList(pattern);
                    htmlLevels += isUnorderedList ? $"{indent}<ul>" : $"{indent}<ol>";
                    htmlLevels += ConvertList(nexNode, currentIndex, out currentIndex, out nexNode);
                    htmlLevels += isUnorderedList ? $"{indent}</ul>\n" : $"{indent}</ol>\n";
                }

                paragraph = nexNode;
                isNextNodeChild = IsNextNodeChild(level, paragraph);
                isNextNodeSibling = IsNextNodeSibling(level, paragraph);
            }

            lastIndex = currentIndex;

            return htmlLevels;
        }

        private bool IsNextNodeSibling(int level, ParagraphAdv paragraph)
        {
            if (!IsList(paragraph))
                return false;

            var format = paragraph.ParagraphFormat;
            var subLevel = format.ListFormat.ListLevel.LevelNumber;
            var result = level == subLevel;

            return result;
        }

        private bool IsNextNodeChild(int level, ParagraphAdv paragraph)
        {
            if (!IsList(paragraph))
                return false;

            var format = paragraph.ParagraphFormat;
            var subLevel = format.ListFormat.ListLevel.LevelNumber;
            var result = level < subLevel;

            return result;
        }

        private bool IsUnorderedList(ListLevelPattern levelPattern) =>
            levelPattern == (ListLevelPattern.Bullet |
                             ListLevelPattern.FarEast |
                             ListLevelPattern.Special |
                             ListLevelPattern.None);
    }
}
