using System;
using System.Reflection;

namespace Cletor.Resources
{
    public static class StaticData
    {
        public static string CachePath
        {
            get
            {
                var cachePath = Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData) + @"\Cletor\";

                return cachePath;
            }
        }

        public static string Exe => Assembly.GetExecutingAssembly().GetName().Name;

        public static string InstallationPath => Assembly.GetExecutingAssembly().Location;

        public static string ContactEmail => "contact@volatileprogramming.org";
        public static string OrgLink => "https://volatileprogramming.org/";
        public static string OrgGithubLink => "https://github.com/volatile-programming";
        public static string RepositoryLink => "https://github.com/volatile-programming/Cletor";
        public static string RepositoryLicence => "https://github.com/volatile-programming/Cletor/blob/master/LICENSE";
        public static string RepositoryInstallerLicence => "https://github.com/volatile-programming/Cletor/blob/master/INSTALLER-LICENCE.md";
        public static string GuardClauseRepository => "https://github.com/ardalis/GuardClauses";
        public static string GuardClauseLicence => "https://github.com/ardalis/GuardClauses/blob/master/LICENSE";
        public static string MahAppLink => "https://mahapps.com/";
        public static string MahAppLicence => "https://github.com/MahApps/MahApps.Metro/blob/develop/LICENSE";
        public static string MahAppIconsLink => "https://github.com/MahApps/MahApps.Metro.IconPacks";
        public static string MahAppIconsLicence => "https://github.com/MahApps/MahApps.Metro.IconPacks/blob/develop/LICENSE";
        public static string PropertyChangeFodyLink => "https://github.com/Fody/PropertyChanged";
        public static string PropertyChangeFodyLicence => "https://github.com/Fody/PropertyChanged/blob/master/license.txt";
        public static string SyncfusionWpfUiLink => "https://www.syncfusion.com/wpf-ui-controls";
        public static string SyncfusionLicence => "https://www.syncfusion.com/license/studio/18.3.0.47/syncfusion_essential_studio_eula.pdf";
        public static string ReverseMarkdownLink => "https://github.com/mysticmind/reversemarkdown-net";
        public static string ReverseMarkdownLicence => "https://github.com/mysticmind/reversemarkdown-net/blob/master/LICENSE";
        public static string MarkdigLink => "https://github.com/lunet-io/markdig";
        public static string MarkdigLicence => "https://github.com/lunet-io/markdig/blob/master/license.txt";
    }
}
