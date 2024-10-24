using Microsoft.JSInterop;

namespace BlazorServerCustomAuth.Services
{
    public class InitializationService
    {
        private readonly IJSRuntime _jsRuntime;

        public InitializationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task WaitForJavaScriptAsync()
        {
            await _jsRuntime.InvokeVoidAsync("blazorInterop.waitForJavaScript");
        }
    }
}
