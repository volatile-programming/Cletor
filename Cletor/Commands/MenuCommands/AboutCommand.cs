using Cletor.Resources.Languages;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cletor.Commands.MenuCommands
{
    public class AboutCommand : MenuItem
    {
        private readonly MainWindow _window;

        public AboutCommand(MainWindow window)
        {
            _window = window;
            ToolTip = UIText.CommandToolTipAbout;
            var gestures = new InputGestureCollection
            {
                new KeyGesture(Key.F8)
            };
            Command = new RoutedUICommand(UIText.CommandHeaderAbout, "Command", typeof(ExitCommand), gestures);
            CommandTarget = window;
            window.CommandBindings.Add(new CommandBinding(Command, Execute, CanExecute));
        }

        private void Execute(object sender, ExecutedRoutedEventArgs e) =>
            _window.AboutFlyout.IsOpen = !_window.AboutFlyout.IsOpen;

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = true;
    }

}
