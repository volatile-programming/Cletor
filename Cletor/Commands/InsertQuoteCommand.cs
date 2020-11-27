using Cletor.Models;
using Cletor.Resources.Styles;
using Cletor.Views.Controls;
using Cletor.Views.Helpers;

namespace Cletor.Commands
{
    public class InsertQuoteCommand : RelayCommand
    {
        private readonly MainWindow _currentWindow;

        public InsertQuoteCommand(MainWindow currentWindow) : base(execute: null)
        {
            _currentWindow = currentWindow;
            _execute = InsertQuote;
        }

        private async void InsertQuote()
        {
            var quote = await CustomDialog.Show<Quote>(_currentWindow, "Insert quote");
            if (quote is null)
                return;

            var table = QuoteFormat.Create(quote);

            _currentWindow.TextEditor.InsertBlock(table);
        }
    }
}
