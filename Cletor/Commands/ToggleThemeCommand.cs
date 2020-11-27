using Cletor.Resources.Enums;
using Cletor.Views.Helpers;

namespace Cletor.Commands
{
    public class ToggleThemeCommand : RelayCommand
    {
        private readonly MainWindow _currentWindow;

        public ToggleThemeCommand(MainWindow currentWindow) : base(execute: null)
        {
            _currentWindow = currentWindow;
            _execute = ToggleTheme;
        }

        private void ToggleTheme()
        {
            var theme = (ConfigurationHandler.Current.Theme == Themes.Light) ? Themes.Dark : Themes.Light;

            ConfigurationHandler.Current.ChangeTheme(_currentWindow, theme);
        }
    }
}
