using DetectLanguageApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DetectLanguageApp.Services
{
    public class DetectLanguageService : IDisposable
    {
        private readonly ApiClient _apiClient;
        private Dictionary<string, string> _languageMap;
        public bool IsReady => !string.IsNullOrEmpty(ConfigurationManager.GetApiToken());
        public DetectLanguageService()
        {
            _apiClient = new ApiClient(ConfigurationManager.GetApiBaseUrl());
        }
        private void SetupAuthentication(string token)
        {
            _apiClient.SetHttpRequestHeader("Authorization", $"Bearer {token}");
        }
        public async Task LoadLanguagesAsync()
        {
            try
            {
                SetupAuthentication(ConfigurationManager.GetApiToken());
                string jsonResponse = await _apiClient.RequeteGetAsync("/languages");
                LanguageListResponse wrapped = null;
                try
                {
                    wrapped = JsonConvert.DeserializeObject<LanguageListResponse>(jsonResponse);
                }
                catch (JsonException)
                {
                    wrapped = null;
                }

                if (wrapped?.Data?.Languages != null)
                {
                    _languageMap = wrapped.Data.Languages.ToDictionary(l => l.Code, l => l.Name);
                    return;
                }
                try
                {
                    var list = JsonConvert.DeserializeObject<List<Language>>(jsonResponse);
                    if (list != null && list.Any())
                    {
                        _languageMap = list.ToDictionary(l => l.Code, l => l.Name);
                        return;
                    }
                }
                catch (JsonException jex)
                {
                    throw new Exception($"Erreur lors de la désérialisation de la liste des langues: {jex.Message}", jex);
                }
                throw new Exception("Erreur lors de la récupération de la liste des langues: réponse inattendue du serveur.");
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("401"))
            {
                throw new InvalidTokenException("Le jeton d'API est invalide ou a expiré.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de la liste des langues: {ex.Message}", ex);
            }
        }
        public async Task<List<Detection>> DetectLanguageAsync(string text)
        {
            if (_languageMap == null || !_languageMap.Any())
            {
                await LoadLanguagesAsync();
            }

            string token = ConfigurationManager.GetApiToken();
            if (string.IsNullOrEmpty(token))
            {
                throw new MissingTokenException("Le jeton d'API est manquant. Veuillez le configurer.");
            }

            SetupAuthentication(token);
            string postData = $"q={Uri.EscapeDataString(text)}";

            try
            {
                string jsonResponse = await _apiClient.RequetePostFormAsync("/detect", postData);
                var response = JsonConvert.DeserializeObject<DetectionResponse>(jsonResponse);

                if (response?.Data?.Detections != null)
                {
                    foreach (var detection in response.Data.Detections)
                    {
                        detection.LanguageName = _languageMap.GetValueOrDefault(detection.LanguageCode, detection.LanguageCode);
                    }
                    return response.Data.Detections;
                }

                return new List<Detection>();
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("401"))
            {
                throw new InvalidTokenException("Le jeton d'API est invalide ou a expiré.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la détection de la langue: {ex.Message}", ex);
            }
        }
        public async Task<AccountStatus> GetAccountStatusAsync()
        {
            string token = ConfigurationManager.GetApiToken();
            if (string.IsNullOrEmpty(token))
            {
                throw new MissingTokenException("Le jeton d'API est manquant. Impossible de vérifier le statut sans jeton.");
            }

            SetupAuthentication(token);

            try
            {
                string jsonResponse = await _apiClient.RequeteGetAsync("/user/status");
                var response = JsonConvert.DeserializeObject<AccountStatusResponse>(jsonResponse);

                return response?.Data;
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("401"))
            {
                throw new InvalidTokenException("Le jeton d'API est invalide ou a expiré.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération du statut du compte: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            _apiClient?.Dispose();
        }
    }
    public class MissingTokenException : Exception
    {
        public MissingTokenException(string message) : base(message) { }
        public MissingTokenException(string message, Exception innerException) : base(message, innerException) { }
    }
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string message) : base(message) { }
        public InvalidTokenException(string message, Exception innerException) : base(message, innerException) { }
    }
}
