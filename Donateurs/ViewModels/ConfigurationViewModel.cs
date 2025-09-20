using Donateurs.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Donateurs.ViewModels
{
    public class ConfigurationViewModel : BaseViewModel
    {
        private string _selectedLanguage;
        private bool _autoRestart;

        public List<string> AvailableLanguages { get; } = new List<string> { "fr", "en-US" };

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }

        public bool AutoRestart
        {
            get => _autoRestart;
            set
            {
                _autoRestart = value;
                OnPropertyChanged(nameof(AutoRestart));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ConfigurationViewModel()
        {
            LoadCurrentSettings();
            SaveCommand = new RelayCommand(SaveSettings);
            CancelCommand = new RelayCommand(CancelSettings);
        }

        private void LoadCurrentSettings()
        {
            var settings = ConfigurationSettings.Load();
            SelectedLanguage = settings.Language;
            AutoRestart = settings.AutoRestart;
        }

        private void SaveSettings(object parameter)
        {
            var currentSettings = ConfigurationSettings.Load();
            bool restartRequired = currentSettings.Language != SelectedLanguage;

            var newSettings = new ConfigurationSettings
            {
                Language = SelectedLanguage,
                AutoRestart = AutoRestart
            };
            newSettings.Save();

            if (restartRequired && newSettings.AutoRestart)
            {
                MessageBox.Show(Donateurs.Properties.translation.RestartMessage, "Redémarrage", MessageBoxButton.OK, MessageBoxImage.Information);
                RestartApplication();
            }
            else
            {
                (parameter as Window)?.Close();
            }
        }

        private void CancelSettings(object parameter)
        {
            (parameter as Window)?.Close();
        }

        private void RestartApplication()
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
