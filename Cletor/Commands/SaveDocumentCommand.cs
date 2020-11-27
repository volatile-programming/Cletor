using Cletor.Commands.MenuCommands;

namespace Cletor.Commands
{
    public class SaveDocumentCommand : RelayCommand
    {
        private readonly SaveCommand _saveCommand;

        public SaveDocumentCommand(SaveCommand saveCommand) : base(execute: null)
        {
            _saveCommand = saveCommand;
            _execute = SaveDocument;
        }

        private void SaveDocument() =>
            _saveCommand.Save();
    }
}
