using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Infrastructure.Services;

public class PotentialClientExcelServices : IPotentialClientExcelServices
{
    private readonly IPotentialClientRepository _repository;
    private static readonly string[] Headers = new[]
    {
        "Tipo Identificación", "Número Identificación", "Nombre Comercial",
        "Actividad Económica", "Email", "Teléfono", "Dirección",
        "Ciudad", "Departamento", "Estado Actual", "Fecha Creación",
        "Última Actualización"
    };

    public PotentialClientExcelServices(IPotentialClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> ExportToExcel(CancellationToken cancellationToken)
    {
        var (clients, _) = await _repository.GetAllForExcel(1, int.MaxValue, cancellationToken);
        var stream = new MemoryStream();

        using var spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
        var workbookPart = spreadsheetDocument.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        var sheetData = new SheetData();
        worksheetPart.Worksheet = new Worksheet(sheetData);

        var sheets = spreadsheetDocument.WorkbookPart!.Workbook.AppendChild(new Sheets());
        sheets.AppendChild(new Sheet()
        {
            Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
            SheetId = 1,
            Name = "Clientes Potenciales"
        });

        AddHeaders(sheetData);
        await AddClientData(sheetData, clients, cancellationToken);

        workbookPart.Workbook.Save();
        stream.Position = 0;
        return stream.ToArray();
    }

    private void AddHeaders(SheetData sheetData)
    {
        var headerRow = new Row();
        foreach (string header in Headers)
        {
            headerRow.AppendChild(CreateCell(header));
        }
        sheetData.AppendChild(headerRow);
    }

    private Task AddClientData(SheetData sheetData, IEnumerable<PotentialClient> clients, 
        CancellationToken cancellationToken)
    {
        foreach (var client in clients)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            var row = new Row();
            row.Append(GetClientCells(client));
            sheetData.AppendChild(row);
        }
        return Task.CompletedTask;
    }

    private IEnumerable<Cell> GetClientCells(PotentialClient client) =>
        new[]
        {
            CreateCell(client.Identification.Type),
            CreateCell(client.Identification.Number),
            CreateCell(client.BusinessInfo.TradeName),
            CreateCell(client.BusinessInfo.EconomicActivity),
            CreateCell(client.BusinessInfo.Email),
            CreateCell(client.BusinessInfo.Phone),
            CreateCell(client.Location.Address),
            CreateCell(client.Location.City),
            CreateCell(client.Location.Department),
            CreateCell(client.Status.Current),
            CreateCell(client.CreatedAt.ToString("dd/MM/yyyy HH:mm")),
            CreateCell(client.UpdatedAt.ToString("dd/MM/yyyy HH:mm"))
        };

    private Cell CreateCell(string text) =>
        new()
        {
            DataType = CellValues.String,
            CellValue = new CellValue(string.IsNullOrEmpty(text) ? "-" : text)
        };
}