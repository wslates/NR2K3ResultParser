using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NR2K3Results.DriverAndResults;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace NR2K3Results.PDFGeneration
{
    class RacePDFGenerator
    {
        public static void OutputPDF(List<Driver> drivers, string series, string raceName, Track track)
        {
            Document document = new Document(PageSize.A4, 15, 25, 15, 30);
            FileStream fs = null;

            try
            {
                fs = new FileStream("test.pdf", FileMode.Create, FileAccess.Write, FileShare.None);

                //build title
                StringBuilder title = new StringBuilder();
                title.AppendLine(series);
                title.AppendLine("Unofficial Race Results for the " + raceName);
                title.AppendLine(track.name + " - " + track.city + ", " + track.state + " - " + track.description);
                title.AppendLine("Total Race Length - " + track.laps + " Laps - " + Decimal.Round(track.laps*track.length) + " Miles");
                title.AppendLine();
                

                PdfWriter write = PdfWriter.GetInstance(document, fs);
                document.Open();

                //title
                Paragraph session = new Paragraph(title.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 11, Font.BOLDITALIC))
                {
                    Alignment = Element.ALIGN_LEFT,
                    Leading = 17,
                    SpacingAfter = 25
                };
                document.Add(session);

               // document.Add(GenerateTopRow(ref SessionData.PRACTICECOLUMNWIDTHS, ref SessionData.PRACTICECOLUMNS));

                //document.Add(GenerateDriverRows(drivers, ref SessionData.PRACTICECOLUMNWIDTHS));
                document.Close();
            }
            catch (IOException e)
            {
                return;
            }
        }


    }
}
