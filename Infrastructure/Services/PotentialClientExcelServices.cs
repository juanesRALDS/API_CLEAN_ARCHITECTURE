using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Infrastructure.Services;

public class PotentialClientExcelService : IPotentialClientExcelService
{
    private readonly IPotentialClientRepository _repository;

    public PotentialClientExcelService(IPotentialClientRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<byte[]> ExportToExcel(CancellationToken cancellationToken)
    {
        IEnumerable<PotentialClient>? clients = await _repository.GetAllAsync(cancellationToken);

        using MemoryStream? memoryStream = new MemoryStream();
        using ( SpreadsheetDocument? spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        {
             WorkbookPart? workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            WorksheetPart? worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            SheetData? sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            Workbook? workbook = spreadsheetDocument.WorkbookPart?.Workbook ?? new Workbook();
            Sheets? sheets = workbook.AppendChild(new Sheets());

            Sheet? sheet = new()
            {
                Id = spreadsheetDocument.WorkbookPart!.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Potential Clients"
            };
            sheets.Append(sheet);

            // Headers
            Row? headerRow = new();
            headerRow.Append(
                CreateCell("Company Name"),
                CreateCell("Phone"),
                CreateCell("Email"),
                CreateCell("Creation Date"),
                CreateCell("Status")
            );
            sheetData.AppendChild(headerRow);

            // Data rows
            foreach (PotentialClient? client in clients)
            {
                Row? dataRow = new();
                dataRow.Append(
                    CreateCell(client.CompanyBusinessName),
                    CreateCell(client.ContactPhone),
                    CreateCell(client.ContactEmail),
                    CreateCell(client.CreationDate.ToString("yyyy-MM-dd")),
                    CreateCell(client.Status)
                );
                sheetData.AppendChild(dataRow);
            }
        }
        return memoryStream.ToArray();
    }

    private Cell CreateCell(string text)
    {
        return new Cell { DataType = CellValues.String, CellValue = new CellValue(text) };
    }
}