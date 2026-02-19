using System.Data;
using Custom.Toast.Models;
using Dapper;
using PortalWeb.Dto;
using PortalWeb.Models;
using SqlDataAccess;

namespace PortalWeb.Services;

public interface ICurrencyService
{
    Task<ToastMessage> AddOrUpdateCurrency(CurrencyDto model, bool insert = true);
    Task<IReadOnlyList<CurrencyModel>> SelectCurrency(string? Currencykey = null, string? locationKey = null, bool? active = null);
    Task<CurrencyModel?> SelectCurrencyFirstOrDefault(string key);
}

public class CurrencyService(ISqlDataAccess sqlDataAccess) : ICurrencyService
{
    private readonly ISqlDataAccess sqlDataAccess = sqlDataAccess;

    public async Task<IReadOnlyList<CurrencyModel>> SelectCurrency(string? Currencykey = null!,
                                                                   string? locationKey = null,
                                                                   bool? active = null!)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@pstrCurrencyKey", Currencykey);
        parameters.Add("@pstrLocationKey", locationKey);
        parameters.Add("@pblnActive", active);


        var result = await sqlDataAccess.QueryAsync<CurrencyModel>("dbo.spSelectCurrency",
                                                                   CommandType.StoredProcedure,
                                                                   parameters);

        return result?.AsList() ?? [];
    }

    public async Task<CurrencyModel?> SelectCurrencyFirstOrDefault(string key)
    {
        var currency = await SelectCurrency(key);
        return currency.Count == 0 ? null : currency[0];
    }

    public async Task<ToastMessage> AddOrUpdateCurrency(CurrencyDto model,
                                                        bool insert = true)
    {
        if (model == null)
            return new() { Type = ToastType.Danger, Message = "Invalid" };

        var currency = await SelectCurrency(model.CurrencyKey, model.LocationKey, true);

        if (currency.Count != 0)
        {
            var date = currency.Max(d => d.fdtmEffectiveTo).Date;

            if (insert)
            {
                if (model.EffectiveFrom <= date)
                {
                    return new() { Type = ToastType.Warning, Message = "The effective date from can not be less than the previous effective to" };
                }
            }    

           
        }


        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@pstrCurrencyKey", model.CurrencyKey);
            parameters.Add("@pstrCurrencyType", model.CurrencyType);
            parameters.Add("@pstrLocationKey", model.LocationKey);
            parameters.Add("@pcurAmount", model.Amount);
            parameters.Add("@pblnActive", model.Active);
            parameters.Add("@pstrWho", model.Who);
            parameters.Add("@pdtmEffectiveFrom", model.EffectiveFrom);
            parameters.Add("@pdtmEffectiveTo", model.EffectiveTo);



            var procedure = insert
            ? "dbo.spAddCurrency"
            : "dbo.spUpdateCurrency";

            var result = await sqlDataAccess.ExecuteAsync(
            procedure,
            CommandType.StoredProcedure,
            parameters);


            if (result == 1)
            {
                return insert
                    ? new() { Type = ToastType.Success, Message = "Currency was added successfully" }
                    : new() { Type = ToastType.Success, Message = "Currency was updated successfully" };
            }

            return insert
                    ? new() { Type = ToastType.Warning, Message = "Unable to add Currency to the system" }
                    : new() { Type = ToastType.Warning, Message = "Unable to update the Currency in the system" };


        }
        catch (Exception ex)
        {
            return new() { Type = ToastType.Danger, Message = ex.Message };
        }
    }
}
