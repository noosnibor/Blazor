using PortalWeb.Dto;
using System.Security.Claims;

namespace PortalWeb.Services;

public interface IAuthenticate
{
    Task<(bool, string)> ValidateCredentials(LoginDto model);
}

public class Authenticate(IUserService user,
                  ICurrencyService currency,
                  IPermissionService permission,
                  ILocationService location,
                  UserSession session) : IAuthenticate
{
    private readonly IUserService user = user;
    private readonly IPermissionService permission = permission;
    private readonly ICurrencyService currency = currency;
    private readonly ILocationService location = location;
    private readonly UserSession session = session;

    public async Task<(bool, string)> ValidateCredentials(LoginDto model)
    {
        var user = await this.user.SelectUserFirstOrDefault(model.Username, null);

        if (user == null || string.IsNullOrWhiteSpace(user.fstrUsername))
            return (false, "Username does not exist in the system / account is not active");

        if (user.fdtmEnd < DateTime.Now)
            return (false, "Your account has expired");

        if (string.IsNullOrWhiteSpace(user.fstrPassword))
            return (false, "User password is not configured. Please contact administrator.");

        if (model.Password == MemoryStoredData.DefaultPassword && user.fblnPasswordChanged == false)
            return (false, "Change Password");

        if (!BCrypt.Net.BCrypt.Verify(model.Password!.Trim(), user.fstrPassword.Trim()))
            return (false, "Password verification failed.");

        var permissions = await permission
               .SelectPermissionByUsername(user.fstrUsername, user.fstrLocationKey!);

        permissions ??= [];

        var currency = await this.currency
            .SelectCurrencyFirstOrDefault(user.fstrCurrencyKey!);

        if (currency == null || string.IsNullOrWhiteSpace(currency.fstrCurrencyType))
            return (false, "Currency configuration missing.");

        var location = await this.location
            .SelectLocationFirstOrDefault(user.fstrLocationKey!);

        if (location == null || string.IsNullOrWhiteSpace(location.fstrLocationAddress))
            return (false, "Location configuration missing.");

        var claims = new List<Claim>
        {
            new("UserKey", user.flngUserKey.ToString()),
            new("FullName", user.fstrFirstname! + " " + user.fstrLastname!),
            new("Email", user.fstrEmailAddress!),
            new("Role", MemoryStoredData.GetRoles().FirstOrDefault(r => r.flngRoleKey == user.flngRoleKey)!.fstrRoleName!),
            new("Username", user.fstrUsername!),
            new("LocationKey", user.fstrLocationKey!),
            new("CurrencyKey", user.fstrCurrencyKey!),
            new("CurrencyType", currency.fstrCurrencyType),
            new("Address", location.fstrLocationAddress)
        };

        foreach (var item in permissions)
        {
            var permission = MemoryStoredData
                             .GetPermissions()
                             .FirstOrDefault(p => p.flngPermissionKey.ToString() == item)!.fstrPermission;
            claims.Add(new Claim("permission", permission!));
        }

        var identity = new ClaimsIdentity(claims, "Custom");

        var principal = new ClaimsPrincipal(identity);


        if (string.IsNullOrEmpty(session.Username))
        {
            session!.FullName = $"{user.fstrFirstname ?? ""} {user.fstrLastname ?? ""}".Trim();
            session.Username = user.fstrUsername;
            session.Role = MemoryStoredData.GetRoles()?
                .FirstOrDefault(r => r.flngRoleKey == user.flngRoleKey)?.fstrRoleName;

            session.LocationKey = user.fstrLocationKey;
            session.CurrenyKey = user.fstrCurrencyKey;
            session.Address = location.fstrLocationAddress;
            session.CurrencyType = currency.fstrCurrencyType;
            session.IsAuthenticated = true;
            session.Principal = principal;
        }

        return (true, "Successfully logged in!");



    }
}
