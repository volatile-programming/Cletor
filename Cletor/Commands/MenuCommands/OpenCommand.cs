using Cletor.Resources;
using Cletor.Resources.Languages;
using Cletor.Views.Helpers;
using Markdig;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cletor.Commands.MenuCommands
{
    public class OpenCommand : MenuItem
    {
        private readonly MainWindow _window;

        public OpenCommand(MainWindow window)
        {
            _window = window;
            Header = UIText.CommandHeaderOpen;
            ToolTip = UIText.CommandToolTipOpen;
            var gestures = new InputGestureCollection
            {
                new KeyGesture(Key.O, ModifierKeys.Alt)
            };
            Command = new RoutedUICommand(UIText.CommandHeaderSave, "Command", typeof(OpenCommand), gestures);
            CommandTarget = window;
            window.CommandBindings.Add(new CommandBinding(Command, Execute, CanExecute));
        }

        private void Execute(object sender, ExecutedRoutedEventArgs e)
        {
            var fileName = AskUserToChoseAFile();

            if (string.IsNullOrWhiteSpace(fileName))
                return;

            _window.ShowProcessMessage(UIText.ProcessMessageLoading);

            var fileExtension = Path.GetExtension(fileName);

            if (!fileExtension.Contains(Constants.DefaultFileExtension))
                _window.TextEditor.Load(fileName);
            else
            {
                var file = File.ReadAllText(fileName);

                file = ConvertMarkdownToHtml(file);

                var tempFilePath = ConfigurationHandler.Current.TemporalFilesPath + "tempConversionFile.html";
                File.WriteAllText(tempFilePath, file);

                _window.TextEditor.Load(tempFilePath);
            }

            _window.HideProcessMessage();

        }

        private string AskUserToChoseAFile()
        {
            var saveFileDialog = new OpenFileDialog()
            {
                FilterIndex = 0,
                DefaultExt = Constants.DefaultFileExtension,
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true,
                AddExtension = true,
                Filter = UIText.AllSupportedFiles,
            };

            var fileName = (saveFileDialog.ShowDialog() == true) ? saveFileDialog.FileName : null;

            return fileName;
        }

        private string ConvertMarkdownToHtml(string html)
        {
            var markdown = Markdown.ToHtml(html);

            return markdown;
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = true;

        public void Open() =>
            Execute(this, null);
    }
}
