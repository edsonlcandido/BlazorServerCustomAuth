namespace BlazorServerCustomAuth
{
    public class ExternalAuthService
    {
        public bool IsAuthenticated { get; private set; }

        public async Task<LoginResponse> Login()
        {
            IsAuthenticated = true;
            await Task.Delay(1000);
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
