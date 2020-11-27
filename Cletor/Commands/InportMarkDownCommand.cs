namespace Cletor.Commands
{
    public class InportMarkDownCommand : RelayCommand
    {
        public InportMarkDownCommand() : base(execute: null)
        {
            _execute = InportMarkDown;
        }

        private void InportMarkDown()
        {

        }
    }
}
