using Cletor.Resources;
using Microsoft.Win32;

namespace Cletor.Views.Helpers
{
    public class RegistryHandler
    {
        private readonly RegistryKey _readerKey;
        private readonly RegistryKey _writerKey;

        public string this[string key]
        {
            get => _readerKey.GetValue(key)?.ToString();
            set => _writerKey.SetValue(key, value);
        }

        public RegistryHandler(string appRegistryKey)
        {
            _writerKey = Registry.CurrentUser.CreateSubKey(appRegistryKey);
            _readerKey = Registry.CurrentUser.OpenSubKey(appRegistryKey);
        }

        public bool IsUnregistered()
        {
            var result = string.IsNullOrEmpty(_readerKey
                .GetValue(Constants.IsToolbarFixedKey) as string);

            return result;
        }
    }
}
