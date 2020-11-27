using Cletor.Resources.Languages;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cletor.Commands.MenuCommands
{
    public class ExitCommand : MenuItem
    {
        public ExitCommand(MainWindow window)
        {
            ToolTip = UIText.CommandToolTipExit;
            var gestures = new InputGestureCollection
            {
                new KeyGesture(Key.F4, ModifierKeys.Alt)
            };
            Command = new RoutedUICommand(UIText.CommandHeaderExit, "Command", typeof(ExitCommand), gestures);
            CommandTarget = window;
            window.CommandBindings.Add(new CommandBinding(Command, Execute, CanExecute));
        }

        private void Execute(object sender, ExecutedRoutedEventArgs e) =>
            System.Windows.Application.Current.Shutdown(0);

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = true;
    }

}
