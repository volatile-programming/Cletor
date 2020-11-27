using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System.Linq;

namespace Cletor.Views.Helpers
{
    public static class SfRichTextBoxAdvExtensions
    {
        public static SectionAdv GetCurrentSection(this SfRichTextBoxAdv editor)
        {
            var hierarchicalIndex = editor.Selection.Start.HierarchicalIndex;

            var index = hierarchicalIndex
                .Split(';')
                .Select(int.Parse)
                .ToList();

            var sectionIndex = index[0];
            var currentSection = editor.Document.Sections[sectionIndex];

            return currentSection;
        }

        public static Node GetCurrentBlock(this SfRichTextBoxAdv editor)
        {
            var hierarchicalIndex = editor.Selection.Start.HierarchicalIndex;

            var index = hierarchicalIndex
                .Split(';')
                .Select(int.Parse)
                .ToList();

            var sectionIndex = index[0];
            var blockIndex = index[1];

            var currentSection = editor.Document.Sections[sectionIndex];
            var currentBlock = currentSection.Blocks[blockIndex];

            return currentBlock;
        }

        public static void InsertBlock(this SfRichTextBoxAdv editor, Node table)
        {
            var hierarchicalIndex = editor.Selection.Start.HierarchicalIndex;

            var indexes = hierarchicalIndex
                .Split(';')
                .Select(int.Parse)
                .ToList();

            var sectionIndex = indexes[0];
            var blockIndex = indexes[1];

            var currentSection = editor.Document.Sections[sectionIndex];
            currentSection.Blocks.Insert(blockIndex, table);
        }
    }
}
