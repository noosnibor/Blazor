using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace PortalWeb.Services
{
    public class CustomAuthenticationProvider(ProtectedSessionStorage protectedSessionStorage) : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage protectedSessionStorage = protectedSessionStorage;
        private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return new AuthenticationState(Anonymous);
        }
    }
}
