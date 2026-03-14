using PortalWeb.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PortalWeb.Services;

public interface IGeneratePDFService
{
    byte[] GeneratePdf(PivotSummaryModel model, UserSession user, string locationName, string Note);
}

public class GeneratePDFService : IGeneratePDFService
{
    public byte[] GeneratePdf(PivotSummaryModel model, UserSession user, string locationName, string Note)
    {

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Spacing(4);
                        col.Item().Text(locationName).FontSize(22).Bold();
                        col.Item().Text("Collection Summary Report");
                        col.Item().Text(user.Address)
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2).LineHeight(1.2f);

                      
                        col.Item().PaddingTop(18).Text(user.FullName).FontSize(14).Bold();
                        col.Item().Text(user.Role).FontSize(10);
                    });

                  

                    row.RelativeItem().AlignRight().Column(col =>
                    {
                        col.Item().Text("SUMMARY").FontSize(28).Bold();
                        col.Item().Text($"Generated: {DateTime.Now:dd-MMM-yyyy}");
                    });
                });


                page.Content().PaddingVertical(20).Column(column =>
                {
                    column.Item().Element(container =>
                    {
                        BuildPivotTable(container, model);
                    });

                    // NOTE SECTION
                    column.Item().PaddingTop(15).Text(text =>
                    {
                        text.Span("Note: ").Bold();
                        text.Span(Note);
                    });
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });
            });
        })
         .GeneratePdf();
    }

    private void BuildPivotTable(IContainer container, PivotSummaryModel model)
    {
        container.Table(table =>
        {
            int columnCount = model.Columns.Count + 2;

            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(2);

                foreach (var _ in model.Columns)
                    columns.RelativeColumn();

                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Background("#e5e7eb").Padding(5).Text("Type").Bold();

                foreach (var col in model.Columns)
                    header.Cell().Background("#e5e7eb").Padding(5).AlignRight().Text(col).Bold();

                header.Cell().Background("#e5e7eb").Padding(5).AlignRight().Text("Total").Bold();
            });

            foreach (var row in model.Rows)
            {
                table.Cell().Padding(5).Text(row);

                foreach (var column in model.Columns)
                {
                    var key = (row, column);

                    decimal value = model.CellTotals.ContainsKey(key)
                        ? model.CellTotals[key]
                        : 0;

                    table.Cell()
                        .Padding(5)
                        .AlignRight()
                        .Text(value.ToString("N2"));
                }

                decimal rowTotal = model.RowTotals.ContainsKey(row)
                    ? model.RowTotals[row]
                    : 0;

                table.Cell()
                    .Padding(5)
                    .AlignRight()
                    .Text(rowTotal.ToString("N2"));
            }

            table.Cell()
                .Background("#f3f4f6")
                .Padding(5)
                .Text("Total")
                .Bold();

            foreach (var column in model.Columns)
            {
                decimal colTotal = model.ColumnTotals.ContainsKey(column)
                    ? model.ColumnTotals[column]
                    : 0;

                table.Cell()
                    .Background("#f3f4f6")
                    .Padding(5)
                    .AlignRight()
                    .Text(colTotal.ToString("N2"))
                    .Bold();
            }

            table.Cell()
                .Background("#f3f4f6")
                .Padding(5)
                .AlignRight()
                .Text(model.GrandTotal.ToString("N2"))
                .Bold();
        });
    }
}
