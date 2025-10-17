using System;
using DetectLanguageApp.Properties;

namespace DetectLanguageApp.Models
{
    public static class ConfigurationManager
    {
        private const string ApiTokenKey = "ApiToken";
        private const string ApiBaseUrl = "https://ws.detectlanguage.com/0.2";
        public static string GetApiToken()
        {
            try
            {
                return (string)Settings.Default[ApiTokenKey];
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void SetApiToken(string token)
        {
            Settings.Default[ApiTokenKey] = token;
            Settings.Default.Save();
        }
        public static string GetApiBaseUrl()
        {
            return ApiBaseUrl;
        }
    }
}
