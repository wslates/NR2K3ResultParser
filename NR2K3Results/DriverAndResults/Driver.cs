using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFileParser
{
    class Driver
    {
        public string firstName;
        public string lastName;
        public string number;
        public string sponsor;

        public override bool Equals(object obj)
        {
            if (!(obj is Driver)) { return false; }

            Driver driver = obj as Driver;

            if (driver.lastName.Equals(lastName) && driver.firstName[0].Equals(firstName[0]) && driver.number==number)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return "#" + number + "; " + firstName + " " + lastName + "; " + sponsor;
        }
    }

    enum Sessions { Practice, Qualifying, HappyHour }
        
}
