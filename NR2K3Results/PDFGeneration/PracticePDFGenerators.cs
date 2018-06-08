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
        
        private static Random rand = new Random();


        public static void OutputPDF(List<Driver> drivers, string series, string selectedSession, string raceName, string track)
        {
            HappyHourPracticePDFGen(drivers, series, selectedSession, raceName, track);
        }

        private static void HappyHourPracticePDFGen(List<Driver> drivers, string series, string selectedSession, string raceName, string track)
        {
            Document document = new Document(PageSize.A4, 15, 25, 15, 30);
            FileStream fs = null;
           
            try
            {
                fs = new FileStream("test.pdf", FileMode.Create, FileAccess.Write, FileShare.None);

                //build title
                StringBuilder title = new StringBuilder();
                title.AppendLine(series);               
                title.AppendLine(track);
                title.AppendLine(raceName);
                title.AppendLine(selectedSession);

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

                document.Add(GenerateTopRow(ref SessionData.PRACTICECOLUMNWIDTHS, ref SessionData.PRACTICECOLUMNS));
           
                document.Add(GenerateDriverRows(drivers, ref SessionData.PRACTICECOLUMNWIDTHS));
                document.Close();
            } catch (IOException e)
            {
                return;
            } 
        }

        private static PdfPTable GenerateTopRow(ref float[] widths, ref List<Tuple<string, int>> tableData)
        {
            
            PdfPTable table = new PdfPTable(tableData.Count)
            {
                //set table to be total width of document excluding margins
                WidthPercentage = 100f,
            };
            table.SetWidths(widths);

          
            foreach(Tuple<string, int> column in tableData)
            {  
                PdfPCell cell = new PdfPCell(new Phrase(column.Item1, FontFactory.GetFont(FontFactory.HELVETICA, 9, Font.BOLD)))
                {
                    //sets only top and bottom border visible
                    Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER,
                    Colspan = 1,
                    //negate padding
                    PaddingTop = -2,
                    PaddingBottom = 1,
                    HorizontalAlignment = column.Item2
                };
                table.AddCell(cell);
            }

            return table;
            
        }

        private static PdfPTable GenerateDriverRows(List<Driver> drivers, ref float[] widths)
        {
            PdfPTable table = new PdfPTable(widths.Length)
            {
                //set table to be total width of document excluding margins
                WidthPercentage = 100f,
            };
            table.SetWidths(widths);

            foreach (Driver driver in drivers)
            {
                table.AddCell(GenerateDriverCell(driver.GetFinish().ToString(), driver.GetFinish(), 0, Element.ALIGN_RIGHT, ref widths));
                table.AddCell(GenerateDriverCell(driver.number, driver.GetFinish(), 1, Element.ALIGN_RIGHT, ref widths));
                table.AddCell(GenerateDriverCell(driver.firstName + " " + driver.lastName, driver.GetFinish(), 2, Element.ALIGN_LEFT, ref widths));
                table.AddCell(GenerateDriverCell(driver.sponsor, driver.GetFinish(), 3, Element.ALIGN_LEFT, ref widths));
                table.AddCell(GenerateDriverCell(driver.team, driver.GetFinish(), 4, Element.ALIGN_LEFT, ref widths));

                table.AddCell(GenerateDriverCell(driver.GetTime(), driver.GetFinish(), 5, Element.ALIGN_RIGHT, ref widths));
                table.AddCell(GenerateDriverCell(driver.GetSpeed(), driver.GetFinish(), 6, Element.ALIGN_RIGHT, ref widths));

                string[] laps = GenerateLaps();


                table.AddCell(GenerateDriverCell(driver.GetOffLeader(), driver.GetFinish(), 7, Element.ALIGN_RIGHT, ref widths));
                table.AddCell(GenerateDriverCell(driver.GetOffNext(), driver.GetFinish(), 8, Element.ALIGN_RIGHT, ref widths));
            }

            return table;
        }

        private static PdfPCell GenerateDriverCell(string data, int verPos, int horizPos, int justify, ref float[] widths)
        {
            int border = 0;
            float fontSize = 9f;

            if (data!=null)
            {
                while ((FontFactory.GetFont(FontFactory.HELVETICA, fontSize).BaseFont.GetWidthPoint(data, fontSize)) > widths[horizPos] * 4.25)
                {
                    fontSize -= .1f;       
                }
            }
            
            //determine whether or not to have line underneath this row.
            if (verPos % 3 == 0)
            {
                border = Rectangle.BOTTOM_BORDER;
            }
            
           
            
            return new PdfPCell(new Phrase(data, FontFactory.GetFont(FontFactory.HELVETICA, fontSize)))
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
