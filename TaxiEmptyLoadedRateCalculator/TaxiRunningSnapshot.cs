using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiEmptyLoadedRateCalculator
{
    class TaxiRunningSnapshot
    {
        private string taxiId;
        private DateTime timestamp;
        private Location location;
        private double speed;
        private TaxiState taxiState;

        public string TaxiId { get => taxiId; set => taxiId = value; }
        public DateTime Timestamp { get => timestamp; set => timestamp = value; }
        public Location Location { get => location; set => location = value; }
        public double Speed { get => speed; set => speed = value; }
        public TaxiState TaxiState { get => taxiState; set => taxiState = value; }
    }

    enum TaxiState
    {
        Empty,Loaded
    }
}
