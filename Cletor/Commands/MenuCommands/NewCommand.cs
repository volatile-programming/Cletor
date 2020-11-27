using Cletor.Resources.Languages;
using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cletor.Commands.MenuCommands
{
    public class NewCommand : MenuItem
    {
        public NewCommand(SfRichTextBoxAdv editor)
        {
            Header = UIText.CommandHeaderNew;
            ToolTip = UIText.CommandToolTipNew;
            var command = SfRichTextBoxAdv.NewDocumentCommand;
            command.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            Command = command;
            CommandTarget = editor;
        }
    }

}
