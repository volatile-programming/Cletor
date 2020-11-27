using Cletor.Resources;
using Cletor.Resources.Languages;
using Cletor.Views.Helpers;
using Microsoft.Win32;
using ReverseMarkdown;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cletor.Commands.MenuCommands
{
    public class SaveCommand : MenuItem
    {
        private readonly MainWindow _window;
        private bool _completed;

        public SaveCommand(MainWindow window)
        {
            _completed = false;
            _window = window;
            Header = UIText.CommandHeaderSave;
            ToolTip = UIText.CommandToolTipSave;
            var gestures = new InputGestureCollection
            {
                new KeyGesture(Key.S, ModifierKeys.Control)
            };
            Command = new RoutedUICommand(UIText.CommandHeaderSave, "Command", typeof(SaveCommand), gestures);
            CommandTarget = window;
            window.CommandBindings.Add(new CommandBinding(Command, Execute, CanExecute));
        }

        private void Execute(object sender, ExecutedRoutedEventArgs e)
        {
            _completed = false;
            var fileName = AskUserToChoseAFile();

            if (string.IsNullOrWhiteSpace(fileName))
                return;

            _window.ShowProcessMessage(UIText.ProcessMessageSaving);

            var fileExtension = Path.GetExtension(fileName);

            if (!(fileExtension.Contains(Constants.DefaultFileExtension) ||
                fileExtension.Contains(Constants.HtmFileExtension)))
                _window.TextEditor.Save(fileName);
            else
            {
                string file = _window.TextEditor.GetTempFile();

                if (fileExtension.Contains(Constants.DefaultFileExtension))
                    file = ConvertHtmlToMarkdown(file);

                File.WriteAllText(fileName, file);
            }

            _window.HideProcessMessage();
            _completed = true;
        }

        private string AskUserToChoseAFile()
        {
            var saveFileDialog = new SaveFileDialog()
            {
                FilterIndex = 0,
                DefaultExt = Constants.DefaultFileExtension,
                AddExtension = true,
                ValidateNames = true,
                OverwritePrompt = true,
                CheckPathExists = true,
                RestoreDirectory = true,
                Filter = UIText.AllSupportedFilesSave,
            };

            var fileName = (saveFileDialog.ShowDialog() == true) ? saveFileDialog.FileName : null;

            return fileName;
        }

        private string ConvertHtmlToMarkdown(string html)
        {
            var configuration = new Config
            {
                UnknownTags = Config.UnknownTagsOption.Raise,
                GithubFlavored = true,
                RemoveComments = true,
                SmartHrefHandling = true
            };

            var converter = new Converter(configuration);
            converter.Register("ins", new UnderlineConverter(converter));
            converter.Register("del", new StrikeThroughConverter(converter));

            var markdown = converter.Convert(html);

            return markdown;
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = true;

        public bool Save()
        {
            Execute(this, null);
            return _completed;
        }
    }
}
