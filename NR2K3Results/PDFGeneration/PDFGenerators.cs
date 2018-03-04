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
    class PDFGenerators
    {
        private static float[] widths = { 7f, 7f, 30f, 40f, 12f, 12f, 12f, 12f };
        public static void OutputPracticePDF(List<Driver> drivers, string series, string selectedSession, string raceName)
        {
            HappyHourPracticePDFGen(drivers, series, selectedSession, raceName);
        }

        private static void HappyHourPracticePDFGen(List<Driver> drivers, string selectedSession, string series, string raceName)
        {
            Document document = new Document(PageSize.A4, 15, 25, 15, 30);
            FileStream fs = null;
           
            fs = new FileStream("test.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
           

            //build title
            StringBuilder title = new StringBuilder();
            title.AppendLine("MENCS Practice 1");
            title.AppendLine("Las Vegas Motor Speedway");
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


            PdfPTable results = new PdfPTable(8)
            {
                //set table to be total width of document excluding margins
                WidthPercentage = 100f
            };

            


            document.Add(GenerateTopRow());
           
            document.Add(GenerateDriverRows(drivers));
            document.Close();
        }

        private static PdfPTable GenerateTopRow()
        {
            
            PdfPTable table = new PdfPTable(8)
            {
                //set table to be total width of document excluding margins
                WidthPercentage = 100f,
            };
            table.SetWidths(widths);

            string[] cols = { "Pos", "Car", "Driver", "Team", "Time", "Speed", "-Fastest", "-Next" };
            
            foreach (string column in cols)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column, FontFactory.GetFont(FontFactory.HELVETICA, 9, Font.BOLD)))
                {
                    //sets only top and bottom border visible
                    Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER,
                    Colspan = 1,
                    //negate padding
                    PaddingTop = -2,
                    PaddingBottom = 1
                };
                table.AddCell(cell);
            }

            return table;
            
        }

        private static PdfPTable GenerateDriverRows(List<Driver> drivers)
        {
            PdfPTable table = new PdfPTable(8)
            {
                //set table to be total width of document excluding margins
                WidthPercentage = 100f,
            };
            table.SetWidths(widths);

            foreach (Driver driver in drivers)
            {
                table.AddCell(GenerateDriverCell(driver.GetFinish().ToString(), driver.GetFinish()));
                table.AddCell(GenerateDriverCell(driver.number, driver.GetFinish()));
                table.AddCell(GenerateDriverCell(driver.firstName + " " + driver.lastName, driver.GetFinish()));
                table.AddCell(GenerateDriverCell(driver.sponsor, driver.GetFinish()));
                table.AddCell(GenerateDriverCell(driver.GetTime(), driver.GetFinish()));
                table.AddCell(GenerateDriverCell(driver.GetSpeed(), driver.GetFinish()));
                table.AddCell(GenerateDriverCell(driver.GetOffLeader(), driver.GetFinish()));
                table.AddCell(GenerateDriverCell(driver.GetOffNext(), driver.GetFinish()));
            }

            return table;
        }

        private static PdfPCell GenerateDriverCell(string data, int pos)
        {
            int border = 0;
            if (pos%3==0)
            {
                border = Rectangle.BOTTOM_BORDER;
            } 

            return new PdfPCell(new Phrase(data, FontFactory.GetFont(FontFactory.HELVETICA, 9)))
            {
                //sets only top and bottom border visible
                Border = border,
                Colspan = 1,
                //negate padding
                PaddingTop = 2,
                PaddingBottom = 2
            };
        }
    }
}
