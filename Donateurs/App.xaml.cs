using System.Globalization;
using System.Threading;
using System.Windows;
using Donateurs.Models;

namespace Donateurs
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var settings = ConfigurationSettings.Load();
            var culture = new CultureInfo(settings.Language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            base.OnStartup(e);
        }
    }
}
