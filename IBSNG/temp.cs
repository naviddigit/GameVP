using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IbsngApiDemo
{
    public class IbsngApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public IbsngApi(string baseUrl = "http://185.126.9.115:8000/IBSng/admin/api")
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
        }

        public string Login(string username, string password)
        {
            var requestUri = $"{_baseUrl}";
            var requestContent = new StringContent(JsonConvert.SerializeObject(new
            {
                username,
                password
            }), Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(requestUri, requestContent);

            response.Result.EnsureSuccessStatusCode();
            var responseContent = response.Result.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseContent.Result);
            return result.token;
        }

        public async Task<IEnumerable<dynamic>> SearchUser(string token, string username)
        {
            var requestUri = $"{_baseUrl}/user/search?q={username}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(responseContent);
            return result;
        }

        public void AddUser(string token, string username, string password, string firstName, string lastName, string email)
        {
            var requestUri = $"{_baseUrl}/user";
            var requestContent = new StringContent(JsonConvert.SerializeObject(new
            {
                username,
                password,
                first_name = firstName,
                last_name = lastName,
                email
            }), Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            requestMessage.Content = requestContent;
            var response = _httpClient.SendAsync(requestMessage);

            response.Result.EnsureSuccessStatusCode();
            var responseContent = response.Result.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseContent.Result);
            //return result;
        }
    }
}
