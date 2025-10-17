using DetectLanguageApp.Models;
using DetectLanguageApp.Services;
using DetectLanguageApp.ViewModels.Base;
using DetectLanguageApp.ViewModels.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DetectLanguageApp.ViewModels
{
    public class AccountStatusViewModel : BaseViewModel
    {
        private readonly DetectLanguageService _service;
        private AccountStatus _status;
        private bool _isLoading;
        private string _errorMessage;

        public AccountStatusViewModel(DetectLanguageService service)
        {
            _service = service;
            LoadStatusCommand = new AsyncCommand(LoadStatusAsync, null);
        }
        public AccountStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoadStatusCommand { get; }
        private async Task LoadStatusAsync(object? parameter)
        {
            IsLoading = true;
            ErrorMessage = null;
            Status = null;

            try
            {
                Status = await _service.GetAccountStatusAsync();
            }
            catch (MissingTokenException ex)
            {
                ErrorMessage = $"ERREUR: {ex.Message} Impossible de vérifier le statut sans jeton.";
            }
            catch (InvalidTokenException ex)
            {
                ErrorMessage = $"ERREUR de jeton: {ex.Message} Veuillez vérifier votre configuration.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"ERREUR: Une erreur inattendue est survenue: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
