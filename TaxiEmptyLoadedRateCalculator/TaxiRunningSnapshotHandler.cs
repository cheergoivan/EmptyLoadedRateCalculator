using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiEmptyLoadedRateCalculator
{
    class TaxiRunningSnapshotHandler
    {
        public static List<List<TaxiRunningStatistics>> analyse(TaxiRunningSnapshot[] snapshots)
        {
            List<List<TaxiRunningStatistics>> result = new List<List<TaxiRunningStatistics>>();
            int i = 0;
            while (i < snapshots.Length)
            {
                List<TaxiRunningStatistics> statisticInOneDay = new List<TaxiRunningStatistics>();
                DateTime currentDay = snapshots[i].Timestamp.Date,nextDay=currentDay.AddDays(1);
                TaxiRunningStatistics lastHourLegacy = null;Boolean hasLegacy = false;
                while (i < snapshots.Length && between(snapshots[i].Timestamp, currentDay, nextDay))
                {
                    TaxiRunningStatistics statisticInOneHour = new TaxiRunningStatistics();
                    DateTime[] floorAndCeiling = GetFloorAndCeiling(snapshots[i].Timestamp);
                    statisticInOneHour.TaxiId = snapshots[i].TaxiId;
                    statisticInOneHour.StartTime = floorAndCeiling[0];
                    statisticInOneHour.EndTime = floorAndCeiling[1];
                    Console.WriteLine("开始计算:" + statisticInOneHour.StartTime + "到" + statisticInOneHour.EndTime + "的空驶率...");
                    if (hasLegacy)
                    {
                        inheritLegacy(statisticInOneHour, lastHourLegacy);
                        hasLegacy = false;
                    }
                    TaxiRunningSnapshot lastSnapshot = null, currentSnapshot = null;
                    while (i < snapshots.Length && between(snapshots[i].Timestamp, statisticInOneHour.StartTime, statisticInOneHour.EndTime))
                    {
                        currentSnapshot = snapshots[i];
                        if (lastSnapshot != null)
                        {
                            double duration = currentSnapshot.Timestamp.Subtract(lastSnapshot.Timestamp).TotalSeconds;
                            double distance = DistanceCalculator.Measure(lastSnapshot.Location, currentSnapshot.Location);
                            if (lastSnapshot.TaxiState == TaxiState.Empty)
                            {
                                statisticInOneHour.EmptyRunningDuration += duration;
                                statisticInOneHour.EmptyRunningDistance += distance;
                            }
                            else
                            {
                                statisticInOneHour.LoadedRunningDuration += duration;
                                statisticInOneHour.LoadedRunningDistance += distance;
                            }
                            printMessage(lastSnapshot.TaxiState, duration, distance);
                        }
                        lastSnapshot = currentSnapshot;
                        i++;
                    }
                    if (i < snapshots.Length)
                    {
                        //currentSnapshot为一个新小时的开始，对上一个小时的末尾进行预测
                        currentSnapshot = snapshots[i];
                        if (currentSnapshot.Timestamp.Subtract(lastSnapshot.Timestamp).TotalSeconds<=Config.GetPredictableRange())
                        {
                            Console.WriteLine("增加预估时间和距离...");
                            lastHourLegacy = new TaxiRunningStatistics();
                            double extraInDuration = statisticInOneHour.EndTime.Subtract(lastSnapshot.Timestamp).TotalSeconds;
                            double duration = currentSnapshot.Timestamp.Subtract(lastSnapshot.Timestamp).TotalSeconds;
                            double legacyInDuration = duration- extraInDuration;
                            double distance = DistanceCalculator.Measure(lastSnapshot.Location, currentSnapshot.Location);
                            double legacyInDistance = distance * legacyInDuration/ duration;
                            double extraInDistance= distance * extraInDuration / duration;
                            if (lastSnapshot.TaxiState == TaxiState.Empty)
                            {
                                statisticInOneHour.EmptyRunningDuration += extraInDuration;
                                statisticInOneHour.EmptyRunningDistance += extraInDistance;
                                lastHourLegacy.EmptyRunningDuration = legacyInDuration;
                                lastHourLegacy.EmptyRunningDistance = legacyInDistance;
                            }
                            else
                            {
                                statisticInOneHour.LoadedRunningDuration += extraInDuration;
                                statisticInOneHour.LoadedRunningDistance += extraInDistance;
                                lastHourLegacy.LoadedRunningDuration = legacyInDuration;
                                lastHourLegacy.LoadedRunningDistance = legacyInDistance;
                            }
                            printMessage(lastSnapshot.TaxiState, extraInDuration, extraInDistance);
                            hasLegacy = true;
                        }
                    }
                    Console.WriteLine(statisticInOneHour.StartTime+"到"+statisticInOneHour.EndTime+"的数据计算完毕:");
                    Console.WriteLine(statisticInOneHour);
                    Console.WriteLine();
                    statisticInOneDay.Add(statisticInOneHour);
                }
                result.Add(statisticInOneDay);
            }
            return result;
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

        private static void inheritLegacy(TaxiRunningStatistics heritor, TaxiRunningStatistics legacy)
        {
            if (legacy != null)
            {
                heritor.EmptyRunningDistance += legacy.EmptyRunningDistance;
                heritor.EmptyRunningDuration += legacy.EmptyRunningDuration;
                heritor.LoadedRunningDistance += legacy.LoadedRunningDistance;
                heritor.LoadedRunningDuration += legacy.LoadedRunningDuration;
                Console.WriteLine("----继承空驶时间：" + legacy.EmptyRunningDuration + "s");
                Console.WriteLine("----继承空驶距离：" + legacy.EmptyRunningDistance + "m");
                Console.WriteLine("----继承重车时间：" + legacy.LoadedRunningDuration + "s");
                Console.WriteLine("----继承重车距离：" + legacy.LoadedRunningDistance + "m");
                Console.WriteLine();
            }
        }

        private static void printMessage(TaxiState state,double duration,double distance)
        {
            String stateMsg = state == TaxiState.Empty ? "空驶" : "重车";
            Console.WriteLine("----增加"+ stateMsg + "时间：" + duration + "s");
            Console.WriteLine("----增加"+ stateMsg + "距离：" + distance + "m");
            Console.WriteLine();
        }
        
    }
}
