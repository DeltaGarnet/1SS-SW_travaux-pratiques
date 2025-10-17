using DetectLanguageApp.Models;
using DetectLanguageApp.ViewModels.Base;
using DetectLanguageApp.ViewModels.Commands;
using System.Windows.Input;

namespace DetectLanguageApp.ViewModels
{
    public class ConfigurationViewModel : BaseViewModel
    {
        private string _apiToken;
        private bool? _dialogResult;

        public ConfigurationViewModel()
        {
            ApiToken = ConfigurationManager.GetApiToken();
            SaveCommand = new RelayCommand(SaveToken, CanSaveToken);
        }
        public string ApiToken
        {
            get => _apiToken;
            set
            {
                if (SetProperty(ref _apiToken, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        public bool? DialogResult
        {
            get => _dialogResult;
            set => SetProperty(ref _dialogResult, value);
        }

        public ICommand SaveCommand { get; }

        private bool CanSaveToken(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(ApiToken);
        }

        private void SaveToken(object? parameter)
        {
            ConfigurationManager.SetApiToken(ApiToken);
            DialogResult = true;
        }
    }
}
