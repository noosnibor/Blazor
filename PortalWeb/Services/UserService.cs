using System.Data;
using Custom.Toast.Models;
using Dapper;
using PortalWeb.Dto;
using PortalWeb.Models;
using SqlDataAccess;

namespace PortalWeb.Services;

public interface IUserService
{
    Task<ToastMessage> AddOrUpdateUser(UserDto model, bool insert = true);
    Task<IReadOnlyList<UserModel>> SelectUser(string? username, string? LocationKey, bool? active = true);
    Task<UserModel?> SelectUserFirstOrDefault(string? username, string? locationKey);
    Task<ToastMessage> UpdatePasswordAsync(string username, string password);
}

public class UserService(ISqlDataAccess sqlDataAccess) : IUserService
{
    private readonly ISqlDataAccess sqlDataAccess = sqlDataAccess;

    public async Task<IReadOnlyList<UserModel>> SelectUser(string? username,
                                                           string? LocationKey,
                                                           bool? active = true)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@pstrUsername", username);
        parameters.Add("@pstrLocationKey", LocationKey);
        parameters.Add("@pblnActive", active);


        var result = await sqlDataAccess.QueryAsync<UserModel>("dbo.spSelectUser",
                                                                   CommandType.StoredProcedure,
                                                                   parameters);

        return result?.AsList() ?? [];
    }

    public async Task<UserModel?> SelectUserFirstOrDefault(string? username, string? locationKey)
    {
        var user = await SelectUser(username, locationKey);
        return user.Count == 0 ? null : user[0];
    }

    public async Task<ToastMessage> AddOrUpdateUser(UserDto model,
                                                          bool insert = true)
    {
        if (model == null)
            return new() { Type = ToastType.Danger, Message = "Invalid" };

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@plngUserKey", model.flngUserKey);
            parameters.Add("@pstrUsername", model.fstrUsername);
            parameters.Add("@pstrPassword", model.fstrPassword);
            parameters.Add("@pstrFirstname", model.fstrFirstname);
            parameters.Add("@pstrLastname", model.fstrLastname);
            parameters.Add("@pstrEmailAddress", model.fstrEmailAddress);
            parameters.Add("@pstrLocationKey", model.fstrLocationKey);
            parameters.Add("@plngRoleKey", model.flngRoleKey);
            parameters.Add("@pstrCurrencyKey", model.fstrCurrencyKey);
            parameters.Add("@pblnActive", model.fblnActive);
            parameters.Add("@pblnPasswordChanged", model.fblnPasswordChanged);
            parameters.Add("@pdtmStart", model.fdtmStart);
            parameters.Add("@pdtmEnd", model.fdtmEnd);
            parameters.Add("@pstrWho", model.fstrWho);
            parameters.Add("@pdtmWhen", model.fdtmWhen);



            var procedure = insert
            ? "dbo.spAddUser"
            : "dbo.spUpdateUser";

            var result = await sqlDataAccess.ExecuteAsync(
            procedure,
            CommandType.StoredProcedure,
            parameters);


            if (result == 1)
            {
                return insert
                    ? new() { Type = ToastType.Success, Message = "User was added successfully" }
                    : new() { Type = ToastType.Success, Message = "User was updated successfully" };
            }

            return insert
                    ? new() { Type = ToastType.Warning, Message = "Unable to add user to the system" }
                    : new() { Type = ToastType.Warning, Message = "Unable to update the user in the system" };


        }
        catch (Exception ex)
        {
            return new() { Type = ToastType.Danger, Message = ex.Message };
        }
    }

    public async Task<ToastMessage> UpdatePasswordAsync(string username, string password)
    {
        // Check the model to ensure data is available
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return new() { Type = ToastType.Danger, Message = "Invalid" };

        try
        {
            var parameters = new DynamicParameters();

            // Build parameter
            parameters.Add("@pstrUsername", username);
            parameters.Add("@pstrPassword", password);
            parameters.Add("@pblnPasswordChanged", true);

            // Execute stored procedure and return result
            int result = await sqlDataAccess.ExecuteAsync("dbo.spUpdateUserPassword", CommandType.StoredProcedure, parameters);

            // Validate result
            if (result == 1)
                return new() { Type = ToastType.Success, Message = "User password was updated successfully" };
            else
                return new() { Type = ToastType.Success, Message = "An error as occurred while updating password" };

        }
        catch (Exception ex)
        {
            // Unexpected errors
            return new() { Type = ToastType.Success, Message = "Unexpected error: " + ex.Message };
         
        }
    }
}
