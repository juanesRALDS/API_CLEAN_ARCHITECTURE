using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;

public class PotentialClientExcelServices : IPotentialClientExcelServices
{
    private readonly IPotentialClientRepository _repository;
    private static readonly string[] Headers = new[]
    {
        "Tipo Identificación", "Número Identificación","Nombre representate legal", "Nombre Comercial",
        "Actividad Económica", "Email", "Teléfono", "Estado Actual", "Fecha Creación",
        "Última Actualización"
    };

    public PotentialClientExcelServices(IPotentialClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> ExportToExcelPotencialClient(CancellationToken cancellationToken)
    {
        try
        {
            (IEnumerable<PotentialClient> clients, int _) = await _repository.GetAllForExcel(1, int.MaxValue, cancellationToken);
            MemoryStream? stream = new MemoryStream();

            using (SpreadsheetDocument? spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart? workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart? worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                SheetData? sheetData = new();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                 Sheets? sheets = spreadsheetDocument.WorkbookPart!.Workbook.AppendChild(new Sheets());
                Sheet? sheet = new()
                {
                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Clientes Potenciales"
                };
                sheets.Append(sheet);

                AddHeaders(sheetData);
                await AddClientsData(sheetData, clients, cancellationToken);

                workbookPart.Workbook.Save();
            }

            stream.Position = 0;
            return stream.ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al exportar a Excel: {ex.Message}", ex);
        }
    }

    private void AddHeaders(SheetData sheetData)
    {
        Row? headerRow = new();
        foreach (string header in Headers)
        {
            headerRow.Append(CreateCell(header));
        }
        sheetData.Append(headerRow);
    }

    private Task AddClientsData(SheetData sheetData, IEnumerable<PotentialClient> clients, 
        CancellationToken cancellationToken)
    {
        foreach (PotentialClient? client in clients)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            Row? row = new Row();
            row.Append(GetClientCells(client));
            sheetData.Append(row);
        }
        return Task.CompletedTask;
    }

    private IEnumerable<Cell> GetClientCells(PotentialClient client)
    {
        return new[]
        {
            CreateCell(client.Identification.Type),
            CreateCell(client.Identification.Number),
            CreateCell(client.LegalRepresentative),
            CreateCell(client.BusinessInfo.TradeName),
            CreateCell(client.BusinessInfo.EconomicActivity),
            CreateCell(client.BusinessInfo.Email),
            CreateCell(client.BusinessInfo.Phone),
            CreateCell(client.Status.Current),
            CreateCell(client.CreatedAt.ToString("dd/MM/yyyy HH:mm")),
            CreateCell(client.UpdatedAt.ToString("dd/MM/yyyy HH:mm"))
        };
    }

    private Cell CreateCell(string value)
    {
        return new Cell()
        {
            DataType = CellValues.String,
            CellValue = new CellValue(string.IsNullOrEmpty(value) ? "-" : value)
        };
    }
}