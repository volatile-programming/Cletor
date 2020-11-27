using Cletor.Presenters;
using Cletor.Resources;
using Cletor.Resources.Enums;
using Cletor.Resources.Languages;
using Cletor.Views.Controls;
using Cletor.Views.Helpers;
using MahApps.Metro.Controls.Dialogs;
using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using RequestNavigateEventArgs = System.Windows.Navigation.RequestNavigateEventArgs;
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;

namespace Cletor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += LoadCacheDataAndConfiguration;
        }

        public EditorPresenters Presenter => DataContext as EditorPresenters;

        private void LoadCacheDataAndConfiguration(object sender, RoutedEventArgs e)
        {
            // Clears the (Ctrl + Z) and the (Ctrl + O) gestures.
            for (int i = 0, b = 0; i < TextEditor.CommandBindings.Count && b < 2; i++)
            {
                var binding = TextEditor.CommandBindings[i];
                var isOpenCommand = binding.Command.Equals(SfRichTextBoxAdv.OpenDocumentCommand);
                var isSaveCommand = binding.Command.Equals(SfRichTextBoxAdv.SaveDocumentCommand);

                if (isOpenCommand || isSaveCommand)
                {
                    TextEditor.CommandBindings.Remove(binding);
                    b++; --i;
                }
            }

            ShowProcessMessage(UIText.ProcessMessageLoading);

            Title = string.Format(Constants.WindowTitleTemplate, UIText.DefaultDocumentTitle);

            ConfigurationHandler.Current.ApplyConfiguration(this);

            DataContext = new EditorPresenters(this);

            HideProcessMessage();
        }

        #region Process Message

        public void ShowProcessMessage(string processMessage)
        {
            StatusMessage.Text = processMessage;
            ProcessMessage.Visibility = Locker.Visibility = Visibility.Visible;
        }

        public void HideProcessMessage() =>
            ProcessMessage.Visibility = Locker.Visibility = Visibility.Collapsed;

        #endregion

        #region Toolbar Events

        private void OnToggleToolBarVisibility(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ConfigurationHandler.Current.IsToolbarFixed)
                return;

            var isMouseOver = IsMouseOverToolbar();
            if (isMouseOver && IsToolbarVisible())
                return;

            AnimateToolbar(isMouseOver);
        }

        public bool IsToolbarVisible() =>
            ToolsBar.Margin.Equals(new Thickness());

        public bool IsMouseOverToolbar()
        {
            var point = Mouse.GetPosition(MainContainer);
            var isMouseOver = (point.X < ToolsBar.ActualWidth && point.X > 0) &&
                              (point.Y < ToolsBar.ActualHeight && point.Y > 0);

            return isMouseOver;
        }

        public void AnimateToolbar(bool showToolbar)
        {
            var hidePosition = new Thickness(0, -50, 0, 0);
            var visiblePosition = new Thickness();

            var thickness = showToolbar ? visiblePosition : hidePosition;
            var pointAnimation = new ThicknessAnimation(thickness, TimeSpan.FromMilliseconds(300));
            ToolsBar.BeginAnimation(Grid.MarginProperty, pointAnimation);
        }

        private void OnToggleToolbarMode(object sender, RoutedEventArgs e) =>
            ConfigurationHandler.Current.ToggleToolbarMode(this);

        #endregion

        #region Options

        private void OnLanguageChanged(object sender, SelectionChangedEventArgs e) =>
            ConfigurationHandler.Current.ChangeLanguage(this);

        private void OnFontFamilyChanged(object sender, SelectionChangedEventArgs e) =>
            ConfigurationHandler.Current.ChangeFontFamily();

        private void OnFontSizeChanged(object sender, RoutedPropertyChangedEventArgs<double?> e) =>
            ConfigurationHandler.Current.ChangeFontSize();

        private void OnThemeChanged(object sender, SelectionChangedEventArgs e) =>
            ConfigurationHandler.Current.ChangeTheme(this);

        private void OnFullScreenToggle(object sender, RoutedEventArgs e) =>
            ConfigurationHandler.Current.ToggleFullScreen(this);

        private void OnChangeTemporalFilesLocation(object sender, RoutedEventArgs e) =>
            ConfigurationHandler.Current.ChangeTemporalFilesLocation(TextEditor);

        #endregion

        #region Hyperlink Event

        private void OnHyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e) =>
            NavigateTo(e);

        private void NavigateTo(RequestNavigateEventArgs e)
        {
            var url = e.Uri.AbsoluteUri;

            try
            {
                Process.Start(url);
            }
            catch
            {
                // HACK: because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
                    {
                        CreateNoWindow = true
                    });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    Process.Start("xdg-open", url);
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    Process.Start("open", url);
                else
                    throw;
            }
            finally
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Document Changed Event

        private void OnEditorDocumentStateChanged(object sender, DocumentUpdatedEventArgs e) =>
            UpdateWindowTitle(e);

        private void UpdateWindowTitle(DocumentUpdatedEventArgs args)
        {
            var flag = args.DocumentState == DocumentState.Unsaved ? '*' : ' ';
            string documentTitle = args.DocumentTitle;
            if (documentTitle.Contains('.'))
                documentTitle = documentTitle.Substring(0, documentTitle.IndexOf('.'));

            Title = string.Format(Constants.WindowTitleTemplate, flag + documentTitle);
        }

        #endregion

        #region Closing Event

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = (e.Key == Key.System && e.SystemKey == Key.F4);
            if (e.Handled)
                CacheDataAndSaveChanges(new CancelEventArgs());
        }

        private void OnClosing(object sender, CancelEventArgs e) =>
            CacheDataAndSaveChanges(e);

        private async void CacheDataAndSaveChanges(CancelEventArgs e)
        {
            e.Cancel = true;

            var areChangesUnsaved = TextEditor.StateFullDocument.State == DocumentState.Unsaved;
            if (areChangesUnsaved)
            {
                var result = await AskUserToSaveChanges();
                switch (result)
                {
                    case MessageDialogResult.Negative:
                        CleanCache();
                        CloseApp();
                        break;
                    case MessageDialogResult.Affirmative:
                        if (SaveState() == false)
                            return;
                        CleanCache();
                        CloseApp();
                        break;
                }
            }
            else
                CloseApp();
        }

        private Task<MessageDialogResult> AskUserToSaveChanges()
        {
            return this.ShowMessageAsync(UIText.ClosingWithoutSavingDialogTitle,
                UIText.ClosingWithoutSavingDialogMessage,
                MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                new MetroDialogSettings
                {
                    AffirmativeButtonText = UIText.AffirmativeButtonText,
                    NegativeButtonText = UIText.NegativeButtonText,
                    FirstAuxiliaryButtonText = UIText.CancelButtonText,
                    DialogResultOnCancel = MessageDialogResult.Canceled,
                    DefaultButtonFocus = MessageDialogResult.Affirmative
                });
        }

        private bool SaveState() =>
            Presenter.SaveCommand.Save();

        private void CleanCache() =>
            TextEditor.CleanCache();

        private void CloseApp()
        {
            if (!ConfigurationHandler.Current.IsFullScreen)
                ConfigurationHandler.Current.SaveWindowSize(this);

            Application.Current.Shutdown();
        }

        #endregion
    }
}
