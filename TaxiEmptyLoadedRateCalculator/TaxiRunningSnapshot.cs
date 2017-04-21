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

        public override string ToString()
        {
            return "TaxiRunningSnapshot [taxiId=" + taxiId+", timestamp="+timestamp.ToString()+", location="+location.ToString()
                +", speed="+speed+", taxiState="+taxiState+"]";
        }
    }

    enum TaxiState
    {
        Empty,Loaded
    }
}
