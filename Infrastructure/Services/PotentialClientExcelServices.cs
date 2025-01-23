using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Infrastructure.Services;

public class PotentialClientExcelServices : IPotentialClientExcelServices
{
    private const int INITIAL_ROW = 1;

    public async Task<byte[]> GenerateExcel(IEnumerable<PotentialClient> clients, int pageNumber, int pageSize, int totalCount)
    {
        return await Task.Run(() =>
        {
            using var memoryStream = new MemoryStream();
            using var spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);

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

            var paginationRow = new Row();
            paginationRow.AppendChild(CreateCell($"Página {pageNumber}"));
            paginationRow.AppendChild(CreateCell($"Registros por página: {pageSize}"));
            paginationRow.AppendChild(CreateCell($"Total registros: {totalCount}"));
            sheetData.AppendChild(paginationRow);

            // Agregar una fila vacía como separador
            sheetData.AppendChild(new Row());

            // Continuar con los headers y data
            AddHeaders(sheetData);
            AddClientData(sheetData, clients);

            workbookPart.Workbook.Save();
            return memoryStream.ToArray();
        });
    }

    private void AddHeaders(SheetData sheetData)
    {
        var headerRow = new Row { RowIndex = INITIAL_ROW };
        var headers = GetHeaders();

        foreach (var header in headers)
        {
            headerRow.AppendChild(CreateCell(header));
        }

        sheetData.AppendChild(headerRow);
    }

    private void AddClientData(SheetData sheetData, IEnumerable<PotentialClient> clients)
    {
        uint rowIndex = INITIAL_ROW + 1;

        foreach (var client in clients)
        {
            var row = new Row { RowIndex = rowIndex++ };
            row.Append(GetClientCells(client));
            sheetData.AppendChild(row);
        }
    }

    private IEnumerable<Cell> GetClientCells(PotentialClient client)
    {
        return new[]
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
    }

    private string[] GetHeaders() => new[]
    {
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


    private Cell CreateCell(string text)
    {
        return new Cell
        {
            DataType = CellValues.String,
            CellValue = new CellValue(string.IsNullOrEmpty(text) ? "-" : text)
        };
    }
}