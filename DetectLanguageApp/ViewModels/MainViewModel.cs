using DetectLanguageApp.Models;
using DetectLanguageApp.Services;
using DetectLanguageApp.ViewModels.Base;
using DetectLanguageApp.Views;
using DetectLanguageApp.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DetectLanguageApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly DetectLanguageService _service;
        private string _inputText;
        private bool _isLoading;
        private string _errorMessage;
        private Detection _selectedDetection;

        public ObservableCollection<Detection> Detections { get; } = new ObservableCollection<Detection>();

        public MainViewModel(DetectLanguageService service)
        {
            _service = service;
            DetectCommand = new AsyncCommand(DetectLanguageAsync, CanDetectLanguage);
            OpenConfigurationCommand = new RelayCommand(OpenConfiguration, null);
            OpenStatusCommand = new RelayCommand(OpenStatus, null);

            CheckTokenStatus();
        }
        public void CheckTokenStatus()
        {
            if (!_service.IsReady)
            {
                ErrorMessage = "ERREUR: Jeton d'API manquant. Veuillez configurer le jeton via le menu 'Configuration'.";
            }
            else
            {
                ErrorMessage = null;
                Task.Run(_service.LoadLanguagesAsync);
            }
            CommandManager.InvalidateRequerySuggested();
        }
        public string InputText
        {
            get => _inputText;
            set
            {
                if (SetProperty(ref _inputText, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                    Detections.Clear();
                    SelectedDetection = null;
                }
            }
        }
        public Detection SelectedDetection
        {
            get => _selectedDetection;
            set => SetProperty(ref _selectedDetection, value);
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

        public ICommand DetectCommand { get; }
        public ICommand OpenConfigurationCommand { get; }
        public ICommand OpenStatusCommand { get; }

        private bool CanDetectLanguage(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(InputText) && _service.IsReady;
        }

        private async Task DetectLanguageAsync(object? parameter)
        {
            IsLoading = true;
            ErrorMessage = null;
            Detections.Clear();
            SelectedDetection = null;

            try
            {
                var results = await _service.DetectLanguageAsync(InputText);

                if (results.Any())
                {
                    foreach (var result in results)
                    {
                        Detections.Add(result);
                    }
                    SelectedDetection = Detections.First();
                }
                else
                {
                    ErrorMessage = "Aucune langue n'a pu être détectée pour le texte fourni.";
                }
            }
            catch (MissingTokenException ex)
            {
                ErrorMessage = $"ERREUR: {ex.Message}";
                MessageBox.Show(ex.Message, "Jeton Manquant", MessageBoxButton.OK, MessageBoxImage.Error);
                OpenConfiguration(null);
            }
            catch (InvalidTokenException ex)
            {
                ErrorMessage = $"ERREUR de jeton: {ex.Message}";
                MessageBox.Show(ex.Message, "Jeton Invalide", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void OpenConfiguration(object? parameter)
        {
            var configWindow = new ConfigurationWindow();
            configWindow.Owner = Application.Current.MainWindow;
            if (configWindow.ShowDialog() == true)
            {
                CheckTokenStatus();
            }
        }

        private void OpenStatus(object? parameter)
        {
            var statusWindow = new AccountStatusWindow
            {
                DataContext = new AccountStatusViewModel(_service),
                Owner = Application.Current.MainWindow
            };
            statusWindow.ShowDialog();
        }
    }
}
