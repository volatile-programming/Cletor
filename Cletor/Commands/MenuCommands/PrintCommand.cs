using Cletor.Resources.Languages;
using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cletor.Commands.MenuCommands
{
    public class PrintCommand : MenuItem
    {
        public PrintCommand(SfRichTextBoxAdv editor)
        {
            Header = UIText.CommandHeaderPrint;
            ToolTip = UIText.CommandToolTipPrint;
            var command = SfRichTextBoxAdv.PrintDocumentCommand;
            command.InputGestures.Add(new KeyGesture(Key.P, ModifierKeys.Control));
            Command = command;
            CommandTarget = editor;
        }
    }

}
