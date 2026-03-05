using System.Data;
using Custom.Toast.Models;
using Dapper;
using PortalWeb.Models;
using SqlDataAccess;

namespace PortalWeb.Services;

public interface IPermissionService
{
    Task<ToastMessage> CreatePermission(List<PermissionModel> model);
    Task<ToastMessage> DeletePermission(int flngPermissionKey, int flngRoleKey);
    Task<List<PermissionModel>> SelectPermission(int? permissionKey = null, int? roleKey = null);
    Task<PermissionModel?> SelectPermissionFirstOrDefault(int? permissionKey = null, int? roleKey = null);
    Task<List<string>> SelectPermissionByUsername(string username, string locationKey);
}

public class PermissionService(ISqlDataAccess sqlDataAccess) : IPermissionService
{
    private readonly ISqlDataAccess sqlDataAccess = sqlDataAccess;

    public async Task<List<PermissionModel>> SelectPermission(int? permissionKey = null, int? roleKey = null)
    {
        var parameters = new DynamicParameters();

        // Build parameter
        parameters.Add("@plngPermissionKey", permissionKey);
        parameters.Add("@plngRoleKey", roleKey);

        // Get results
        var result = await sqlDataAccess.QueryAsync<PermissionModel>("dbo.spSelectPermission", CommandType.StoredProcedure, parameters);

        // Ensure we always return a list (never null)
        return result?.AsList() ?? [];
    }

    public async Task<List<string>> SelectPermissionByUsername(string username, string locationKey)
    {
        var parameters = new DynamicParameters();

        // Build parameter
        parameters.Add("@pstrUsername", username);
        parameters.Add("@pstrLocationKey", locationKey);

        // Get results
        var result = await sqlDataAccess.QueryAsync<string>("dbo.spSelectUserPermission", CommandType.StoredProcedure, parameters);

        // Ensure we always return a list (never null)
        return result?.AsList() ?? [];
    }

    public async Task<PermissionModel?> SelectPermissionFirstOrDefault(int? permissionKey = null, int? roleKey = null)
    {
        var permission = await SelectPermission(permissionKey, roleKey);
        return permission.Count == 0 ? null : permission[0];
    }

    public async Task<ToastMessage> CreatePermission(List<PermissionModel> model)
    {
        // Check the model to ensure data is available
        if (model == null)
            return new() { Type = ToastType.Danger, Message = "Invalid" };

        try
        {
            var table = new DataTable();
            table.Columns.Add("flngPermissionKey", typeof(int));
            table.Columns.Add("flngRoleKey", typeof(int));

            foreach (var r in model)
                table.Rows.Add(r.flngPermissionKey, r.flngRoleKey);


            var parameters = new DynamicParameters();
            parameters.Add("@Permission", table.AsTableValuedParameter("dbo.tempPermission"));

            int result = await sqlDataAccess.ExecuteAsync("dbo.spAddPermission", CommandType.StoredProcedure, parameters);

            // Validate result
            if (result == 1)
                return new() { Type = ToastType.Success, Message = "Permission was added successfully" };
            else
                return new() { Type = ToastType.Warning, Message = "Unable to add permission" };

        }
        catch (Exception ex)
        {
            // Unexpected errors
            return new() { Type = ToastType.Success, Message = ex.Message };
        }
    }

    public async Task<ToastMessage> DeletePermission(int flngPermissionKey, int flngRoleKey)
    {
        // Check the model to ensure data is available
        if (flngPermissionKey == 0 || flngRoleKey == 0)
            return new() { Type = ToastType.Danger, Message = "Invalid" };

        try
        {
            var parameters = new DynamicParameters();

            // Build parameter
            parameters.Add("@plngPermissionKey", flngPermissionKey);
            parameters.Add("@plngRoleKey", flngRoleKey);


            // Execute stored procedure and return result
            int result = await sqlDataAccess.ExecuteAsync("dbo.spDeletePermission", CommandType.StoredProcedure, parameters);

            // Validate result
            if (result == 1)
                return new() { Type = ToastType.Success, Message = "Permission was deleted successfully" };
            else
                return new() { Type = ToastType.Warning, Message = "Unable to delete permission" };

        }
        catch (Exception ex)
        {
            // Unexpected errors
            return new() { Type = ToastType.Danger, Message = ex.Message };
        }
    }

}
