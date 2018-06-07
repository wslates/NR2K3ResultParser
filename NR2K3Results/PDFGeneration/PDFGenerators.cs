using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp;
using iTextSharp.text.pdf;
using System.IO;
using NR2K3Results.DriverAndResults;

namespace NR2K3Results.PDFGeneration
{
    class PracticePDFGenerators
    {
        private static float[] widths = { 5f, 6f, 17f, 30f, 7f, 10f, 6f, 7f, 10f, 10f };
        private static Random rand = new Random();

        public static void OutputPracticePDF(List<Driver> drivers, string series, string selectedSession, string raceName, string track)
        {
            HappyHourPracticePDFGen(drivers, series, selectedSession, raceName, track);
        }

        private static void HappyHourPracticePDFGen(List<Driver> drivers, string selectedSession, string series, string raceName, string track)
        {
            Document document = new Document(PageSize.A4, 15, 25, 15, 30);
            FileStream fs = null;
           
            try
            {
                fs = new FileStream("test.pdf", FileMode.Create, FileAccess.Write, FileShare.None);

                //build title
                StringBuilder title = new StringBuilder();
                title.AppendLine("MENCS Practice 1");
                title.AppendLine(track);
                title.AppendLine("Pennzoil 400");


                PdfWriter write = PdfWriter.GetInstance(document, fs);
                document.Open();

                //title
                Paragraph session = new Paragraph(title.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD))
                {
                    Alignment = Element.TITLE,
                    Leading = 17,
                    SpacingAfter = 25
                };
                document.Add(session);

                document.Add(GenerateTopRow());
           
                document.Add(GenerateDriverRows(drivers));
                document.Close();
            } catch (IOException e)
            {
                return;
            } 
        }

        private static PdfPTable GenerateTopRow()
        {
            
            PdfPTable table = new PdfPTable(10)
            {
                //set table to be total width of document excluding margins
                WidthPercentage = 100f,
            };
            table.SetWidths(widths);

            string[] cols = { "Pos", "Car", "Driver", "Sponsor", "Time", "Speed", "Lap #", "# Laps", "-Fastest", "-Next" };
            int[] justify = {Element.ALIGN_RIGHT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT,
                             Element.ALIGN_RIGHT, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT };
            for(int i = 0; i<10; i++)
            {  
                PdfPCell cell = new PdfPCell(new Phrase(cols[i], FontFactory.GetFont(FontFactory.HELVETICA, 9, Font.BOLD)))
                {
                    //sets only top and bottom border visible
                    Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER,
                    Colspan = 1,
                    //negate padding
                    PaddingTop = -2,
                    PaddingBottom = 1,
                    HorizontalAlignment = justify[i]
                };
                table.AddCell(cell);
            }

            return table;
            
        }

        private static PdfPTable GenerateDriverRows(List<Driver> drivers)
        {
            PdfPTable table = new PdfPTable(10)
            {
                //set table to be total width of document excluding margins
                WidthPercentage = 100f,
            };
            table.SetWidths(widths);

            foreach (Driver driver in drivers)
            {
                table.AddCell(GenerateDriverCell(driver.GetFinish().ToString(), driver.GetFinish(), 0, Element.ALIGN_RIGHT));
                table.AddCell(GenerateDriverCell(driver.number, driver.GetFinish(), 1, Element.ALIGN_RIGHT));
                table.AddCell(GenerateDriverCell(driver.firstName + " " + driver.lastName, driver.GetFinish(), 2, Element.ALIGN_LEFT));
                table.AddCell(GenerateDriverCell(driver.sponsor, driver.GetFinish(), 3, Element.ALIGN_LEFT));
                table.AddCell(GenerateDriverCell(driver.GetTime(), driver.GetFinish(), 4, Element.ALIGN_RIGHT));
                table.AddCell(GenerateDriverCell(driver.GetSpeed(), driver.GetFinish(), 5, Element.ALIGN_RIGHT));

                string[] laps = GenerateLaps();

                table.AddCell(GenerateDriverCell(laps[1], driver.GetFinish(), 6, Element.ALIGN_RIGHT));
                table.AddCell(GenerateDriverCell(laps[0], driver.GetFinish(), 7, Element.ALIGN_RIGHT));

                table.AddCell(GenerateDriverCell(driver.GetOffLeader(), driver.GetFinish(), 8, Element.ALIGN_RIGHT));
                table.AddCell(GenerateDriverCell(driver.GetOffNext(), driver.GetFinish(), 9, Element.ALIGN_RIGHT));
            }

            return table;
        }

        private static PdfPCell GenerateDriverCell(string data, int verPos, int horizPos, int justify)
        {
            int border = 0;

            //determine whether or not to have line underneath this row.
            if (verPos % 3 == 0)
            {
                border = Rectangle.BOTTOM_BORDER;
            }

            if (data.Length>10)
            {
                while (FontFactory.GetFont(FontFactory.HELVETICA, 9).BaseFont.GetWidthPoint(data, 9) > widths[horizPos] * 4.64)
                {
                    data = data.Substring(0, (data.Length - 1));
                }
            }
           
            
            return new PdfPCell(new Phrase(data, FontFactory.GetFont(FontFactory.HELVETICA, 9)))
            {
                Border = border,
                Colspan = 1,
                PaddingTop = 2,
                PaddingBottom = 2,
                HorizontalAlignment = justify, 
                
            };
        }

        private static string[] GenerateLaps()
        {
            string[] cells = new string[2];
            cells[0] = rand.Next(5, 50).ToString();          
            cells[1] = rand.Next(5, Convert.ToInt16(cells[0])).ToString();
            return cells;
        }
    }
}
