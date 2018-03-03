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

        public static void Parse(ref List<Driver> drivers, string FilePath)
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




            double fastTime = Convert.ToDouble(finalResults.GetRange(0, 4)[3]);
            double prevTime = fastTime;
            for (int i = 0; i < finalResults.Count-3; i+=4)
            {
                
                string[] result = finalResults.GetRange(i, 4).ToArray();
                DriverResult driverRes = new DriverResult
                {
                    finish = Convert.ToInt16(result[0]),
                    time = Convert.ToDouble(result[3]),
                    timeOffLeader = fastTime - Convert.ToDouble(result[3]),
                    timeOffNext = prevTime - Convert.ToDouble(result[3])
                };

                string[] name = result[2].Split(' ');

                Driver driver = new Driver
                {
                    number = result[1],
                    firstName = result[2][0].ToString(),
                    lastName = result[2].Substring(2, result[2].Length-2),
                    result = driverRes
                };

                prevTime = Convert.ToDouble(result[3]);

  
                if (drivers.Contains(driver))
                {
                    drivers[drivers.IndexOf(driver)].result = driverRes;
                }

                
            }

            //in case some drivers were in the roster but not in the race, remove them
            drivers = drivers.Where(d => d.result != null).ToList();
        }
    }
}
    


