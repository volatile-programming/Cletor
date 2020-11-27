using Cletor.Presenters;
using Cletor.Resources;
using Cletor.Resources.Enums;
using Cletor.Resources.Languages;
using Cletor.Views.Controls;
using ControlzEx.Theming;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using Application = System.Windows.Application;

namespace Cletor.Views.Helpers
{
    public class ConfigurationHandler : BasePresenter
    {
        private static ConfigurationHandler _instance;
        private readonly RegistryHandler _registryHandler;
        private string _previousTemporalFilesPath;
        private ConfigurationHandler()
        {
            _registryHandler = new RegistryHandler(Constants.AppRegistryKey);

            if (_registryHandler.IsUnregistered())
                RegisterInitialConfiguration();
            _previousTemporalFilesPath = TemporalFilesPath;
        }

        public static ConfigurationHandler Current => _instance ??= new ConfigurationHandler();

        #region Register and Load Options

        public void ApplyConfiguration(MainWindow window)
        {
            if (IsFullScreen)
                ToggleFullScreen(window);
            else
            {
                window.Width = WindowWidth;
                window.Height = WindowHeight;
            }

            ChangeLanguage(window);
            ChangeTheme(window);
            ChangeFontFamily();
            ChangeFontSize();

            if (!IsToolbarFixed)
                window.AnimateToolbar(false);
        }

        private void RegisterInitialConfiguration()
        {
            _registryHandler[Constants.TemporalFilesKey] = Constants.TemporalFilesValue;
            _registryHandler[Constants.ThemeKey] = Constants.ThemeValue;
            _registryHandler[Constants.LanguageKey] = Constants.LanguageValue;
            _registryHandler[Constants.FontFamilyKey] = Constants.FontFamilyValue;
            _registryHandler[Constants.FontSizeKey] = Constants.FontSizeValue;
            _registryHandler[Constants.WindowWidthKey] = Constants.WindowWidthValue;
            _registryHandler[Constants.WindowHeightKey] = Constants.WindowHeightValue;
            _registryHandler[Constants.IsFullScreenKey] = Constants.IsFullScreenValue;
            _registryHandler[Constants.IsToolbarFixedKey] = Constants.IsToolbarFixedValue;
        }

        #endregion

        #region Toolbar

        public bool IsToolbarFixed
        {
            get => bool.Parse(_registryHandler[Constants.IsToolbarFixedKey]);
            set => _registryHandler[Constants.IsToolbarFixedKey] = value.ToString();
        }

        public void ToggleToolbarMode(MainWindow mainWindow, bool? isFullScreen = null)
        {
            IsToolbarFixed = !isFullScreen ?? !IsToolbarFixed;
            mainWindow.AnimateToolbar(IsToolbarFixed);

            if (isFullScreen != null)
                mainWindow.PinButton.Visibility = isFullScreen == true ?
                    Visibility.Visible :
                    Visibility.Collapsed;

            if (mainWindow.Presenter != null)
                mainWindow.Presenter.PinIcon = IsToolbarFixed ?
                    PackIconModernKind.ArrowUp :
                    PackIconModernKind.ArrowDown;
        }

        #endregion

        #region Temporal Files

        public string TemporalFilesPath
        {
            get
            {
                var value = _registryHandler[Constants.TemporalFilesKey];
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);

                return value;
            }
            set => _registryHandler[Constants.TemporalFilesKey] = value;
        }

        public void ChangeTemporalFilesLocation(StateFullTextEditor textEditor)
        {
            var openFileDialog = new FolderBrowserDialog()
            {
                Description = UIText.FolderPickerTitle,
                UseDescriptionForTitle = true,
                ShowNewFolderButton = true,
                AutoUpgradeEnabled = true,
                SelectedPath = TemporalFilesPath
            };

            var result = openFileDialog.ShowDialog();
            if (result != DialogResult.OK)
                return;

            TemporalFilesPath = openFileDialog.SelectedPath + @"\";
            textEditor.CacheChanges();
            ClearCache(_previousTemporalFilesPath);
            _previousTemporalFilesPath = TemporalFilesPath;
        }

        public void ClearCache(string temporalFilesPath)
        {
            if (File.Exists(temporalFilesPath))
                File.Delete(temporalFilesPath);
            else if (temporalFilesPath.Contains(Constants.TempFilesSubPath))
            {
                foreach (var file in Directory.GetFiles(_previousTemporalFilesPath))
                    File.Delete(file);

                foreach (var directory in Directory.GetDirectories(_previousTemporalFilesPath))
                    ClearCache(directory);

                Directory.Delete(_previousTemporalFilesPath);
            }
        }

        #endregion

        #region Language

        public string Language
        {
            get => _registryHandler[Constants.LanguageKey];
            set => _registryHandler[Constants.LanguageKey] = value;
        }

        public string SelectedLanguage
        {
            get => TranslateLanguage(Language);
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value == Language)
                    return;

                Language = TranslateLanguage(value);
            }
        }

        // Local value converter.
        private string TranslateLanguage(string languageToTranslate)
        {
            if (string.IsNullOrWhiteSpace(languageToTranslate))
                return null;

            if (languageToTranslate.Length == 2)
            {
                switch (languageToTranslate)
                {
                    case "es":
                        return UIText.LanguageSpanish;
                    case "fr":
                        return UIText.LanguageFrench;
                    default:
                        return UIText.LanguageEnglish;
                }
            }

            if (UIText.LanguageSpanish == languageToTranslate)
                return "es";

            if (UIText.LanguageFrench == languageToTranslate)
                return "fr";

            return "en";
        }

        public string[] Languages => new[]
        {
            UIText.LanguageEnglish,
            UIText.LanguageSpanish,
            UIText.LanguageFrench
        };

        public void ChangeLanguage(MainWindow mainWindow)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
            mainWindow.Language = XmlLanguage.GetLanguage(Language);
            mainWindow.TextEditor.Language = XmlLanguage.GetLanguage(Language);
        }

        #endregion

        #region Theme

        public Themes Theme
        {
            get => (Themes)Enum.Parse(typeof(Themes), _registryHandler[Constants.ThemeKey]);
            set => _registryHandler[Constants.ThemeKey] = value.ToString();
        }

        public void ChangeTheme(MainWindow mainWindow, Themes? theme = null)
        {
            if (theme != null)
                Theme = theme.Value;

            ThemeManager.Current.ChangeTheme(mainWindow, $"{Theme}.Blue");
        }

        public Themes[] ThemeList => new[] { Themes.Dark, Themes.Light };

        #endregion

        #region Font Family

        public FontFamily FontFamily
        {
            get => new FontFamily(_registryHandler[Constants.FontFamilyKey]);
            set => _registryHandler[Constants.FontFamilyKey] = value.Source;
        }

        public ICollection<FontFamily> FontFamilies => Fonts.SystemFontFamilies;

        public void ChangeFontFamily() =>
            Application.Current.Resources["BaseFontFamily"] = FontFamily;

        #endregion

        #region Font Size

        public double FontSize
        {
            get => double.Parse(_registryHandler[Constants.FontSizeKey]);
            set => _registryHandler[Constants.FontSizeKey] = value.ToString();
        }

        public void ChangeFontSize()
        {
            Application.Current.Resources["BaseFontSize"] = FontSize;
            Application.Current.Resources["BaseHeaderFontSize"] = FontSize * 1.25;
        }

        #endregion

        #region Full Screen

        public bool IsFullScreen
        {
            get => bool.Parse(_registryHandler[Constants.IsFullScreenKey]);
            set => _registryHandler[Constants.IsFullScreenKey] = value.ToString();
        }

        public void ToggleFullScreen(MainWindow mainWindow)
        {
            mainWindow.ShowCloseButton = !IsFullScreen;
            mainWindow.ShowMaxRestoreButton = !IsFullScreen;
            mainWindow.ShowMinButton = !IsFullScreen;
            mainWindow.KeepBorderOnMaximize = !IsFullScreen;
            mainWindow.ShowTitleBar = !IsFullScreen;

            mainWindow.WindowState = IsFullScreen ?
                WindowState.Maximized :
                WindowState.Normal;
            mainWindow.MainStatusBar.Visibility = IsFullScreen ?
                Visibility.Collapsed :
                Visibility.Visible;

            ToggleToolbarMode(mainWindow, IsFullScreen);
        }
        #endregion

        #region Window Size

        public double WindowHeight
        {
            get => double.Parse(_registryHandler[Constants.WindowHeightKey]);
            set => _registryHandler[Constants.WindowHeightKey] = value.ToString();
        }

        public double WindowWidth
        {
            get => double.Parse(_registryHandler[Constants.WindowWidthKey]);
            set => _registryHandler[Constants.WindowWidthKey] = value.ToString();
        }

        public void SaveWindowSize(MainWindow mainWindow)
        {
            WindowWidth = mainWindow.Width;
            WindowHeight = mainWindow.Height;
        }

        #endregion
    }
}
