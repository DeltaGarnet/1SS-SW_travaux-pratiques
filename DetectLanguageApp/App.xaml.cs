using DetectLanguageApp.Services;
using DetectLanguageApp.ViewModels;
using DetectLanguageApp.Views;
using System.Windows;

namespace DetectLanguageApp
{
    public partial class App : Application
    {
        private DetectLanguageService _detectLanguageService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _detectLanguageService = new DetectLanguageService();
            var mainWindow = new MainWindow
            {
                DataContext = new MainViewModel(_detectLanguageService)
            };
            mainWindow.Show();
            if (!_detectLanguageService.IsReady)
            {
                ((MainViewModel)mainWindow.DataContext).OpenConfigurationCommand.Execute(null);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _detectLanguageService?.Dispose();
            base.OnExit(e);
        }
    }
}
