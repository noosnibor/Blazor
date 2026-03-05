using System.Data;
using Custom.Toast.Models;
using Dapper;
using PortalWeb.Dto;
using PortalWeb.Models;
using SqlDataAccess;

namespace PortalWeb.Services;

public interface ICollectionService
{
    Task<ToastMessage> AddOrUpdateCollection(CollectionDto model, bool insert = true);
    Task<IReadOnlyList<CollectionModel>> SelectCollection(string LocationKey, DateTime? transactionDate = null);
    Task<CollectionModel?> SelectCollectionFirstOrDefault(string key);
}

public class CollectionService(ISqlDataAccess sqlDataAccess) : ICollectionService
{
    private readonly ISqlDataAccess sqlDataAccess = sqlDataAccess;

    public async Task<IReadOnlyList<CollectionModel>> SelectCollection(string LocationKey,
                                                                       DateTime? transactionDate = null)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@pstrLocationKey", LocationKey);
        parameters.Add("@pdtmTransactionDate", transactionDate!.Value);



        var result = await sqlDataAccess.QueryAsync<CollectionModel>("dbo.spSelectCollection",
                                                                   CommandType.StoredProcedure,
                                                                   parameters);

        return result?.AsList() ?? [];
    }

    public async Task<CollectionModel?> SelectCollectionFirstOrDefault(string key)
    {
        var collection = await SelectCollection(key);
        return collection.Count == 0 ? null : collection[0];
    }

    public async Task<ToastMessage> AddOrUpdateCollection(CollectionDto model,
                                                      bool insert = true)
    {
        if (model == null)
            return new() { Type = ToastType.Danger, Message = "Invalid" };

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@plngCollectionKey", model.CollectionKey);
            parameters.Add("@pstrFirstname", model.Firstname);
            parameters.Add("@pstrLastname", model.Lastname);
            parameters.Add("@pstrEmailAddress", model.EmailAddress);
            parameters.Add("@pdtmTransactionDate", model.TransactionDate);
            parameters.Add("@plngCollectionNumber", model.CollectionNumber);
            parameters.Add("@plngMemberStatusKey", model.MemberStatusKey);
            parameters.Add("@plngCollectionTypeKey", model.CollectionTypeKey);
            parameters.Add("@plngPaymentTypeKey", model.PaymentTypeKey);
            parameters.Add("@pcurAmount", model.Amount);
            parameters.Add("@pcurLocalAmount", model.LocalAmount);
            parameters.Add("@pstrCurrencyKey", model.CurrencyKey);
            parameters.Add("@pstrLocationKey", model.LocationKey);
            parameters.Add("@pstrWho", model.Who);
            parameters.Add("@pdtmWhen", model.When);


            var procedure = insert
            ? "dbo.spAddCollection"
            : "dbo.spUpdateCollection";

            var result = await sqlDataAccess.ExecuteAsync(
            procedure,
            CommandType.StoredProcedure,
            parameters);


            if (result == 1)
            {
                return insert
                    ? new() { Type = ToastType.Success, Message = "Collection was added successfully" }
                    : new() { Type = ToastType.Success, Message = "Collection was updated successfully" };
            }

            return insert
                    ? new() { Type = ToastType.Warning, Message = "Unable to add Collection to the system" }
                    : new() { Type = ToastType.Warning, Message = "Unable to update the Collection in the system" };


        }
        catch (Exception ex)
        {
            return new() { Type = ToastType.Danger, Message = ex.Message };
        }
    }
}
