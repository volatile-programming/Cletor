using Cletor.Resources.Languages;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cletor.Commands.MenuCommands
{
    public class ShortcutsCommand : MenuItem
    {
        private readonly MainWindow _window;

        public ShortcutsCommand(MainWindow window)
        {
            _window = window;
            ToolTip = UIText.CommandToolTipShortcuts;
            var gestures = new InputGestureCollection
            {
                new KeyGesture(Key.F1)
            };
            Command = new RoutedUICommand(UIText.CommandHeaderShortcuts, "Command", typeof(ExitCommand), gestures);
            CommandTarget = window;
            window.CommandBindings.Add(new CommandBinding(Command, Execute, CanExecute));
        }

        private void Execute(object sender, ExecutedRoutedEventArgs e) =>
            _window.ShortcutsFlyout.IsOpen = !_window.ShortcutsFlyout.IsOpen;

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = true;
    }

}
