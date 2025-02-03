using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Borders;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;
using iText.Commons.Actions;
using iText.Kernel.Events;
using iText.Kernel.Pdf.Canvas;

namespace SagaAserhi.Infrastructure.Services;
public class ContratPDFServices : IContratPDFServices
{
    public async Task<byte[]> GenerateContractPDFServices(PotentialClient client, Site site)
    {
        return await Task.Run(() =>
        {
            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf, PageSize.A4);

            // Configurar márgenes
            document.SetMargins(40, 40, 40, 40);

            // Agregar manejador de números de página
            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageNumberEventHandler());

            try
            {
                // Agregar contenido al documento
                AddHeader(document);
                AddContractInfo(document, client, site);
                AddClauses(document);
                AddWastesTable(document, site);
                AddPaymentInfo(document);
                AddSignatures(document, client);

                // Cerrar el documento
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
        document.Add(new Paragraph("R-GCL005 No. 3")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetFontSize(14));

        document.Add(new Paragraph("CONTRATO DE SERVICIO DE GESTIÓN EXTERNA DE RESIDUOS PELIGROSOS HOSPITALARIOS Y SIMILARES")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetFontSize(12));
    }
    private void AddContractInfo(Document document, PotentialClient client, Site site)
    {
        document.Add(new Paragraph("CONTRATISTA: ASERHI S.A.S. E.S.P.").SetBold());
        document.Add(new Paragraph("NIT: 830.502.145-5"));
        document.Add(new Paragraph("REPRESENTANTE LEGAL: YHON ELKIN GIRALDO ARISTIZABAL"));
        document.Add(new Paragraph("DIRECCIÓN: CALLE 16N No.7-69 BARRIO EL RECUERDO, POPAYÁN"));
        document.Add(new Paragraph("TELÉFONO: 3148908132"));
        document.Add(new Paragraph("E-MAIL: comercial.aserhi@hotmail.com"));

        document.Add(new Paragraph("CONTRATANTE: " + client.BusinessInfo.TradeName).SetBold());
        document.Add(new Paragraph("NIT: " + client.Identification.Number));
        document.Add(new Paragraph("REPRESENTANTE LEGAL: " + client.LegalRepresentative));
        document.Add(new Paragraph("DIRECCIÓN: " + site.Address));
        document.Add(new Paragraph("MUNICIPIO: " + site.City));
        document.Add(new Paragraph("DEPARTAMENTO: " + site.Department));
        document.Add(new Paragraph("TELÉFONO: " + site.Phone));
        document.Add(new Paragraph("E-MAIL: " + client.BusinessInfo.Email));
    }

    private static void AddWastesTable(Document document, Site site)
    {
        try
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (site?.Wastes == null || !site.Wastes.Any())
            {
                document.Add(new Paragraph("No hay residuos para mostrar")
                    .SetTextAlignment(TextAlignment.CENTER));
                return;
            }

            // Agregar información del sitio
            document.Add(new Paragraph($"Sede: {site.Name}")
                .SetFontSize(10)
                .SetBold());

            Table table = new Table(5).UseAllAvailableWidth();

            // Encabezados
            string[] headers = { "SEDE", "MUNICIPIO", "RESIDUOS", "TRATAMIENTO", "PRECIO POR KG" };
            foreach (string header in headers)
            {
                table.AddHeaderCell(new Cell()
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .Add(new Paragraph(header).SetBold())
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(5));
            }

            // Datos
            foreach (Waste? waste in site.Wastes)
            {
                // Sede
                table.AddCell(new Cell()
                    .Add(new Paragraph(site.Name ?? "N/A"))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(5));

                // Municipio
                table.AddCell(new Cell()
                    .Add(new Paragraph(site.City ?? "N/A"))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(5));

                // Residuos
                table.AddCell(new Cell()
                    .Add(new Paragraph(waste.Type ?? "N/A"))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(5));

                // Tratamiento
                table.AddCell(new Cell()
                    .Add(new Paragraph(waste.Treatment ?? "N/A"))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(5));

                // Precio
                table.AddCell(new Cell()
                    .Add(new Paragraph(waste.Price > 0
                        ? $"$ {waste.Price:N2}"
                        : "$ 0.00"))
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetPadding(5));
            }

            // Agregar frecuencia y precio total
            document.Add(table);
            document.Add(new Paragraph($"Frecuencia de viaje: {site.Frequencies.FrequencyOfTravel}")
                .SetFontSize(10));
            document.Add(new Paragraph($"Precio Total: $ {site.TotalPrice:N2}")
                .SetFontSize(10)
                .SetBold());

            // Después de agregar la tabla y la información de frecuencia y precio
            document.Add(new Paragraph("\nPARÁGRAFO PRIMERO: Este valor será sostenido sin variación por la duración del contrato.")
                .SetFontSize(10));
                
            document.Add(new Paragraph("2) Este valor podrá incrementarse con excepciones de fuerza mayor ocasionadas por incrementos repentinos de los combustibles o nuevas determinaciones legales que afecten radicalmente los costos operativos.")
                .SetFontSize(10));

            document.Add(new Paragraph("\nOCTAVA - FORMA DE PAGO: EL CONTRATANTE deberá cancelar el valor de la factura en los treinta (30) días siguientes a la fecha de presentación de las misma, mediante consignación o transferencia electrónica que podrá efectuarse en la cuenta de ahorros Nº 251291944 del Banco de AV Villas a nombre de ASERHI S.A.S E.S.P y enviar fotocopia del soporte a los siguientes correos electrónicos cartera.aserhi@hotmail.com o pagos en efectivo y/o cheque al personal autorizado por la empresa.")
                .SetFontSize(10));

            document.Add(new Paragraph("PARÁGRAFO PRIMERO: En caso de mora en el pago, EL CONTRATANTE reconocerá un interés moratorio mensual conforme a lo establecido en derecho comercial")
                .SetFontSize(10));

            document.Add(new Paragraph("\nNOVENA - AUTORIZACIÓN DE MANEJO, CONSULTA Y REPORTE DE DATOS: " +
                "En cumplimiento de la Ley 1581 de 2012 y su decreto reglamentario 1377 de 2013, las partes tienen derecho de conocer, actualizar, rectificar y solicitar la suspensión de sus datos personales en cualquier momento...")
                .SetFontSize(10));

            // ... Continuar con el resto de las cláusulas ...
            string[] additionalClauses = {
                "\nDECIMA - SOLUCIÓN DE DIFERENCIAS: Toda controversia o diferencia relativa a este convenio...",
                "\nDECIMA PRIMERA - INDEPENDENCIA DEL CONTRATANTE: El CONTRATISTA actuará por su propia cuenta...",
                "\nDECIMA SEGUNDA - CONFIDENCIALIDAD: las PARTES mantendrán la confidencialidad de los datos e información...",
                "\nDECIMA TERCERA - RÉGIMEN APLICABLE: EL CONTRATISTA obra bajo autonomía en la recolección..."
            };

            foreach (var clause in additionalClauses)
            {
                document.Add(new Paragraph(clause)
                    .SetFontSize(10)
                    .SetMarginBottom(10));
            }

            document.Add(new Paragraph($"\nPara la constancia se firma en Cali, a los (16) días del mes de septiembre de 2024")
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER));
        }
        catch (Exception ex)
        {
            throw new Exception("Error al generar la tabla de residuos y cláusulas adicionales", ex);
        }
    }

    private void AddSignatures(Document document, PotentialClient client)
    {
        document.Add(new Paragraph("\n\n"));
        Table signatureTable = new Table(2).UseAllAvailableWidth();

        signatureTable.AddCell(new Cell()
            .Add(new Paragraph("_______________________\n" +
                             "EL CONTRATANTE\n" +
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
        document.Add(new Paragraph("\nCLÁUSULAS DEL CONTRATO")
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));

        string[] clauses = new string[]
        {
            "PRIMERA - OBJETO: El presente contrato tiene como finalidad la prestación del Servicio Especial de Aseo en el componente de la Gestión Externa, que comprende recolección, transporte, almacenamiento, tratamiento y disposición final de residuos peligrosos (hospitalarios y similares).",
            
            "SEGUNDA - OBLIGACIONES DEL CONTRATISTA: EL CONTRATISTA se obliga a:\n" +
            "a) Recoger los residuos hospitalarios y similares de conformidad a las frecuencias pactadas entre las partes.\n" +
            "b) Transportar, almacenar, hacer tratamiento térmico (incineración) y disponer finalmente de manera segura los residuos.\n" +
            "c) Entregar los registros de control y peso al personal responsable.\n" +
            "d) Facturar el servicio prestado en forma detallada.\n" +
            "e) Suministrar a solicitud del CONTRATANTE los registros y licencias sanitarias y ambientales requeridos.\n" +
            "f) Brindar asesoría y capacitación en el manejo de residuos.\n" +
            "g) Garantizar la seguridad en el transporte conforme a la normativa vigente.\n" +
            "h) Mantener los vehículos y equipos en óptimas condiciones.",

            "TERCERA - OBLIGACIONES DEL CONTRATANTE:\n" +
            "a) Clasificar los residuos según la normativa vigente.\n" +
            "b) Reportar novedades semanalmente al correo rutas.aserhi@hotmail.com.\n" +
            "c) Utilizar bolsas rojas con las especificaciones técnicas requeridas.\n" +
            "d) Rotular adecuadamente los residuos.\n" +
            "e) Almacenar correctamente los residuos cortopunzantes.\n" +
            "f) Mantener una unidad de almacenamiento temporal adecuada.\n" +
            "g) Informar sobre residuos especiales o peligrosos.\n" +
            "h) Asignar personal responsable para la entrega de residuos.\n" +
            "i) Realizar los pagos en los plazos acordados.",

            "CUARTA - DURACIÓN: El presente contrato tendrá una vigencia de un (1) año a partir de su firma.",

            "QUINTA - TERMINACIÓN DEL CONTRATO: El contrato podrá terminarse:\n" +
            "a) Por mutuo acuerdo\n" +
            "b) Por incumplimiento de las obligaciones\n" +
            "c) Cuando el usuario no presente los residuos en la forma pactada",

            "SEXTA - MULTAS Y SANCIONES: El incumplimiento de las obligaciones dará lugar a multas equivalentes al valor restante del contrato, sin perjuicio de las demás acciones legales pertinentes."
        };

        foreach (string clause in clauses)
        {
            document.Add(new Paragraph(clause)
                .SetFontSize(10)
                .SetMarginBottom(10));
        }
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
        PdfCanvas pdfCanvas = new PdfCanvas(page);
        Canvas canvas = new Canvas(pdfCanvas, pageSize);
        
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