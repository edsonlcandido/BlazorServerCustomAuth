using BlazorServerCustomAuth.Requests;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using DotNetEnv;
using System.Text.Json.Serialization;

namespace BlazorServerCustomAuth.Services
{
    public class ExternalAuthService
    {
        public bool IsAuthenticated { get; private set; }
        private readonly HttpClient _httpClient;

        public ExternalAuthService(HttpClient httpClient) 
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            //Env.Load();

            //var baseUrl = Env.GetString("BACKEND_URI");

            //HttpClient httpClient = new HttpClient();
            var response = await _httpClient.PostAsJsonAsync($"api/collections/users/auth-with-password",
                new
                {
                    identity = loginRequest.Email,
                    password = loginRequest.Password
                }
               );
            response.EnsureSuccessStatusCode();
            LoginResponse loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (loginResponse.Token != null) {
                IsAuthenticated = true;
                return loginResponse;
            }

            return new LoginResponse();

            //AccessToken = "12345698";
            //return new LoginResponse()
            //{
            //    Token = "12345698",
            //    User = new ApiUser()
            //    {
            //        Id = "1",
            //        Name = "Edson",
            //        Email = "",
            //        Role = "General"
            //    }
            //};
        }

        public void Logout()
        {
            IsAuthenticated = false;
        }
    }

    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("record")]
        public ApiUser User { get; set; }
    }

    public class ApiUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("username")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}
