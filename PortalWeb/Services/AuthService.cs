using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PortalWeb.Dto;

namespace PortalWeb.Services;

public interface IAuthService
{
    Task<(bool, string)> LoginAsync(LoginDto model);
    Task LogoutAsync();
}

public class AuthService(IUserService user,
                         ICurrencyService currency,
                         IPermissionService permission,
                         ILocationService location,
                         IJwtTokenService jwt,
                         AuthenticationStateProvider auth,
                         ProtectedSessionStorage storage,
                         UserSession session) : IAuthService
{
    private readonly IUserService user = user;
    private readonly IPermissionService permission = permission;
    private readonly IJwtTokenService _jwt = jwt;
    private readonly CustomAuthStateProvider _auth = (CustomAuthStateProvider)auth;
    private readonly ProtectedSessionStorage _storage = storage;
    private readonly UserSession _session = session;
    private readonly ICurrencyService currency = currency;
    private readonly ILocationService location = location;


    public async Task<(bool, string)> LoginAsync(LoginDto model)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(model.Username))
                return (false, "Username is required");

            if (string.IsNullOrWhiteSpace(model.Password))
                return (false, "Password is required");

            var user = await this.user.SelectUserFirstOrDefault(model.Username, null);

            if (user == null || string.IsNullOrWhiteSpace(user.fstrUsername))
                return (false, "Username does not exist in the system / account is not active");

            if (user.fdtmEnd < DateTime.Now)
                return (false, "Your account has expired");

            if (string.IsNullOrWhiteSpace(user.fstrPassword))
                return (false, "User password is not configured. Please contact administrator.");

            if (model.Password == MemoryStoredData.DefaultPassword && user.fblnPasswordChanged == false)
                return (false, "Change Password");

            //if (model.Password != MemoryStoredData.DefaultPassword)
            //    return (false, "Password invalid");

            var passwordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.fstrPassword);

            if (!passwordValid)
                return (false, $"The password is incorrect for user {user.fstrUsername}");

            var permissions = await permission
                .SelectPermissionByUsername(user.fstrUsername, user.fstrLocationKey!);

            var currency = await this.currency
                .SelectCurrencyFirstOrDefault(user.fstrCurrencyKey!);

            if (currency == null)
                return (false, "Currency configuration missing.");

            var location = await this.location
                .SelectLocationFirstOrDefault(user.fstrLocationKey!);

            if (location == null)
                return (false, "Location configuration missing.");

            var token = _jwt.GenerateToken(
                user,
                currency.fstrCurrencyType!,
                location.fstrLocationAddress!,
                permissions
            );

            await _storage.SetAsync("authToken", token);
            await _auth.SignInAsync(token);

            _session.FullName = user.fstrFirstname + " " + user.fstrLastname;
            _session.Username = user.fstrUsername;
            _session.Role = MemoryStoredData.GetRoles()
                .FirstOrDefault(r => r.flngRoleKey == user.flngRoleKey)?.fstrRoleName;

            _session.LocationKey = user.fstrLocationKey;
            _session.CurrenyKey = user.fstrCurrencyKey;
            _session.Address = location.fstrLocationAddress;
            _session.CurrencyType = currency.fstrCurrencyType;
            _session.IsAuthenticated = true;

            return (true, "Successfully logged in!");
        }
        catch (Exception ex)
        {
            return (true, ex.Message);
        }
    }

    public async Task LogoutAsync()
    {
        await _storage.DeleteAsync("authToken");
        await _auth.SignOutAsync();
    }
}
