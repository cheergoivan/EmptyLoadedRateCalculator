using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiEmptyLoadedRateCalculator
{
    class Location
    {
        private double longititude;
        private double latitude;

        public Location(double longitude, double latitude)
        {
            this.longititude = longitude;
            this.latitude = latitude;
        }

        public double Longitude { get => longititude;}

        public double Latitude { get => latitude; }

        
    }
}
