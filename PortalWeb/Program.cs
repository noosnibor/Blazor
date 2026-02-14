using Custom.Toast.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PortalWeb.Components;
using PortalWeb.Services;
using SqlDataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Connection String
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Dependency Injection
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationProvider>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess.SqlDataAccess>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

// Toast
builder.Services.AddCustomToast();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
