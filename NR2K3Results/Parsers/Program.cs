using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NR2K3Results.DriverAndResults;

namespace CarFileParser
{
    class CarFileParser
    {
        public static List<Driver> GetRosterDrivers(string CarsFilePath, string RosterFilePath)
        {
            string[] lines = System.IO.File.ReadAllLines(RosterFilePath);
            List<Driver> drivers = new List<Driver>();

            foreach (string line in lines)
            {
                //plus sign at beginning of every line, get rid of that and append to path
                drivers.Add(OpenCarFile(CarsFilePath + "\\" + line.Substring(1, line.Length - 1)));
            }

            return drivers;
        }
           

        private static Driver OpenCarFile(string path)
        {
            Driver driver = new Driver();
            //only need 63 of the 3433 lines
            var lines = System.IO.File.ReadLines(path).Take(63);

            //parses data by splitting at equals sign, getting rid of variable name
            foreach (string line in lines)
            {
                if (line.Contains("car_number"))
                {
                    driver.number = line.Split('=')[1].Replace(" ", string.Empty);
                } else if (line.Contains("first_name"))
                {
                    driver.firstName = line.Split('=')[1];
                } else if (line.Contains("last_name"))
                {
                    driver.lastName = line.Split('=')[1];
                } else if (line.Contains("sponsor"))
                {
                    driver.sponsor = line.Split('=')[1];
                    break;
                }    
            }
            return driver;

        }
    }
}
