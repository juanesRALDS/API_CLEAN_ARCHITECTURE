using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SagaAserhi.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SagaAserhi.Infrastructure.Services
{
    public class ProposalExcelServices
    {
        private static readonly string[] Headers = new[]
        {
            "Número de Propuesta", "Estado de Propuesta", "Envío", "Revisión", "Fecha de Creación", "Nombre Comercial del Cliente", "Dirección del Sitio", "Ciudad del Sitio", "Teléfono del Sitio"
        };

        private static Cell CreateCell(string text)
        {
            return new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(text)
            };
        }

        private static void AddHeaders(SheetData sheetData)
        {
            Row headerRow = new();
            IEnumerable<Cell> headerCells = Headers.Select(header => CreateCell(header));
            headerRow.Append(headerCells);
            sheetData.Append(headerRow);
        }

        private static void AddProposalsData(SheetData sheetData, IEnumerable<Proposal> proposals)
        {
            foreach (Proposal proposal in proposals)
            {
                foreach (var site in proposal.Sites)
                {
                    Row dataRow = new();
                    IEnumerable<Cell> cells = new[]
                    {
                        CreateCell(proposal.Number ?? string.Empty),
                        CreateCell(proposal.Status.Proposal ?? string.Empty),
                        CreateCell(proposal.Status.Sending ?? string.Empty),
                        CreateCell(proposal.Status.Review ?? string.Empty),
                        CreateCell(proposal.CreatedAt.ToString("yyyy-MM-dd")),
                        CreateCell(proposal.PotentialClient.BusinessInfo.TradeName ?? string.Empty),
                        CreateCell(site.Address ?? string.Empty),
                        CreateCell(site.City ?? string.Empty),
                        CreateCell(site.Phone ?? string.Empty)
                    };
                    dataRow.Append(cells);
                    sheetData.Append(dataRow);
                }
            }
        }

        public static void GenerateExcelFile(IEnumerable<Proposal> proposals, string filePath)
        {
            using (var spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new()
                {
                    Id = spreadsheetDocument.WorkbookPart!.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Proposals"
                };
                sheets.Append(sheet);

                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>() ?? new SheetData();
                if (worksheetPart.Worksheet.GetFirstChild<SheetData>() == null)
                {
                    worksheetPart.Worksheet.AppendChild(sheetData);
                }

                AddHeaders(sheetData);
                AddProposalsData(sheetData, proposals);

                workbookPart.Workbook.Save();
            }
        }
    }
}