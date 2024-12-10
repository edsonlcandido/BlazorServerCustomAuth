using BlazorServerCustomAuth.Data;
using BlazorServerCustomAuth.Midlewares;
using BlazorServerCustomAuth.Providers;
using BlazorServerCustomAuth.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorServerCustomAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Env.Load();
            //var baseUrl = Env.GetString("BACKEND_URI");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "AuthCookie";
                    options.LoginPath = "/login";
                    options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
                    options.AccessDeniedPath = "/access-denied";
                });

            builder.Services.AddSingleton<WeatherForecastService>();
            //builder.Services.AddSingleton<HttpContext>();
            //builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ExternalAuthService>();
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new ("https://api.edsonluizcandido.com.br")
            });
            builder.Services.AddScoped<LocalStorageService>();
            builder.Services.AddScoped<CustomAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());
            builder.Services.AddScoped<ProtectedLocalStorage>();
            builder.Services.AddScoped<InitializationService>();
            builder.Services.AddAuthorizationCore();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}
