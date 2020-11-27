using Cletor.Commands.MenuCommands;

namespace Cletor.Commands
{
    public class OpenDocumentCommand : RelayCommand
    {
        private readonly OpenCommand _openCommand;

        public OpenDocumentCommand(OpenCommand openCommand) : base(execute: null)
        {
            _openCommand = openCommand;
            _execute = OpenDocument;
        }

        private void OpenDocument() =>
            _openCommand.Open();
    }
}
