namespace Cletor.Commands
{
    public class OpenOptionsCommand : RelayCommand
    {
        private readonly MainWindow _currentWindow;

        public OpenOptionsCommand(MainWindow currentWindow) : base(execute: null)
        {
            _currentWindow = currentWindow;
            _execute = OpenOptions;
        }

        private void OpenOptions() =>
            _currentWindow.OptionsFlyout.IsOpen = !_currentWindow.OptionsFlyout.IsOpen;
    }
}
