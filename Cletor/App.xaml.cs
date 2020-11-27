using Cletor.Resources;
using Syncfusion.Licensing;

namespace Cletor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App() =>
            SyncfusionLicenseProvider.RegisterLicense(Constants.SyncfusionKey);
    }
}
