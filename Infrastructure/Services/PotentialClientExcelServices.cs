using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Infrastructure.Services;
public class PotentialClientExcelServices : IPotentialClientExcelServices
{
    public async Task<byte[]> GenerateExcel(IEnumerable<PotentialClient> clients)
    {
        using var memoryStream = new MemoryStream();
        using var spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);

        var workbookPart = spreadsheetDocument.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        var sheetData = new SheetData();
        worksheetPart.Worksheet = new Worksheet(sheetData);

        var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
        var sheet = new Sheet()
        {
            Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
            SheetId = 1,
            Name = "Clientes Potenciales"
        };
        sheets.AppendChild(sheet);

        // Headers
        Row headerRow = new();
        string[] headers = {
            "Tipo Identificación",
            "Número Identificación",
            "Nombre Comercial",
            "Actividad Económica",
            "Email",
            "Teléfono",
            "Dirección",
            "Ciudad",
            "Departamento",
            "Estado Actual",
            "Fecha Creación",
            "Última Actualización"
        };

        foreach (string header in headers)
        {
            headerRow.AppendChild(CreateCell(header));
        }
        sheetData.AppendChild(headerRow);

        // Data rows
        foreach (var client in clients)
        {
            Row dataRow = new();
            dataRow.Append(
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
            );
            sheetData.AppendChild(dataRow);
        }

        workbookPart.Workbook.Save();
        spreadsheetDocument.Close();
        return memoryStream.ToArray();
    }

    private Cell CreateCell(string text)
    {
        return new Cell 
        { 
            DataType = CellValues.String, 
            CellValue = new CellValue(string.IsNullOrEmpty(text) ? "-" : text) 
        };
    }
}