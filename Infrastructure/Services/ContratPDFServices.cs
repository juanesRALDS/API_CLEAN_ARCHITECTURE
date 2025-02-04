using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Borders;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;
using iText.Kernel.Events;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace SagaAserhi.Infrastructure.Services;
public class ContratPDFServices : IContratPDFServices
{
    public async Task<byte[]> GenerateContractPDFServices(PotentialClient client, Site site)
    {
        return await Task.Run(() =>
        {
            using MemoryStream? ms = new();
            using PdfWriter? writer = new(ms);
            using PdfDocument? pdf = new(writer);
            using Document? document = new(pdf, PageSize.A4);

            document.SetMargins(40, 40, 40, 40);
            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageNumberEventHandler());

            try
            {
                AddHeader(document);
                AddContractInfo(document, client, site);
                AddClauses(document);
                AddWastesTable(document, site);
                AddPaymentInfo(document);
                AddSignatures(document, client);

                document.Close();
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el contrato: {ex.Message}", ex);
            }
        });
    }

    private void AddHeader(Document document)
    {
        PdfFont? boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

        document.Add(new Paragraph("R-GCL005 No. 3")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetFontSize(14));

        document.Add(new Paragraph("CONTRATO DE SERVICIO DE GESTIÓN EXTERNA DE RESIDUOS PELIGROSOS HOSPITALARIOS Y SIMILARES")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFont(boldFont)
            .SetBold()
            .SetFontSize(12));
    }
    private Paragraph CreateLabeledParagraph(string label, string value)
    {
        PdfFont? boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        Paragraph? paragraph = new();
        paragraph.Add(new iText.Layout.Element.Text(label).SetFont(boldFont));
        paragraph.Add(new iText.Layout.Element.Text(value));
        return paragraph;
    }

    private void AddContractInfo(Document document, PotentialClient client, Site site)
    {
        document.Add(CreateLabeledParagraph("CONTRATISTA: ", "ASERHI S.A.S. E.S.P."));
        document.Add(CreateLabeledParagraph("NIT: ", "830.502.145-5"));
        document.Add(CreateLabeledParagraph("REPRESENTANTE LEGAL: ", "YHON ELKIN GIRALDO ARISTIZABAL"));
        document.Add(CreateLabeledParagraph("DIRECCIÓN: ", "CALLE 16N No.7-69 BARRIO EL RECUERDO, POPAYÁN"));
        document.Add(CreateLabeledParagraph("TELÉFONO: ", "3148908132"));
        document.Add(CreateLabeledParagraph("E-MAIL: ", "comercial.aserhi@hotmail.com"));

        document.Add(new Paragraph("\n"));

        document.Add(CreateLabeledParagraph("CONTRATANTE", client.BusinessInfo.TradeName));
        document.Add(CreateLabeledParagraph("NIT", client.Identification.Number));
        document.Add(CreateLabeledParagraph("REPRESENTANTE LEGAL", client.LegalRepresentative));
        document.Add(CreateLabeledParagraph("DIRECCIÓN", site.Address));
        document.Add(CreateLabeledParagraph("MUNICIPIO", site.City));
        document.Add(CreateLabeledParagraph("DEPARTAMENTO", site.Department));
        document.Add(CreateLabeledParagraph("TELÉFONO", site.Phone));
        document.Add(CreateLabeledParagraph("E-MAIL", client.BusinessInfo.Email));

        document.Add(new Paragraph("\n\n"));

    }

    private Cell CreateHeaderCell(string header)
    {
        return new Cell()
            .SetBackgroundColor(ColorConstants.WHITE)
            .Add(CreateLabeledParagraph(header, ""))
            .SetTextAlignment(TextAlignment.CENTER)
            .SetPadding(5);
    }
    private void AddWastesTable(Document document, Site site)
    {
        try
        {
            if (document == null || site?.Wastes == null)
                throw new ArgumentNullException("Documento o residuos nulos");

            document.Add(new Paragraph("\n\n"));

            Table table = new Table(5)
            .UseAllAvailableWidth()
            .SetMarginTop(20)
            .SetMarginBottom(20);

            string[] headers = { "SEDE", "MUNICIPIO", "RESIDUOS", "TRATAMIENTO", "PRECIO" };
            foreach (string header in headers)
            {
                table.AddHeaderCell(CreateHeaderCell(header));
            }

            // Datos
            foreach ( Waste? waste in site.Wastes)
            {
                table.AddCell(CreateCell(site.Name));
                table.AddCell(CreateCell(site.City));
                table.AddCell(CreateCell(waste.Type));
                table.AddCell(CreateCell(waste.Treatment));
                table.AddCell(CreateCell(waste.DescriptionWaste));
            }

            document.Add(table);

            // Leer y agregar cláusulas adicionales
            string clausesPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Files", "ClausesForHomeContract.txt");
            if (File.Exists(clausesPath))
            {
                string additionalClauses = File.ReadAllText(clausesPath);
                document.Add(new Paragraph(additionalClauses)
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.JUSTIFIED)
                    .SetMarginTop(10));
            }
            else
            {
                throw new FileNotFoundException("Archivo de cláusulas adicionales no encontrado", clausesPath);
            }

            document.Add(new Paragraph("\n\n"));
        }
        catch (Exception ex)
        {
            throw new Exception("Error al generar la tabla de residuos y cláusulas adicionales", ex);
        }
    }

    private Cell CreateCell(string content, TextAlignment alignment = TextAlignment.CENTER)
    {
        return new Cell()
            .Add(new Paragraph(content ?? "N/A"))
            .SetTextAlignment(alignment)
            .SetPadding(5);
    }

    private void AddSignatures(Document document, PotentialClient client)
    {
        document.Add(new Paragraph("\n\n"));
        Table signatureTable = new Table(2).UseAllAvailableWidth();

        signatureTable.AddCell(new Cell()
            .Add(new Paragraph($"_______________________\n" +
                             $"EL CONTRATANTE\n" +
                             $"{client.LegalRepresentative}\n" +
                             "Representante Legal\n" +
                             $"C.C. {client.Identification.Number}")
                .SetTextAlignment(TextAlignment.CENTER))
            .SetBorder(Border.NO_BORDER));

        signatureTable.AddCell(new Cell()
            .Add(new Paragraph("_______________________\n" +
                             "EL CONTRATISTA\n" +
                             "YHON ELKIN GIRALDO ARISTIZABAL\n" +
                             "Representante Legal\n" +
                             "C.C. 9.858.979")
                .SetTextAlignment(TextAlignment.CENTER))
            .SetBorder(Border.NO_BORDER));

        document.Add(signatureTable);
    }

    private void AddPaymentInfo(Document document)
    {
        document.Add(new Paragraph("\nINFORMACIÓN DE PAGO")
            .SetBold()
            .SetTextAlignment(TextAlignment.LEFT));

        document.Add(new Paragraph("Forma de pago: 30 días fecha factura")
            .SetFontSize(10));
    }



    private void AddClauses(Document document)
    {
        try
        {
            // Obtener la ruta base del proyecto
            string basePath = Directory.GetCurrentDirectory();

            // Construir rutas relativas a la ubicación actual
            string filePath = System.IO.Path.Combine(basePath, "Files", "Small-MediumContractClauses.txt");

            // Verificar si el archivo existe
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Archivo de cláusulas no encontrado en: {filePath}");
            }

            string clausesContent = File.ReadAllText(filePath);

            document.Add(new Paragraph(clausesContent)
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.JUSTIFIED)
                .SetMarginBottom(10));
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al procesar las cláusulas del contrato: {ex.Message}", ex);
        }
    }

    public class PageNumberEventHandler : iText.Kernel.Events.IEventHandler
    {
        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();

            Rectangle pageSize = page.GetPageSize();
            PdfCanvas pdfCanvas = new(page);
            Canvas canvas = new(pdfCanvas, pageSize);

            int pageNumber = pdfDoc.GetPageNumber(page);
            int totalPages = pdfDoc.GetNumberOfPages();

            // Agregar número de página
            canvas.SetFontSize(8)
                .ShowTextAligned(
                    $"PÁGINA: {pageNumber} de {totalPages}",
                    pageSize.GetWidth() - 40,
                    20,
                    TextAlignment.RIGHT,
                    VerticalAlignment.BOTTOM,
                    0
                );
        }
    }
}