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
        // Get user by username
        var user = await this.user.SelectUserFirstOrDefault(model.Username!, null);

        // Can't find username
        if (user?.fstrUsername is null)
            return (false, "Username does not exists in the system / account is not active");

        // Check if the account has expired
        if (user.fdtmEnd < DateTime.Now)
            return (false, "Your account has expired");

        // Check for default password
        if (model.Password! == MemoryStoredData.DefaultPassword && user.fblnPasswordChanged == false)
        {
            return (false, "Change Password");
        }

        if (!BCrypt.Net.BCrypt.Verify(model.Password!, user.fstrPassword))
            return (false, $"The password is incorrect for user {user.fstrUsername}");

        // Get user permissions
        var permissions = await permission.SelectPermissionByUsername(user.fstrUsername!, user.fstrLocationKey!);

        var currency = await this.currency.SelectCurrencyFirstOrDefault(user.fstrCurrencyKey!);
        var r = currency?.fstrCurrencyType;

        // Get location address
        var location = await this.location.SelectLocationFirstOrDefault(user.fstrLocationKey!);

        // Generate token
        var token = _jwt.GenerateToken(user, r!, location!.fstrLocationAddress!, permissions);

        await _storage.SetAsync("authToken", token);

        // Notify user state
        await _auth.SignInAsync(token);

        _session.FullName = user.fstrFirstname + " " + user.fstrLastname;

        _session.Username = user.fstrUsername;

        _session.Role = MemoryStoredData.GetRoles().FirstOrDefault(r => r.flngRoleKey == user.flngRoleKey)!.fstrRoleName;

        _session.LocationKey = user.fstrLocationKey;

        _session.CurrenyKey = user.fstrCurrencyKey;

        _session.Address = location.fstrLocationAddress;

        _session.CurrencyType = r;

        _session.IsAuthenticated = true;

        return (true, "Successfully logged in!");
    }

    public async Task LogoutAsync()
    {
        await _storage.DeleteAsync("authToken");
        await _auth.SignOutAsync();
    }
}
