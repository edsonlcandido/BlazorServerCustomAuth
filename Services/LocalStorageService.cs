using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorServerCustomAuth.Services
{
    public class LocalStorageService
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly string _key = "AuthApp";

        public LocalStorageService(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async Task PersistUserToBrowserAsync(LoginResponse user)
        {

            string userJson = JsonSerializer.Serialize(user);
            await _protectedLocalStorage.SetAsync(_key, userJson);
        }
        public async Task<LoginResponse> GetUserFromBrowserAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>(_key);
            var userJson = result.Success ? result.Value : null;
            if (string.IsNullOrEmpty(userJson))
            {
                return null;
            }
            return JsonSerializer.Deserialize<LoginResponse>(userJson);
        }

        internal async Task ClearUserFromBrowserAsync()
        {
            await _protectedLocalStorage.DeleteAsync(_key);
        }
    }
}
