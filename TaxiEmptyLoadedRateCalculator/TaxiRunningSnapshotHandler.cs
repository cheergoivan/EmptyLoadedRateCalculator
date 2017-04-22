using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiEmptyLoadedRateCalculator
{
    class TaxiRunningSnapshotHandler
    {
        public static TaxiRunningStatistics[] analyse(TaxiRunningSnapshot[] snapshots)
        {
            List<TaxiRunningStatistics> list = new List<TaxiRunningStatistics>();
            for(int i = 0; i < snapshots.Length; i++)
            {
                TaxiRunningSnapshot item = snapshots[i];
                TaxiRunningStatistics statistic = new TaxiRunningStatistics();
                DateTime[] floorAndCeiling = GetFloorAndCeiling(item.Timestamp);
                statistic.StartTime = floorAndCeiling[0];
                statistic.EndTime = floorAndCeiling[1];
                while(i<snapshots.Length&&between(snapshots[i].Timestamp,statistic.StartTime,statistic.EndTime))
                {

                    i++;
                }
            }
            return list.ToArray();
        }

        private static DateTime[] GetFloorAndCeiling(DateTime d)
        {
            DateTime[] result = new DateTime[2];
            result[0] = d.AddSeconds(-d.Minute*60-d.Second);
            result[1] = result[0].AddHours(1);
            return result;
        }

        private static Boolean between(DateTime curr,DateTime start,DateTime end)
        {
            return curr.CompareTo(start) >= 0 && curr.CompareTo(end) <= 0;
        }
    }
}
