using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Infrastructure.Services
{
    public sealed class ProposalExcelServices : IProposalExcelService
    {
        private readonly IProposalRepository _repository;
        private const string SHEET_NAME = "Proposals";
        private static readonly string[] Headers = { "Title", "Description", "Amount", "Status", "Creation Date", "Company Name" };

        public ProposalExcelServices(IProposalRepository repository)
        {
            _repository = repository;
        }

        public async Task<byte[]> ExportToExcel(CancellationToken cancellationToken)
        {
            IEnumerable<Proposal> proposals = await _repository.GetAllAsync(cancellationToken) 
                ?? throw new InvalidOperationException("No se pudieron obtener las propuestas");

            MemoryStream memoryStream = new();
            SpreadsheetDocument document = CreateSpreadsheetDocument(memoryStream);
            SheetData worksheet = CreateWorksheet(document);
            
            AddHeaders(worksheet);
            AddProposalsData(worksheet, proposals);

            document.Close();
            return memoryStream.ToArray();
        }

        private static SpreadsheetDocument CreateSpreadsheetDocument(MemoryStream stream)
        {
            SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
            WorkbookPart workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            return document;
        }

        private static SheetData CreateWorksheet(SpreadsheetDocument document)
        {
            WorksheetPart worksheetPart = document.WorkbookPart!.AddNewPart<WorksheetPart>();
            SheetData sheetData = new();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
            Sheet sheet = new()
            {
                Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = SHEET_NAME
            };
            sheets.Append(sheet);

            return sheetData;
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
                Row dataRow = new();
                IEnumerable<Cell> cells = new[]
                {
                    CreateCell(proposal.Title ?? string.Empty),
                    CreateCell(proposal.Description ?? string.Empty),
                    CreateCell(proposal.Amount.ToString("N2")),
                    CreateCell(proposal.Status ?? string.Empty),
                    CreateCell(proposal.CreationDate.ToString("yyyy-MM-dd")),
                    CreateCell(proposal.CompanyBusinessName ?? string.Empty)
                };
                dataRow.Append(cells);
                sheetData.Append(dataRow);
            }
        }

        private static Cell CreateCell(string text)
        {
            return new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(text ?? string.Empty)
            };
        }
    }
}