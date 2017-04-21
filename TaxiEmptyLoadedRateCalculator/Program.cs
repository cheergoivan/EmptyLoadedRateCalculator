using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace TaxiEmptyLoadedRateCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Location l1 = new Location(120.822662, 32.036146);
            Location l2 = new Location(120.705247, 32.058932);
            Console.WriteLine(DistanceCalculator.Measure(l1, l2));
            Console.Read();
        }

        
    }
}
