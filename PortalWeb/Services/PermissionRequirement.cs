using Microsoft.AspNetCore.Authorization;

namespace PortalWeb.Services;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
