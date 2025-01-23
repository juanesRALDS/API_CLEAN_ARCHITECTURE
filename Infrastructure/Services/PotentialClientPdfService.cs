using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Infrastructure.Services
{
    public class PotentialClientPdfService : IPotentialClientPdfService
    {
        public async Task<byte[]> GeneratePdf(IEnumerable<PotentialClient> clients)
        {
            return await Task.Run(() =>
            {
                MemoryStream memoryStream = new MemoryStream();
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                AddTitle(document);
                AddDate(document);
                AddClientTable(document, clients);
                AddFooter(document, clients);

                document.Close();
                return memoryStream.ToArray();
            });
        }

        private void AddTitle(Document document)
        {
            Paragraph title = new Paragraph("Reporte de Clientes Potenciales")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20)
                .SetBold()
                .SetMarginBottom(20);

            document.Add(title);
        }

        private void AddDate(Document document)
        {
            Paragraph date = new Paragraph($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(10)
                .SetMarginBottom(10);

            document.Add(date);
        }

        private void AddClientTable(Document document, IEnumerable<PotentialClient> clients)
        {
            float[] columnWidths = { 1, 2, 2, 2, 2, 1 }; // Proporción de ancho de columnas
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

            // Encabezados
            string[] headers = {
                "Identificación", "Nombre Comercial", "Actividad Económica",
                "Contacto", "Ubicación", "Estado"
            };

            foreach (string header in headers)
            {
                Cell headerCell = new Cell()
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
                    .Add(new Paragraph(header));

                table.AddHeaderCell(headerCell);
            }

            // Datos de clientes
            foreach (PotentialClient client in clients)
            {
                table.AddCell(CreateCell($"{client.Identification?.Type}: {client.Identification?.Number}"));
                table.AddCell(CreateCell(client.BusinessInfo!.TradeName));
                table.AddCell(CreateCell(client.BusinessInfo!.EconomicActivity));
                table.AddCell(CreateCell($"Email: {client.BusinessInfo?.Email}\nTel: {client.BusinessInfo?.Phone}"));
                table.AddCell(CreateCell($"{client.Location?.City}, {client.Location?.Department}\n{client.Location?.Address}"));
                table.AddCell(CreateCell($"Estado: {client.Status?.Current}\nActualizado: {client.UpdatedAt:dd/MM/yyyy}"));
            }

            document.Add(table.SetMarginTop(20));
        }

        private void AddFooter(Document document, IEnumerable<PotentialClient> clients)
        {
            int clientCount = clients.Count();
            Paragraph footer = new Paragraph($"Total de clientes: {clientCount}")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(10)
                .SetMarginTop(20);

            document.Add(footer);
        }

        private Cell CreateCell(string content)
        {
            return new Cell()
                .SetTextAlignment(TextAlignment.LEFT)
                .SetPadding(5)
                .Add(new Paragraph(content ?? "N/A").SetFontSize(9));
        }
    }
}
