using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using TimeKeep.Client;
using TimeKeep.Client.Providers;
using TimeKeep.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<PTOEntriesService>();

builder.Services.AddBlazoredLocalStorage();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTYyNDcxQDMxMzkyZTM0MmUzMGVITC9uUnNPcG5kU001cjlWYjBxYk1RVjVBWWhaZC9iK3JicUNua29VV1E9");
builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();

await builder.Build().RunAsync();
