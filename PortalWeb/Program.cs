using Custom.Toast.Extensions;
using Custom.Toast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PortalWeb.Components;
using PortalWeb.Models;
using PortalWeb.Services;
using SqlDataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddRazorPages();
//builder.Services.AddServerSideBlazor()
//    .AddCircuitOptions(options =>
//    {
//        options.DetailedErrors = true;
//    });

//Connection String
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Dependency Injection
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess.SqlDataAccess>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddScoped<IToastService, ToastService>();
builder.Services.AddScoped<UserSession>();
builder.Services.AddScoped<LoginModel>();

// Toast
builder.Services.AddCustomToast();

builder.Services.AddAuthorizationCore(options =>
{
    var permissions = MemoryStoredData
        .GetPermissions()
        .Select(x => x.fstrPermission)
        .Distinct()
        .ToList();

    foreach (var permission in permissions)
    {
        options.AddPolicy(permission!, policy =>
        {
            policy.Requirements.Add(new PermissionRequirement(permission!));
        });
    }
});

builder.Services
    .AddAuthentication("AuthCookie")
    .AddCookie("AuthCookie", options =>
    {
        options.LoginPath = "/";
        options.AccessDeniedPath = "/";
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        };
    });

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
