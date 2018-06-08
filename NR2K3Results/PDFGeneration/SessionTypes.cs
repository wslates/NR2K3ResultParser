using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR2K3Results.PDFGeneration
{
    class SessionData
    {
        /// <summary>
        /// Practice and Happy Hour Data
        /// </summary>
        public static float[] PRACTICECOLUMNWIDTHS = { 5f, 6f, 17f, 30f, 25f, 10f, 10f, 10f, 10f };
        public static List<Tuple<string, int>> PRACTICECOLUMNS = new List<Tuple<string, int>>()
        {
            Tuple.Create("Pos", Element.ALIGN_RIGHT),
            Tuple.Create("Car", Element.ALIGN_RIGHT),
            Tuple.Create("Driver", Element.ALIGN_LEFT),
            Tuple.Create("Sponsor", Element.ALIGN_LEFT),
            Tuple.Create("Team", Element.ALIGN_LEFT),
            Tuple.Create("Time", Element.ALIGN_RIGHT),
            Tuple.Create("Speed", Element.ALIGN_RIGHT),
            Tuple.Create("-Fastest", Element.ALIGN_RIGHT),
            Tuple.Create("-Next", Element.ALIGN_RIGHT)
        };

        public static List<Tuple<string, int>> QUALIFYINGCOLUMNS = new List<Tuple<string, int>>()
        {
            Tuple.Create("Pos", Element.ALIGN_RIGHT),
            Tuple.Create("Car", Element.ALIGN_RIGHT),
            Tuple.Create("Driver", Element.ALIGN_LEFT),
            Tuple.Create("Sponsor", Element.ALIGN_LEFT),
            Tuple.Create("Time", Element.ALIGN_RIGHT),
            Tuple.Create("Speed", Element.ALIGN_RIGHT),
            Tuple.Create("-Fastest", Element.ALIGN_RIGHT),
            Tuple.Create("-Next", Element.ALIGN_RIGHT)
        };
    }
}
