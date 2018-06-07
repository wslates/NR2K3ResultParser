using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NR2K3Results.DriverAndResults;
using iTextSharp.text;
using System.IO;

namespace NR2K3Results.PDFGeneration
{
    class QualifyingPDFGenerator
    {
        public void OutputPDF(List<Driver> drivers, string series, string selectedSession, string raceName, string track)
        {
           
        }

        private static void QualifyingPDFGen(List<Driver> drivers, string series, string selectedSession, string raceName, string track)
        {
            Document document = new Document(PageSize.A4, 15, 25, 15, 30);
            FileStream fs = null;
        }
    }
}
