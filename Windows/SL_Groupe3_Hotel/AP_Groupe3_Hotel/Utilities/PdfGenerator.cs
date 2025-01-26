using AP_Groupe3_Hotel.Models;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.ComponentModel;
using System.IO;

namespace AP_Groupe3_Hotel.Utilities
{
    public class PdfGenerator
    {
        public void GenerateReservationsPdf(ICollectionView reservations, string outputPath)
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = new PdfWriter(fs);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Titre du document
                PdfFont titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                Paragraph title = new Paragraph("Liste des réservations")
                    .SetFont(titleFont)
                    .SetFontSize(18)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);  // TextAlignment.CENTER
                document.Add(title);

                document.Add(new Paragraph("\n"));

                // Table
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1, 1, 1, 1 }))
                    .UseAllAvailableWidth();

                // En-têtes de la table
                PdfFont headerFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                Cell cell;

                cell = new Cell().Add(new Paragraph("Numéro de la réservation").SetFont(headerFont));
                table.AddHeaderCell(cell);

                cell = new Cell().Add(new Paragraph("Chambre réservée").SetFont(headerFont));
                table.AddHeaderCell(cell);

                cell = new Cell().Add(new Paragraph("Date d'arrivée").SetFont(headerFont));
                table.AddHeaderCell(cell);

                cell = new Cell().Add(new Paragraph("Date de départ").SetFont(headerFont)); 
                table.AddHeaderCell(cell);

                cell = new Cell().Add(new Paragraph("Nom du client").SetFont(headerFont));
                table.AddHeaderCell(cell);

                // Corps de la table
                foreach (TbReservation reservation in reservations)
                {
                    table.AddCell(new Cell().Add(new Paragraph(reservation.PkRes.ToString() ?? "")));
                    table.AddCell(new Cell().Add(new Paragraph(reservation.TbChambre.PfkChaEtaNavigation.CodeEta.ToString() + " - " + reservation.TbChambre.CodeCha.ToString() ?? "")));
                    table.AddCell(new Cell().Add(new Paragraph(reservation.DatArrRes.ToString() ?? "")));
                    table.AddCell(new Cell().Add(new Paragraph(reservation.DatDepRes.ToString() ?? "")));
                    table.AddCell(new Cell().Add(new Paragraph(reservation.FkResCliNavigation.NomCli.ToString()+ " " + reservation.FkResCliNavigation.PreCli.ToString() ?? "")));
                }

                document.Add(table);

                document.Close();
            }

        }
    }
}
