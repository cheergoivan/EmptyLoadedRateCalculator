using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiEmptyLoadedRateCalculator
{
    class TaxiSnapshot
    {
        private string taxiId;
        private DateTime timestamp;
        private Location location;
        private TaxiState taxiState;

        public string TaxiId { get => taxiId; set => taxiId = value; }
        public DateTime Timestamp { get => timestamp; set => timestamp = value; }
        public Location Location { get => location; set => location = value; }

    }

    enum TaxiState
    {
        Empty,Loaded
    }
}
