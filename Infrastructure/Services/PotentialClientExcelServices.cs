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
        var clients = await _repository.GetAllAsync(cancellationToken);

        using var memoryStream = new MemoryStream();
        using (var spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            var workbook = spreadsheetDocument.WorkbookPart?.Workbook ?? new Workbook();
            var sheets = workbook.AppendChild(new Sheets());

            var sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart!.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Potential Clients"
            };
            sheets.Append(sheet);

            // Headers
            var headerRow = new Row();
            headerRow.Append(
                CreateCell("Company Name"),
                CreateCell("Phone"),
                CreateCell("Email"),
                CreateCell("Creation Date"),
                CreateCell("Status")
            );
            sheetData.AppendChild(headerRow);

            // Data rows
            foreach (var client in clients)
            {
                var dataRow = new Row();
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