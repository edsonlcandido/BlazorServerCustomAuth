using Microsoft.JSInterop;

namespace BlazorServerCustomAuth.Midlewares
{
    public class JavaScriptInitializationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJSRuntime _jsRuntime;

        public JavaScriptInitializationMiddleware(RequestDelegate next, IJSRuntime jsRuntime)
        {
            _next = next;
            _jsRuntime = jsRuntime;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _jsRuntime.InvokeVoidAsync("blazorInterop.waitForJavaScript");
            await _next(context);
        }
    }
}
