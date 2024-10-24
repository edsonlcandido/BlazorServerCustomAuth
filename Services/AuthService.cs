using BlazorServerCustomAuth.Requests;
using Microsoft.AspNetCore.Components;

namespace BlazorServerCustomAuth.Services
{
    public class ExternalAuthService
    {
        [CascadingParameter(Name = "AccessToken")]
        public string AccessToken { get; set; }
        public bool IsAuthenticated { get; private set; }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            IsAuthenticated = true;
            await Task.Delay(500);
            AccessToken = "12345698";
            return new LoginResponse()
            {
                Token = "12345698",
                User = new ApiUser()
                {
                    Id = "1",
                    Name = "Edson",
                    Email = "",
                    Role = "Admin"
                }
            };
        }

        public void Logout()
        {
            IsAuthenticated = false;
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }

        public ApiUser User { get; set; }
    }

    public class ApiUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
