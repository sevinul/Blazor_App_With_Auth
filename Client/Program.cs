using Blazor.WebAssembly.DynamicCulture.Extensions;
using Blazor.WebAssembly.DynamicCulture.Provider;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor_App_With_Auth.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

			/* TODO
            builder.Services.AddSingleton(new HttpClient
            { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }
            );
            builder.Services.AddApiAuthorization();
			*/			
			  
            builder.Services.AddHttpClient(
                "Blazor_App_With_Auth.API", 
                client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorApp17.ServerAPI"));

            builder.Services.AddApiAuthorization()
                .AddAccountClaimsPrincipalFactory<RolesClaimsPrincipalFactory>();

            // Add Localization
            builder.Services.AddLocalization();
            builder.Services.AddLocalizationDynamic(options =>
            {
                string[] languages = [ "en", "de", "fr", "tr" ];
                options.SetDefaultCulture("de");
                options.AddSupportedCultures(languages);
                options.AddSupportedUICultures(languages);
                // options.IgnoreCulture = true;
                options.CultureProviders = new List<ICultureProvider>()
                {
                    new AcceptLanguageHeaderCultureProvider()
                };
            });

            // Set Middleware
            var host = builder.Build();
            await host.SetMiddlewareCulturesAsync();
            // Run
            await host.RunAsync();
        }
    }
}
