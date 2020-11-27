namespace Cletor.Commands
{
    public class InsertCheckListCommand : RelayCommand
    {
        public InsertCheckListCommand() : base(execute: null)
        {
            _execute = InsertCheckList;
        }

        private void InsertCheckList()
        {

        }
    }
}
