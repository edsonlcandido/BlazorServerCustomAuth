using BlazorServerCustomAuth.Requests;
using BlazorServerCustomAuth.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Reflection;
using System.Security.Claims;

namespace BlazorServerCustomAuth.Providers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly LocalStorageService _localStorageService;
        private readonly ExternalAuthService _externalAuthService;
        private readonly InitializationService _initializationService;
        private AuthenticationState _authenticationState;

        public CustomAuthenticationStateProvider(LocalStorageService localStorageService, 
            ExternalAuthService externalAuthService, InitializationService initializationService)
        {
            _localStorageService = localStorageService;
            _externalAuthService = externalAuthService;
            _initializationService = initializationService;
            _authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task InitializeAuthenticationStateAsync()
        {
            // Aguarda até que o JavaScript esteja pronto para acessar o localStorage
            await _initializationService.WaitForJavaScriptAsync();

            var result = await _localStorageService.GetUserFromBrowserAsync();
            var loginResponse = result;
            if (loginResponse != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginResponse.User.Name),
                    new Claim(ClaimTypes.Email, loginResponse.User.Email),
                    new Claim(ClaimTypes.Role, loginResponse.User.Role),
                    new Claim("Token", loginResponse.Token)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "CustomAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                _authenticationState = new AuthenticationState(claimsPrincipal);
            }
            else
            {
                _authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            NotifyAuthenticationStateChanged(Task.FromResult(_authenticationState));
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Inicializa com um estado não autenticado
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(anonymous);
        }
        public async Task LoginAsync(LoginRequest loginRequest)
        {
            var LoginResponse = await _externalAuthService.Login(loginRequest);
            if (LoginResponse != null)
            {
                await _localStorageService.PersistUserToBrowserAsync(LoginResponse);

                var claims = new List<Claim>
                 {
                     new Claim(ClaimTypes.Name, LoginResponse.User.Name),
                     new Claim(ClaimTypes.Email, LoginResponse.User.Email),
                     new Claim(ClaimTypes.Role, LoginResponse.User.Role),
                     new Claim("Token", LoginResponse.Token)
                 };
                var claimsIdentity = new ClaimsIdentity(claims,"CustomAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                _authenticationState = new AuthenticationState(claimsPrincipal);
                NotifyAuthenticationStateChanged(Task.FromResult(_authenticationState));
            }
        }
    }
}
