using Cletor.Commands;
using Cletor.Commands.MenuCommands;
using Cletor.Models;
using Cletor.Views.Helpers;
using MahApps.Metro.IconPacks;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Cletor.Presenters
{
    public class EditorPresenters : BasePresenter
    {
        public EditorPresenters(MainWindow window)
        {
            CurrentWindow = window;
            InitializeComponents();
        }

        #region Properties

        public MainWindow CurrentWindow { get; }
        public List<RemoteImage> Images { get; private set; }
        public RelayCommand InsertRemotePicture { get; private set; }
        public RelayCommand InsertQuote { get; private set; }
        public RelayCommand InsertCheckList { get; private set; }
        public RelayCommand ImportMarkDown { get; private set; }
        public RelayCommand ExportToMarkDown { get; private set; }
        public RelayCommand OpenOptions { get; private set; }
        public RelayCommand ToggleTheme { get; private set; }
        public RelayCommand ToggleFullScreen { get; private set; }
        public RelayCommand Save { get; private set; }
        public RelayCommand Open { get; private set; }
        public List<MenuItem> MainMenu { get; private set; }
        public PackIconModernKind PinIcon { get; set; }
        public ConfigurationHandler Options => ConfigurationHandler.Current;
        public OpenCommand OpenCommand { get; private set; }
        public SaveCommand SaveCommand { get; private set; }

        #endregion

        #region Initialize Components

        private void InitializeComponents()
        {
            Images = new List<RemoteImage>();

            InsertRemotePicture = new InsertRemotePictureCommand(CurrentWindow, Images);
            InsertQuote = new InsertQuoteCommand(CurrentWindow);
            InsertCheckList = new InsertCheckListCommand();
            ImportMarkDown = new InportMarkDownCommand();
            ExportToMarkDown = new ExportToMarkDownCommand();
            OpenOptions = new OpenOptionsCommand(CurrentWindow);
            ToggleTheme = new ToggleThemeCommand(CurrentWindow);
            ToggleFullScreen = new ToggleFullScreenCommand(CurrentWindow);

            OpenCommand = new OpenCommand(CurrentWindow);
            Open = new OpenDocumentCommand(OpenCommand);
            SaveCommand = new SaveCommand(CurrentWindow);
            Save = new SaveDocumentCommand(SaveCommand);

            MainMenu = new List<MenuItem>
            {
                new NewCommand(CurrentWindow.TextEditor),
                OpenCommand,
                SaveCommand,
                new EmptyCommand(),
                new PrintCommand(CurrentWindow.TextEditor),
                new EmptyCommand(),
                new ShortcutsCommand(CurrentWindow),
                new AboutCommand(CurrentWindow),
                new ExitCommand(CurrentWindow)
            };

            PinIcon = ConfigurationHandler.Current.IsToolbarFixed ?
                PackIconModernKind.ArrowUp :
                PackIconModernKind.ArrowDown;
        }

        #endregion
    }
}
