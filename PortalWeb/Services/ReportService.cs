using System.Data;
using Dapper;
using PortalWeb.Dto;
using PortalWeb.Models;
using SqlDataAccess;

namespace PortalWeb.Services;

public interface IReportService
{
    Task<IReadOnlyList<DetailReportModel>> DetailReport(DetailReportDto parameters);
    Task<CollectionTypeSummary> GenerateCollectionType(SummaryReportDto parameters);
    Task<MonthlySummary> GenerateMonthlySummaryByCollectionType(SummaryReportDto parameters);
    Task<MonthlySummary> GenerateMonthlySummaryByLocation(SummaryReportDto parameters);
    Task<MonthlySummary> GenerateMonthlySummaryByPaymentType(SummaryReportDto parameters);
    Task<PaymentTypeSummary> GeneratePaymentType(SummaryReportDto parameters);
}

public class ReportService(ISqlDataAccess sqlDataAccess, ILocationService locationService) : IReportService
{
    private readonly ISqlDataAccess sqlDataAccess = sqlDataAccess;
    private readonly ILocationService locationService = locationService;

    public async Task<IReadOnlyList<DetailReportModel>> DetailReport(DetailReportDto parameters)
    {

        // Construct a dynamic parameter container
        var col = new DynamicParameters();

        // Build parameter container
        col.Add("@pstrLocationKey", parameters.LocationKey);
        col.Add("@pdtmFrom", parameters.From, DbType.DateTime2);
        col.Add("@pdtmTo", parameters.To, DbType.DateTime2);
        col.Add("@pstrFirstname", parameters.Firstname);
        col.Add("@pstrLastname", parameters.Lastname);
        col.Add("@plngCollectionTypeKey", parameters.CollectionTypeKey);
        col.Add("@plngPaymentTypeKey", parameters.PaymentTypeKey);

        var result = await sqlDataAccess.QueryAsync<DetailReportModel>("dbo.spDetailReport",
                                                                   CommandType.StoredProcedure,
                                                                   col);

        return result?.AsList() ?? [];
    }

    private async Task<IEnumerable<SummaryReportModel>> SummaryReport(SummaryReportDto parameters)
    {
        var col = new DynamicParameters();

        // Build parameter container
        col.Add("@pstrLocationKey", parameters.LocationKey);
        col.Add("@pdtmTransactionDateFrom", parameters.TransactionDateFrom, DbType.DateTime2);
        col.Add("@pdtmTransactionDateTo", parameters.TransactionDateTo, DbType.DateTime2);

        var result = await sqlDataAccess.QueryAsync<SummaryReportModel>("dbo.spSummaryReport", CommandType.StoredProcedure, col);

        return result?.AsList() ?? [];
    }

    private static IEnumerable<string> FormatMonths(DateTime? from, DateTime? to)
    {
        return [.. Enumerable.Range(0,
            ((to!.Value.Year - from!.Value.Year) * 12) + to!.Value.Month - from!.Value.Month + 1)
            .Select(i => from!.Value.AddMonths(i))
            .Select(d => d.ToString("MMM-yyyy"))];
    }

    private static string SelectedMonth(DateTime? date) => date!.Value.ToString("MMM-yyyy");

    public async Task<PaymentTypeSummary> GeneratePaymentType(SummaryReportDto parameters)
    {
        var results = await SummaryReport(parameters);

        if (!results.Any())
            return new PaymentTypeSummary();


        var currencies = new HashSet<string>();
        var paymentTypes = new HashSet<int>();

        var cellTotals = new Dictionary<(int, string), decimal>();
        var rowTotals = new Dictionary<int, decimal>();
        var columnTotals = new Dictionary<string, decimal>();

        decimal grandTotal = 0;

        foreach (var r in results)
        {
            if (string.IsNullOrEmpty(r.fstrCurrencyKey))
                continue;

            var currency = r.fstrCurrencyKey;
            var paymentType = r.flngPaymentTypeKey;
            var localAmount = r.fcurLocalAmount;
            var amount = r.fcurAmount;

            currencies.Add(currency);
            paymentTypes.Add(paymentType);

            // CELL TOTAL
            var cellKey = (paymentType, currency);
            cellTotals[cellKey] = cellTotals.TryGetValue(cellKey, out var cVal)
                ? cVal + amount
                : amount;

            // ROW TOTAL
            rowTotals[paymentType] = rowTotals.TryGetValue(paymentType, out var rVal)
                ? rVal + localAmount
                : localAmount;

            // COLUMN TOTAL
            columnTotals[currency] = columnTotals.TryGetValue(currency, out var colVal)
                ? colVal + localAmount
                : localAmount;

            // GRAND TOTAL
            grandTotal += localAmount;
        }

        return new PaymentTypeSummary
        {
            Currencies = [.. currencies.OrderBy(x => x)],
            PaymentTypes = [.. paymentTypes.OrderBy(x => x)],
            CellTotals = cellTotals,
            RowTotals = rowTotals,
            ColumnTotals = columnTotals,
            GrandTotal = grandTotal,

        };
    }

    public async Task<MonthlySummary> GenerateMonthlySummaryByPaymentType(SummaryReportDto parameters)
    {
        var results = await SummaryReport(parameters);

        if (!results.Any())
            return new MonthlySummary();

        var months = FormatMonths(
            parameters.TransactionDateFrom,
            parameters.TransactionDateTo
        ).ToList();

        // Cache collection types once (avoid repeated FirstOrDefault calls)
        var paymentTypes = MemoryStoredData.GetPaymentType()
            .ToDictionary(x => x.flngPaymentKey, x => x.fstrPaymentType);

        var data = results
            .GroupBy(t => t.flngPaymentTypeKey)
            .Select(g =>
            {
                var paymentName = paymentTypes.TryGetValue(g.Key, out var name)
                 ? name
                 : "Unknown";

                return new MonthModel
                {
                    Id = paymentName!,
                    Amount = months.ToDictionary(
                        month => month,
                        month => g
                            .Where(x => SelectedMonth(x.fdtmTransactionDate) == month)
                            .Sum(x => x.fcurLocalAmount)
                    )
                };
            })
            .ToList();

        return new MonthlySummary
        {
            Months = months,
            Data = data
        };
    }

    public async Task<CollectionTypeSummary> GenerateCollectionType(SummaryReportDto parameters)
    {
        var results = await SummaryReport(parameters);

        if (!results.Any())
            return new CollectionTypeSummary();


        var currencies = new HashSet<string>();
        var collectionType = new HashSet<int>();

        var cellTotals = new Dictionary<(int, string), decimal>();
        var rowTotals = new Dictionary<int, decimal>();
        var columnTotals = new Dictionary<string, decimal>();

        decimal grandTotal = 0;

        foreach (var r in results)
        {
            if (string.IsNullOrEmpty(r.fstrCurrencyKey))
                continue;

            var currency = r.fstrCurrencyKey;
            var collectType = r.flngCollectionTypeKey;
            var localAmount = r.fcurLocalAmount;
            var amount = r.fcurAmount;

            currencies.Add(currency);
            collectionType.Add(collectType);

            // CELL TOTAL
            var cellKey = (collectType, currency);
            cellTotals[cellKey] = cellTotals.TryGetValue(cellKey, out var cVal)
                ? cVal + amount
                : amount;

            // ROW TOTAL
            rowTotals[collectType] = rowTotals.TryGetValue(collectType, out var rVal)
                ? rVal + localAmount
                : localAmount;

            // COLUMN TOTAL
            columnTotals[currency] = columnTotals.TryGetValue(currency, out var colVal)
                ? colVal + localAmount
                : localAmount;

            // GRAND TOTAL
            grandTotal += localAmount;
        }

        return new CollectionTypeSummary
        {
            Currencies = [.. currencies.OrderBy(x => x)],
            CollectionTypes = [.. collectionType.OrderBy(x => x)],
            CellTotals = cellTotals,
            RowTotals = rowTotals,
            ColumnTotals = columnTotals,
            GrandTotal = grandTotal,

        };
    }

    public async Task<MonthlySummary> GenerateMonthlySummaryByCollectionType(SummaryReportDto parameters)
    {
        var results = await SummaryReport(parameters);

        if (!results.Any())
            return new MonthlySummary();

        var months = FormatMonths(
            parameters.TransactionDateFrom,
            parameters.TransactionDateTo
        ).ToList();

        // Cache collection types once (avoid repeated FirstOrDefault calls)
        var collectType = MemoryStoredData.GetCollectionType()
            .ToDictionary(x => x.flngCollectionTypeKey, x => x.fstrCollectionType);

        var data = results
            .GroupBy(t => t.flngCollectionTypeKey)
            .Select(g =>
            {
                var collectionName = collectType.TryGetValue(g.Key, out var name)
                 ? name
                 : "Unknown";

                return new MonthModel
                {
                    Id = collectionName!,
                    Amount = months.ToDictionary(
                        month => month,
                        month => g
                            .Where(x => SelectedMonth(x.fdtmTransactionDate) == month)
                            .Sum(x => x.fcurLocalAmount)
                    )
                };
            })
            .ToList();

        return new MonthlySummary
        {
            Months = months,
            Data = data
        };
    }

    public async Task<MonthlySummary> GenerateMonthlySummaryByLocation(SummaryReportDto parameters)
    {
        var results = await SummaryReport(parameters);

        if (!results.Any())
            return new MonthlySummary();

        var months = FormatMonths(
            parameters.TransactionDateFrom,
            parameters.TransactionDateTo
        ).ToList();

        // Cache collection types once (avoid repeated FirstOrDefault calls)
        var location = await locationService.SelectLocation(null, true);
        var locationDictionary = location.ToDictionary(l => l.fstrLocationKey!, l => l.fstrLocationName);

        var data = results
            .GroupBy(t => t.fstrLocationKey)
            .Select(g =>
            {
                var locationName = locationDictionary.TryGetValue(g.Key!, out var name)
                 ? name
                 : "Unknown";

                return new MonthModel
                {
                    Id = locationName!,
                    Amount = months.ToDictionary(
                        month => month,
                        month => g
                            .Where(x => SelectedMonth(x.fdtmTransactionDate) == month)
                            .Sum(x => x.fcurLocalAmount)
                    )
                };
            })
            .ToList();

        return new MonthlySummary
        {
            Months = months,
            Data = data
        };
    }
}
