using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Infrastructure.Services;

public class ProposalExcelServices : IProposalExcelService
{
    private readonly IProposalRepository _proposalRepository;
    private readonly IPotentialClientRepository _clientRepository;

    private static readonly string[] Headers = new[]
    {
        "Número de Propuesta", "Estado de Propuesta", "Envío", "Revisión",
        "Fecha de Creación", "Nombre Comercial del Cliente",
        "Dirección del Sitio", "Ciudad del Sitio", "Teléfono del Sitio",
        "Tipo de Residuo", "Clasificación", "Tratamiento", "Frecuencia", "Precio"
    };
    
    public ProposalExcelServices(IProposalRepository proposalRepository,
        IPotentialClientRepository clientRepository)
    {
        _proposalRepository = proposalRepository;
        _clientRepository = clientRepository;
    }

    public async Task<byte[]> ExportToExcel(CancellationToken cancellationToken)
    {
        try
        {
            (List<Proposal> proposals, _) = await _proposalRepository.GetAllProposals(1, int.MaxValue);
            MemoryStream stream = new();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                SheetData sheetData = new();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = spreadsheetDocument.WorkbookPart!.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Propuestas"
                };
                sheets.Append(sheet);

                AddHeaders(sheetData);
                await AddProposalsData(sheetData, proposals, cancellationToken);

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
        Row headerRow = new Row();

        foreach (string header in Headers)
        {
            Cell cell = new Cell()
            {
                DataType = CellValues.String,
                CellValue = new CellValue(header)
            };
            headerRow.Append(cell);
        }

        sheetData.Append(headerRow);
    }

    private async Task AddProposalsData(SheetData sheetData, IEnumerable<Proposal> proposals,
        CancellationToken cancellationToken)
    {
        foreach (Proposal proposal in proposals)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            PotentialClient? client = await _clientRepository.GetByIdPotencialClient(proposal.ClientId);

            foreach (Site site in proposal.Sites)
            {
                foreach (Waste waste in site.Wastes)
                {
                    Row row = new Row();
                    row.Append(new[]
                    {
                        CreateCell(proposal.Number),
                        CreateCell(proposal.Status?.Proposal ?? string.Empty),
                        CreateCell(proposal.Status?.Sending ?? string.Empty),
                        CreateCell(proposal.Status?.Review ?? string.Empty),
                        CreateCell(proposal.CreatedAt.ToString("dd/MM/yyyy")),
                        CreateCell(client?.BusinessInfo?.TradeName ?? string.Empty),
                        CreateCell(site.Address),
                        CreateCell(site.City),
                        CreateCell(site.Phone),
                        CreateCell(waste.Type),
                        CreateCell(waste.Classification),
                        CreateCell(waste.Treatment),
                        CreateCell(waste.Price.ToString("C"))
                    });
                    sheetData.Append(row);
                }
            }
        }
    }

    private Cell CreateCell(string value)
    {
        return new Cell()
        {
            DataType = CellValues.String,
            CellValue = new CellValue(value)
        };
    }
}