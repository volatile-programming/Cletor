using Cletor.Views.Helpers;

namespace Cletor.Commands
{
    public class ToggleFullScreenCommand : RelayCommand
    {
        private readonly MainWindow _currentWindow;

        public ToggleFullScreenCommand(MainWindow currentWindow) : base(execute: null)
        {
            _currentWindow = currentWindow;
            _execute = ToggleFullScreen;
        }

        private void ToggleFullScreen()
        {
            ConfigurationHandler.Current.IsFullScreen =
                !ConfigurationHandler.Current.IsFullScreen;

            ConfigurationHandler.Current.ToggleFullScreen(_currentWindow);
        }
    }
}
