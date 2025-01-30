using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Borders;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Infrastructure.Services;
public class ContratPDFServices : IContratPDFServices
{
    public async Task<byte[]> GenerateContractPDFServices(PotentialClient client, Site site)
    {
        return await Task.Run(() =>
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, PageSize.A4);
                document.SetMargins(40, 40, 40, 40);

                AddHeader(document);
                AddContractInfo(document, client, site);
                AddClauses(document);
                AddWastesTable(document, site);
                AddPaymentInfo(document);
                AddSignatures(document, client);
                AddPageNumbers(document);

                document.Close();
                return ms.ToArray();
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
        // Información del contratista (ASERHI)
        document.Add(new Paragraph("\nCONTRATISTA")
            .SetBold());
        document.Add(new Paragraph($"CONTRATISTA: ASERHI S.A.S. E.S.P.")
            .SetFontSize(10));
        document.Add(new Paragraph($"NIT: 830.502.145-5")
            .SetFontSize(10));
        // ... resto de la información fija del contratista

        // Información del contratante
        document.Add(new Paragraph("\nCONTRATANTE")
            .SetBold());
        document.Add(new Paragraph($"Razón Social: {client.BusinessInfo.TradeName}")
            .SetFontSize(10));
        document.Add(new Paragraph($"NIT: {client.Identification.Number}")
            .SetFontSize(10));
        document.Add(new Paragraph($"Representante Legal: {client.LegalRepresentative}")
            .SetFontSize(10));
        document.Add(new Paragraph($"Dirección: {site.Address}")
            .SetFontSize(10));
        document.Add(new Paragraph($"Municipio: {site.City}")
            .SetFontSize(10));
        document.Add(new Paragraph($"Departamento: {site.Department}")
            .SetFontSize(10));
        document.Add(new Paragraph($"Teléfono: {site.Phone}")
            .SetFontSize(10));
        document.Add(new Paragraph($"Email: {client.BusinessInfo.Email}")
            .SetFontSize(10));
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
        foreach (var waste in site.Wastes)
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
    }
    catch (Exception ex)
    {
        throw new Exception("Error al generar la tabla de residuos", ex);
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

    private void AddPageNumbers(Document document)
    {
        int n = document.GetPdfDocument().GetNumberOfPages();
        for (int i = 1; i <= n; i++)
        {
            document.ShowTextAligned(new Paragraph($"PÁGINA: {i} de {n}")
                .SetFontSize(8),
                559, 20, i,
                TextAlignment.RIGHT,
                VerticalAlignment.BOTTOM,
                0);
        }
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
            "SEGUNDA - OBLIGACIONES DEL CONTRATISTA: El CONTRATISTA se obliga a cumplir con la recolección, transporte y disposición de los residuos según las normativas vigentes.",
            "TERCERA - OBLIGACIONES DEL CONTRATANTE: Quien se obliga a realizar la gestión integral de sus residuos conforme a la normativa vigente.",
            "CUARTA - DURACIÓN: El presente contrato tendrá una duración de 7 días, contados a partir de la fecha de suscripción de este documento.",
            "QUINTA - TERMINACIÓN DEL CONTRATO: Se podrá dar por terminado por mutuo acuerdo o por incumplimiento de las obligaciones.",
            "SEXTA - MULTAS: En caso de incumplimiento, se aplicarán penalidades conforme a lo estipulado en el contrato.",
            "SÉPTIMA - TARIFAS Y FRECUENCIA DE RECOLECCIÓN: Se establecerán según la sede y tipo de residuos.",
            "OCTAVA - FORMA DE PAGO: El pago se debe realizar dentro de los 30 días posteriores a la facturación.",
            "NOVENA - AUTORIZACIÓN DE MANEJO DE DATOS: Ambas partes autorizan el tratamiento de datos conforme a la Ley 1581 de 2012.",
            "DÉCIMA - SOLUCIÓN DE DIFERENCIAS: Las controversias se resolverán mediante conciliación o instancias judiciales.",
            "DÉCIMA PRIMERA - INDEPENDENCIA DEL CONTRATANTE: No existe relación laboral entre las partes.",
            "DÉCIMA SEGUNDA - CONFIDENCIALIDAD: Las partes mantendrán la confidencialidad de la información intercambiada.",
            "DÉCIMA TERCERA - RÉGIMEN APLICABLE: Este contrato se rige por la legislación colombiana vigente."
        };

        foreach (var clause in clauses)
        {
            document.Add(new Paragraph(clause).SetFontSize(10));
        }
    }
}