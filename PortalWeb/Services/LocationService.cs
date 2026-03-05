using System.Data;
using Custom.Toast.Models;
using Dapper;
using PortalWeb.Dto;
using PortalWeb.Models;
using SqlDataAccess;

namespace PortalWeb.Services;

public interface ILocationService
{
    Task<ToastMessage> AddOrUpdateLocation(LocationDto model, bool insert = true);
    Task<IReadOnlyList<LocationModel>> SelectLocation(string? key = null, bool? active = null);
    Task<LocationModel?> SelectLocationFirstOrDefault(string key);
}

public class LocationService(ISqlDataAccess sqlDataAccess) : ILocationService
{
    private readonly ISqlDataAccess sqlDataAccess = sqlDataAccess;

    public async Task<IReadOnlyList<LocationModel>> SelectLocation(string? key = null!,
                                                                   bool? active = null!)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@pstrLocationKey", key);
        parameters.Add("@pblnActive", active);

        var result = await sqlDataAccess.QueryAsync<LocationModel>("dbo.spSelectLocation",
                                                                   CommandType.StoredProcedure,
                                                                   parameters);

        return result?.AsList() ?? [];
    }

    public async Task<LocationModel?> SelectLocationFirstOrDefault(string key)
    {
        var locations = await SelectLocation(key);
        return locations.Count == 0 ? null : locations[0];
    }

    public async Task<ToastMessage> AddOrUpdateLocation(LocationDto model,
                                                        bool insert = true)
    {
        if (model == null)
            return new() { Type = ToastType.Danger, Message = "Invalid"};

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@pstrLocationKey", model.LocationKey);
            parameters.Add("@pstrLocationName", model.LocationName);
            parameters.Add("@pstrLocationAddress", model.LocationAddress);
            parameters.Add("@pblnActive", model.Active);
            parameters.Add("@pstrWho", model.Who);
            parameters.Add("@pdtmWhen", model.When);

            var procedure = insert
            ? "dbo.spAddLocation"
            : "dbo.spUpdateLocation";

            var result = await sqlDataAccess.ExecuteAsync(
            procedure,
            CommandType.StoredProcedure,
            parameters);


            if (result == 1)
            {
                return insert
                    ? new() { Type = ToastType.Success, Message = "Location was added successfully" }
                    : new() { Type = ToastType.Success, Message = "Location was updated successfully" };
            }

            return insert
                    ? new() { Type = ToastType.Warning, Message = "Unable to add Location to the system" }
                    : new() { Type = ToastType.Warning, Message = "Unable to update the location in the system" }; 

            
        }
        catch (Exception ex)
        {
            return new() { Type = ToastType.Danger, Message = ex.Message };
        }
    }
}
