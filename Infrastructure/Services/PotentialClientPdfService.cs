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
using iText.Kernel.Geom;

namespace SagaAserhi.Infrastructure.Services
{
    public class PotentialClientPdfService : IPotentialClientPdfService
    {
        public async Task<byte[]> GeneratePdf(IEnumerable<PotentialClient> clients)
        {
            return await Task.Run(() =>
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf, PageSize.A4);
                    document.SetMargins(40, 40, 40, 40);

                    AddTitle(document);
                    AddDate(document);
                    AddClientTable(document, clients);
                    AddFooter(document, clients);

                    document.Close();
                    return memoryStream.ToArray();
                }
            });
        }

        private void AddTitle(Document document)
        {
            Paragraph title = new Paragraph("Reporte de Clientes Potenciales")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(22)
                .SetBold()
                .SetMarginBottom(15);

            document.Add(title);
        }

        private void AddDate(Document document)
        {
            Paragraph date = new Paragraph($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(10)
                .SetItalic()
                .SetMarginBottom(15);

            document.Add(date);
        }

        private void AddClientTable(Document document, IEnumerable<PotentialClient> clients)
        {
            float[] columnWidths = { 1.3f, 2f, 2f, 2f, 2.5f, 1.5f };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths))
                .UseAllAvailableWidth();

            string[] headers = { "ID", "Representante", "Nombre", "Actividad", "Contacto", "Estado" };

            foreach (string header in headers)
            {
                Cell headerCell = new Cell()
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
                    .SetPadding(8)
                    .Add(new Paragraph(header).SetFontSize(10));

                table.AddHeaderCell(headerCell);
            }

            foreach (PotentialClient client in clients)
            {
                table.AddCell(CreateCell(TruncateText($"{client.Identification?.Type}: {client.Identification?.Number}", 18), TextAlignment.CENTER));
                table.AddCell(CreateCell(TruncateText(client.LegalRepresentative, 25)));
                table.AddCell(CreateCell(TruncateText(client.BusinessInfo!.TradeName, 25)));
                table.AddCell(CreateCell(TruncateText(client.BusinessInfo!.EconomicActivity, 25)));
                table.AddCell(CreateCell($"Email: {TruncateText(client.BusinessInfo?.Email, 22)}\nTel: {client.BusinessInfo?.Phone}"));
                table.AddCell(CreateCell($"{client.Status?.Current}\n{client.UpdatedAt:dd/MM/yy}", TextAlignment.CENTER));
            }

            document.Add(table);
        }

        private void AddFooter(Document document, IEnumerable<PotentialClient> clients)
        {
            int clientCount = clients.Count();
            Paragraph footer = new Paragraph($"Total de clientes: {clientCount}")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(10)
                .SetMarginTop(15)
                .SetBold();

            document.Add(footer);
        }

        private Cell CreateCell(string content, TextAlignment alignment = TextAlignment.LEFT)
        {
            return new Cell()
                .SetTextAlignment(alignment)
                .SetPadding(6)
                .Add(new Paragraph(content ?? "N/A")
                    .SetFontSize(9)
                    .SetFixedLeading(12));
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return "N/A";
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }
    }
}
