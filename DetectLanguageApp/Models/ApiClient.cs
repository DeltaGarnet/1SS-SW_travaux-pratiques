using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DetectLanguageApp.Models
{
    public class ApiClient : IDisposable
    {
        private string _urlBaseApi;
        private HttpClient _httpClient;

        public ApiClient(string urlBaseApi)
        {
            if (urlBaseApi.EndsWith('/'))
                urlBaseApi = urlBaseApi.Substring(0, urlBaseApi.Length - 1);
            _urlBaseApi = urlBaseApi;

            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public void SetHttpRequestHeader(string header, string val)
        {
            _httpClient.DefaultRequestHeaders.Remove(header);
            _httpClient.DefaultRequestHeaders.Add(header, val);
        }
        public void RemoveHttpRequestHeader(string header)
        {
            _httpClient.DefaultRequestHeaders.Remove(header);
        }
        public async Task<string> RequeteGetAsync(string endpoint)
        {
            HttpResponseMessage hrm = await _httpClient.GetAsync(_urlBaseApi + endpoint);
            hrm.EnsureSuccessStatusCode();
            return await hrm.Content.ReadAsStringAsync();
        }
        public async Task<string> RequetePostFormAsync(string endpoint, string formUrlEncodedData)
        {
            var content = new StringContent(formUrlEncodedData, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage hrm = await _httpClient.PostAsync(_urlBaseApi + endpoint, content);
            hrm.EnsureSuccessStatusCode();
            return await hrm.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
