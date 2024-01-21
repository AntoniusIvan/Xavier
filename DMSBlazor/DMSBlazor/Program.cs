using Blazored.LocalStorage;
using DMSBlazor.Client.Pages;
using DMSBlazor.Components;
using HybridDMS.Components.Authentication;
using HybridDMS.Components.Data;
using HybridDMS.Components.Pages;
using HybridDMS.Components.Services;
using Microsoft.Maui.Networking;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<TodoService>();
builder.Services.AddSingleton<IConnectivity, BlazorConnectivity>();

builder.Services.AddScoped<CatalogService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly, typeof(Todo).Assembly);

app.Run();

public class BlazorConnectivity : IConnectivity
{
    public IEnumerable<ConnectionProfile> ConnectionProfiles => null;

    public NetworkAccess NetworkAccess => NetworkAccess.Internet;

    //now this is rendering blazor server. If Client interaction (ClientSide), invoke some javascript
    public event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
}
