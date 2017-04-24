using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiEmptyLoadedRateCalculator
{
    class TaxiRunningStatistics
    {
        private string taxiId;
        private DateTime startTime;
        private DateTime endTime;
        private double loadedRunningDistance;
        private double emptyRunningDistance;
        private double loadedRunningDuration;
        private double emptyRunningDuration;

        public string TaxiId { get => taxiId; set => taxiId = value; }
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }
        public double LoadedRunningDistance { get => loadedRunningDistance; set => loadedRunningDistance = value; }
        public double EmptyRunningDistance { get => emptyRunningDistance; set => emptyRunningDistance = value; }
        public double LoadedRunningDuration { get => loadedRunningDuration; set => loadedRunningDuration = value; }
        public double EmptyRunningDuration { get => emptyRunningDuration; set => emptyRunningDuration = value; }

        public override string ToString()
        {
            return "TaxiRunningStatistics [taxiId=" + taxiId + ", startTime=" + startTime.ToString() + ", endTime=" + endTime.ToString()
                + ", loadedRunningDistance=" + loadedRunningDistance + ", emptyRunningDistance=" + emptyRunningDistance + ", loadedRunningDuration=" +
                loadedRunningDuration + ", emptyRunningDuration=" + emptyRunningDuration + "]";
        }
    }
}
