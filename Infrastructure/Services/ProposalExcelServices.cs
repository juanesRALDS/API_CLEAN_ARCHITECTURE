using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.Interfaces.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SagaAserhi.Infrastructure.Services
{
    public class ProposalExcelServices : IProposalExcelService
    {
        private static readonly string[] Headers = new[]
        {
            "Número de Propuesta", "Estado de Propuesta", "Envío", "Revisión", 
            "Fecha de Creación", "Nombre Comercial del Cliente", 
            "Dirección del Sitio", "Ciudad del Sitio", "Teléfono del Sitio"
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
            headerRow.Append(Headers.Select(header => CreateCell(header)));
            sheetData.Append(headerRow);
        }

        private static void AddProposalsData(SheetData sheetData, IEnumerable<Proposal> proposals)
        {
            foreach (var proposal in proposals)
            {
                foreach (var site in proposal.Sites)
                {
                    Row dataRow = new();
                    var cells = new[]
                    {
                        CreateCell(proposal.Number),
                        CreateCell(proposal.Status.Proposal),
                        CreateCell(proposal.Status.Sending),
                        CreateCell(proposal.Status.Review),
                        CreateCell(proposal.CreatedAt.ToString("yyyy-MM-dd")),
                        CreateCell(proposal.PotentialClient?.BusinessInfo?.TradeName ?? string.Empty),
                        CreateCell(site.Address),
                        CreateCell(site.City),
                        CreateCell(site.Phone)
                    };
                    dataRow.Append(cells);
                    sheetData.Append(dataRow);
                }
            }
        }

        public async Task<byte[]> ExportToExcel(CancellationToken cancellationToken)
        {
            using var memoryStream = new MemoryStream();
            using var spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
            
            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = spreadsheetDocument.WorkbookPart!.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Proposals"
            };
            sheets.Append(sheet);

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            
            AddHeaders(sheetData);
            // Aquí deberías obtener y agregar los datos de las propuestas
            // AddProposalsData(sheetData, proposals);

            workbookPart.Workbook.Save();
            spreadsheetDocument.Close();

            return memoryStream.ToArray();
        }
    }
}