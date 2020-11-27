namespace Cletor.Resources
{
    public static class Constants
    {
        public const string AppRegistryKey = @"SOFTWARE\Cletor";

        public const string TemporalFilesKey = "TemporalFilesPath";
        public static string TemporalFilesValue => StaticData.CachePath;

        public const string ThemeKey = "Theme";
        public const string ThemeValue = "Dark";

        public const string LanguageKey = "Language";
        public const string LanguageValue = "en";

        public const string FontFamilyKey = "FontFamily";
        public const string FontFamilyValue = "Consolas";

        public const string FontSizeKey = "FontSize";
        public const string FontSizeValue = "14";

        public const string WindowWidthKey = "WindowWidth";
        public const string WindowWidthValue = "640";

        public const string WindowHeightKey = "WindowHeight";
        public const string WindowHeightValue = "480";

        public static string IsFullScreenKey = "IsFullScreen";
        public static string IsFullScreenValue = "False";

        public static string IsToolbarFixedKey = "IsToolbarFixed";
        public static string IsToolbarFixedValue = "True";

        public const string WindowTitleTemplate = "Clean Editor | {0}";

        public const string DefaultFileExtension = "md";
        public const string HtmFileExtension = "htm";
        public const string TempFilesSubPath = "Cletor";
        public const string HtmlFileExtension = "html";
        public const string TempFileExtension = ".temp.";

        public const string NormalStyleName = "Normal";
        public const string Heading1StyleName = "Heading 1";
        public const string Heading2StyleName = "Heading 2";
        public const string Heading3StyleName = "Heading 3";
        public const string Heading4StyleName = "Heading 4";
        public const string Heading5StyleName = "Heading 5";
        public const string Heading6StyleName = "Heading 6";
        public const string HyperlinkStyleName = "Hyperlink";

        public const string MahAppsColorsGray3 = "MahApps.Colors.Gray3";
        public const string MahAppsColorsGray9 = "MahApps.Colors.Gray9";

        // 18.3.0.50
        // TODO: add your custom key here.
        internal const string SyncfusionKey = "";
    }
}
