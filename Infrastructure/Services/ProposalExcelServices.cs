using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;
using System.Collections.Generic;

namespace SagaAserhi.Infrastructure.Services
{
    public class ProposalExcelServices : IProposalExcelService
    {
        private readonly IProposalRepository _repository;

        public ProposalExcelServices(IProposalRepository repository)
        {
            _repository = repository;
        }

        public async Task<byte[]> ExportToExcel(CancellationToken cancellationToken)
        {
            IEnumerable<Proposal> proposals = await _repository.GetAllAsync(cancellationToken);

            MemoryStream memoryStream = new();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                SheetData sheetData = new();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new()
                {
                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Proposals"
                };
                sheets.Append(sheet);

                // Headers
                Row headerRow = new();
                headerRow.Append(
                    CreateCell("Title"),
                    CreateCell("Description"),
                    CreateCell("Amount"),
                    CreateCell("Status"),
                    CreateCell("Creation Date"),
                    CreateCell("Company Name")
                );
                sheetData.Append(headerRow);

                // Data
                foreach (Proposal proposal in proposals)
                {
                    Row dataRow = new();
                    dataRow.Append(
                        CreateCell(proposal.Title),
                        CreateCell(proposal.Description),
                        CreateCell(proposal.Amount.ToString("N2")),
                        CreateCell(proposal.Status),
                        CreateCell(proposal.CreationDate.ToString("yyyy-MM-dd")),
                        CreateCell(proposal.CompanyBusinessName)
                    );
                    sheetData.Append(dataRow);
                }
            }
            return memoryStream.ToArray();
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