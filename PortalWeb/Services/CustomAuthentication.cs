using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Client.Extensions.Msal;
using PortalWeb.Components.Pages;
using System.Security.Claims;

namespace PortalWeb.Services;

public class CustomAuthentication(UserSession user) : AuthenticationStateProvider
{
    private readonly UserSession user = user;
    // This is for user that is not logged in
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (user.IsAuthenticated)
        {
            return Task.FromResult(new AuthenticationState(user.Principal!));
        }
        else
        {
            return Task.FromResult(new AuthenticationState(Anonymous));
        }
        
    }

    public void NotifyUserAuthentication(ClaimsPrincipal user)
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Anonymous)));
    }
}
