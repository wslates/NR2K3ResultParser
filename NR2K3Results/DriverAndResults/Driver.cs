using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR2K3Results.DriverAndResults
{
    class Driver: IComparable<Driver>
    {
        public string firstName;
        public string lastName;
        public string number;
        public string sponsor;
        public string team;
        public DriverResult result;

        public int CompareTo(Driver other)
        {
            if (other.result.finish>result.finish)
            {
                return -1;
            } else
            {
                return 1;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Driver)) { return false; }

            Driver driver = obj as Driver;

            if (driver.lastName.Equals(lastName) && 
                driver.firstName[0]==firstName[0] && 
                driver.number.Equals(number))
            {
                return true;
            }

            return false;
        }

        

        public override string ToString()
        {
            return  "#" + number + 
                    "; " + firstName + " " + lastName + 
                    "; " + sponsor + 
                    "; " + team +
                    "; " + result.finish + 
                    "; " + result.time + 
                    "; " + result.timeOffLeader + 
                    "; " + result.timeOffNext;
        }

        
    }

    enum Sessions { Practice, Qualifying, HappyHour }
        
}
