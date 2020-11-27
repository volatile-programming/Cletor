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
    }
}
