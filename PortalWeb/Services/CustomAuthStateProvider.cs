using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace PortalWeb.Services
{
    public class CustomAuthStateProvider(ProtectedSessionStorage storage, UserSession user) : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage storage = storage;
        private readonly UserSession user = user;

        private static readonly ClaimsPrincipal Anonymous =
            new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var result = await storage.GetAsync<string>("authToken");

                if (!result.Success || string.IsNullOrWhiteSpace(result.Value))
                    return new AuthenticationState(Anonymous);

                var claims = ParseClaims(result.Value);

                // 🔐 Optional but recommended: check expiration
                var exp = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                if (exp != null &&
                    DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)) < DateTimeOffset.UtcNow)
                {
                    await storage.DeleteAsync("authToken");
                    return new AuthenticationState(Anonymous);
                }

                var identity = new ClaimsIdentity(claims, "jwt");


                // ✅ Rebuild session from claims (THIS fixes refresh issue)

                user.FullName =
                    claims.FirstOrDefault(c => c.Type == "FullName")?.Value;

                user.Username =
                    claims.FirstOrDefault(c => c.Type == "Username")?.Value;

                user.Role =
                    claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                user.LocationKey =
                    claims.FirstOrDefault(c => c.Type == "LocationKey")?.Value;

                user.CurrenyKey =
                    claims.FirstOrDefault(c => c.Type == "CurrencyKey")?.Value;

                user.CurrencyType =
                    claims.FirstOrDefault(c => c.Type == "CurrencyType")?.Value;

                user.Address =
                    claims.FirstOrDefault(c => c.Type == "Address")?.Value;

                user.IsAuthenticated = true;


                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch
            {
                // Token tampered with or unreadable
                await storage.DeleteAsync("authToken");
                return new AuthenticationState(Anonymous);
            }
        }

        public async Task SignInAsync(string token)
        {
            await storage.SetAsync("authToken", token);

            var identity = new ClaimsIdentity(ParseClaims(token), "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user)));
        }

        public async Task SignOutAsync()
        {
            await storage.DeleteAsync("authToken");

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(Anonymous)));
        }

        private static IEnumerable<Claim> ParseClaims(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }
    }
}
