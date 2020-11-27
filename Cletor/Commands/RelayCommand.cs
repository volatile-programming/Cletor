using System;
using System.Windows.Input;

namespace Cletor.Commands
{
    public class RelayCommand : ICommand
    {
        protected Action _execute;
        protected Action<object> _executeParam;
        protected Func<bool> _canExecute;

        /// <summary>
        /// Raised when RaiseCanExecuteChanged is called.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute) : this(execute, null) { }
        public RelayCommand(Action<object> executeParam) : this(executeParam, null) { }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public RelayCommand(Action<object> executeParam, Func<bool> canExecute)
        {
            _executeParam = executeParam;
            _canExecute = canExecute;
        }

        // Specify the keys and mouse actions that invoke the command.
        //public Key GestureKey { get; set; }
        //public ModifierKeys GestureModifier { get; set; }
        //public MouseAction MouseGesture { get; set; }

        /// <summary>
        /// Determines whether this RelayCommand can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        /// <summary>
        /// Executes the RelayCommand on the current command target.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _execute?.Invoke();
            _executeParam?.Invoke(parameter);
        }

        /// <summary>
        /// Method used to raise the CanExecuteChanged event
        /// to indicate that the return value of the CanExecute
        /// method has changed.
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
