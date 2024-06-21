using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helper
{
    public class HttpServiceFactory
    {
        private readonly HttpClient _httpClient;

        public HttpServiceFactory(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public void SetAuthenticationToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<TResponse> GetAsync<TResponse>(string requestUri)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                TResponse responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(responseContent) ?? throw new Exception(responseContent);

                return responseData;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("HTTP GET request failed.", ex);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest data)
        {
            try
            {
                string requestData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                TResponse responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(responseContent) ?? throw new Exception(responseContent);
                return responseData;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("HTTP POST request failed.", ex);
            }
        }

        public async Task PostAsync<TRequest>(string requestUri, TRequest data)
        {
            try
            {
                string requestData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("HTTP POST request failed.", ex);
            }
        }

        public async Task<TResponse> PostAsync<TResponse>(string requestUri)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(requestUri, null);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                TResponse responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(responseContent) ?? throw new Exception(responseContent);
                return responseData;
            }
            catch (HttpRequestException ex)
            {
                // Handle or log the exception here
                throw new Exception("HTTP POST request failed.", ex);
            }
        }
    }
}
