namespace Cletor.Commands
{
    public class ExportToMarkDownCommand : RelayCommand
    {
        public ExportToMarkDownCommand() : base(execute: null)
        {
            _execute = ExportToMarkDown;
        }

        private void ExportToMarkDown()
        {

        }
    }
}
