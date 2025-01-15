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
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Infrastructure.Services;
public class PotentialClientPdfService : IPotentialClientPdfService
{
    public async Task<byte[]> GeneratePdf(IEnumerable<PotentialClient> clients)
    {
        using var memoryStream = new MemoryStream();
        var writer = new PdfWriter(memoryStream);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        // Título del documento
        document.Add(new Paragraph("Reporte de Clientes Potenciales")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20)
            .SetBold());

        // Fecha del reporte
        document.Add(new Paragraph($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}")
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetFontSize(10));

        // Tabla
        var table = new Table(6).UseAllAvailableWidth();
        
        // Encabezados
        string[] headers = { 
            "Identificación", 
            "Nombre Comercial", 
            "Actividad Económica",
            "Contacto",
            "Ubicación",
            "Estado" 
        };
        
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
            // Columna Identificación
            table.AddCell(new Cell().Add(new Paragraph(
                $"{client.Identification.Type}: {client.Identification.Number}")));

            // Columna Nombre Comercial
            table.AddCell(new Cell().Add(new Paragraph(
                client.BusinessInfo.TradeName)));

            // Columna Actividad Económica
            table.AddCell(new Cell().Add(new Paragraph(
                client.BusinessInfo.EconomicActivity)));

            // Columna Contacto
            table.AddCell(new Cell().Add(new Paragraph(
                $"Email: {client.BusinessInfo.Email}\n" +
                $"Tel: {client.BusinessInfo.Phone}")));

            // Columna Ubicación
            table.AddCell(new Cell().Add(new Paragraph(
                $"{client.Location.City}, {client.Location.Department}\n" +
                $"{client.Location.Address}")));

            // Columna Estado
            table.AddCell(new Cell().Add(new Paragraph(
                $"Estado: {client.Status.Current}\n" +
                $"Actualizado: {client.UpdatedAt:dd/MM/yyyy}")));
        }

        document.Add(table);

        // Pie de página
        document.Add(new Paragraph($"Total de clientes: {clients.Count()}")
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetFontSize(10));

        document.Close();
        return memoryStream.ToArray();
    }
}