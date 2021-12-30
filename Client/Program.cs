using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using JWTWebToken.Client.Handlers;
using JWTWebToken.Client.Services;


namespace JWTWebToken.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // Microsoft.AspNetCore.Components.Authorization
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // LidClassService
            builder.Services.AddScoped<IProfielInterface, ProfielService>();

            // AddHttpMessageHandler
            builder.Services.AddHttpClient<IProfielInterface, ProfielService>("ProfileClassService", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
               .AddHttpMessageHandler<DelegatieHandler>();

            // Authentication State Provider 
            builder.Services.AddScoped<AuthenticationStateProvider, AuthenticatieStatus>();

            // Local Storage
            builder.Services.AddBlazoredLocalStorage();

            // DelegatieHandler
            builder.Services.AddTransient<DelegatieHandler>();

            await builder.Build().RunAsync();
        }
    }
}
