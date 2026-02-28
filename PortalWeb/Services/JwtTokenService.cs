using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PortalWeb.Models;

namespace PortalWeb.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserModel user, string currencyType, string address, IEnumerable<string> permissions);
    }

    public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
    {
        private readonly IConfiguration configuration = configuration;

        public string GenerateToken(UserModel user, string currencyType, string address, IEnumerable<string> permissions)
        {
            var claims = new List<Claim>
        {
            new("UserKey", user.flngUserKey.ToString()),
            new("FullName", user.fstrFirstname! + " " + user.fstrLastname!),
            new("Email", user.fstrEmailAddress!),
            new("Role", MemoryStoredData.GetRoles().FirstOrDefault(r => r.flngRoleKey == user.flngRoleKey)!.fstrRoleName!),
            new("Username", user.fstrUsername!),
            new("LocationKey", user.fstrLocationKey!),
            new("CurrencyKey", user.fstrCurrencyKey!),
            new("CurrencyType", currencyType),
            new("Address", address)
        };

            foreach (var item in permissions)
            {
                var permission = MemoryStoredData
                                 .GetPermissions()
                                 .FirstOrDefault(p => p.flngPermissionKey.ToString() == item)!.fstrPermission;
                claims.Add(new Claim("permission", permission!));
            }


            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

