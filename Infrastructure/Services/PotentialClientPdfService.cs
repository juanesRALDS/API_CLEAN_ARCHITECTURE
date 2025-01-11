using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces;

namespace SagaAserhi.Infrastructure.Services
{
    public class PotentialClientPdfService : IPotentialClientPdfService
    {
        private readonly IPotentialClientRepository _repository;

        public PotentialClientPdfService(IPotentialClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<byte[]> ExportToPdf(CancellationToken cancellationToken)
        {
            var clients = await _repository.GetAllAsync(cancellationToken);

            using var memoryStream = new MemoryStream();
            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // Título
            document.Add(new Paragraph("Listado de Clientes Potenciales")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18)
                .SetBold()
                .SetMarginBottom(20));

            // Tabla
            var table = new Table(5).UseAllAvailableWidth();
            
            // Encabezados
            string[] headers = { "Empresa", "Teléfono", "Email", "Fecha", "Estado" };
            foreach (var header in headers)
            {
                table.AddCell(new Cell()
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
                    .Add(new Paragraph(header)));
            }

            // Datos
            foreach (var client in clients)
            {
                table.AddCell(new Cell().Add(new Paragraph(client.CompanyBusinessName)));
                table.AddCell(new Cell().Add(new Paragraph(client.ContactPhone)));
                table.AddCell(new Cell().Add(new Paragraph(client.ContactEmail)));
                table.AddCell(new Cell().Add(new Paragraph(client.CreationDate.ToString("dd/MM/yyyy"))));
                table.AddCell(new Cell().Add(new Paragraph(client.Status)));
            }

            document.Add(table);
            document.Close();

            return memoryStream.ToArray();
        }
    }
}