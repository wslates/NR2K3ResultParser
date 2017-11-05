using CarFileParser;
using HtmlAgilityPack;
using NR2K3Results.DriverAndResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR2K3ResultParser
{
    class ResultParser
    {

        public static void Parse(List<Driver> drivers, string FilePath)
        {
            HtmlDocument doc = new HtmlDocument();


            doc.Load(FilePath);

            //get html table nodes
            HtmlNodeCollection tables = doc.DocumentNode.SelectNodes("//table");
            List<string> results = null;

            //select data from specified session
            //table argmuents:
            //              0 = practice
            //              1 = qualifying
            //              2 = happy hour
            //              3 = race
            foreach (HtmlNode node in tables[0].SelectNodes("tr"))
            {
                results = new List<string>(node.InnerText.Split('\n'));
            }

            //parse the results
            List<string> finalResults = new List<string>();
            foreach (string result in results)
            {
                //issue is that some of the data are just spaces, filter those out
                if (!result.Trim().Equals(""))
                {
                    finalResults.Add(result.Trim());
                }
            }

            DriverResult drivResult = new DriverResult();
            Driver driver = new Driver();
            for (int i = 0; i < finalResults.Count; i++)
            {
                if ((i + 1) % 4 == 1)
                {
                    //position
                    drivResult.finish = finalResults.ElementAt(i);
                }
                else if ((i + 1) % 4 == 2)
                {
                    //car number
                    driver.number = finalResults.ElementAt(i);
                }
                else if ((i + 1) % 4 == 3)
                {
                    //name
                    string[] name = finalResults.ElementAt(i).Split(' ');
                    driver.firstName = name[0];
                    driver.lastName = name[1];
                }
                else if ((i + 1) % 4 == 0)
                {
                    //lap time
                    drivResult.time = finalResults.ElementAt(i);
                }

            }

        }
    }
}
    


