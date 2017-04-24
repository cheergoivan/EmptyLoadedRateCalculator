using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace TaxiEmptyLoadedRateCalculator
{
    class Writer
    {
        public static void newExcelWithStatisticsHourly(string directory,List<TaxiRunningStatistics> statisticsInOneDay)
        {
            if (statisticsInOneDay.Count == 0)
                return;
            String taxiId=statisticsInOneDay[0].TaxiId;
            String subDir=directory+"\\"+taxiId;
            if (!Directory.Exists(subDir))
            {
                Directory.CreateDirectory(subDir);
            }
            Application xls = new Application();//lauch excel application
            xls.Visible = false;
            xls.DisplayAlerts = false;
            string file = subDir+"\\"+statisticsInOneDay[0].StartTime.Date.ToString("yyyy-MM-dd");
            if (!File.Exists(file))
            {
                Workbook book = xls.Workbooks.Add(Missing.Value);
                //小时平均数据
                Worksheet sheet1 = book.Worksheets.get_Item(1);
                sheet1.Name = Config.GetSheetName(1,1);
                for(int i = 1; i <= 9; i++)
                {
                    sheet1.Cells[1, i] = Config.GetSheetColumnName(1,1,i);
                }
                double emptyRunningDistanceInOneDay = 0, loadedRunningDistanceInOneDay=0,
                    emptyRunningDurationInOneDay=0, loadedRunningDurationInOneDay=0;
                for (int i = 2,j=1; i < 2 + statisticsInOneDay.Count; i++)
                {
                    sheet1.Cells[i, j++] = statisticsInOneDay[i - 2].TaxiId;
                    sheet1.Cells[i, j++] = statisticsInOneDay[i - 2].StartTime;
                    sheet1.Cells[i, j++] = statisticsInOneDay[i - 2].EndTime;
                    emptyRunningDistanceInOneDay += statisticsInOneDay[i - 2].EmptyRunningDistance;
                    loadedRunningDistanceInOneDay += statisticsInOneDay[i - 2].LoadedRunningDistance;
                    sheet1.Cells[i, j++] = DistanceCalculator.FormatDistanceAccordingToCofig(statisticsInOneDay[i - 2].EmptyRunningDistance);
                    sheet1.Cells[i, j++] = DistanceCalculator.FormatDistanceAccordingToCofig(statisticsInOneDay[i - 2].LoadedRunningDistance);
                    sheet1.Cells[i, j++] = statisticsInOneDay[i - 2].EmptyRunningDuration;
                    sheet1.Cells[i, j++] = statisticsInOneDay[i - 2].LoadedRunningDuration;
                    emptyRunningDurationInOneDay += statisticsInOneDay[i - 2].EmptyRunningDuration;
                    loadedRunningDurationInOneDay += statisticsInOneDay[i - 2].LoadedRunningDuration;
                    double emptyLoadedRateInTime = calculateRate(statisticsInOneDay[i - 2].EmptyRunningDuration, statisticsInOneDay[i - 2].LoadedRunningDuration);
                    sheet1.Cells[i, j++] = emptyLoadedRateInTime + "%";
                    double emptyLoadedRateInDistance = calculateRate(statisticsInOneDay[i - 2].EmptyRunningDistance, statisticsInOneDay[i - 2].LoadedRunningDistance);
                    sheet1.Cells[i, j++] = emptyLoadedRateInDistance + "%";
                    j = 1;
                }
                //添加日平均数据
                Worksheet sheet2 = book.Worksheets.Add(Missing.Value, book.Worksheets.get_Item(book.Worksheets.Count),Missing.Value,Missing.Value);
                sheet2.Name = Config.GetSheetName(1, 2);
                for (int i = 1; i <= 9; i++)
                {
                    sheet2.Cells[1, i] = Config.GetSheetColumnName(1, 1, i);
                }
                for (int i = 2, j = 1; i <=2; i++)
                {
                    sheet2.Cells[i, j++] = statisticsInOneDay[0].TaxiId;
                    sheet2.Cells[i, j++] = statisticsInOneDay[0].StartTime;
                    sheet2.Cells[i, j++] = statisticsInOneDay[statisticsInOneDay.Count-1].EndTime;
                    sheet2.Cells[i, j++] = DistanceCalculator.FormatDistanceAccordingToCofig(emptyRunningDistanceInOneDay);
                    sheet2.Cells[i, j++] = DistanceCalculator.FormatDistanceAccordingToCofig(loadedRunningDistanceInOneDay);
                    sheet2.Cells[i, j++] = emptyRunningDurationInOneDay;
                    sheet2.Cells[i, j++] = loadedRunningDurationInOneDay;
                    double emptyLoadedRateInTime = calculateRate(emptyRunningDurationInOneDay, loadedRunningDurationInOneDay);
                    sheet2.Cells[i, j++] = emptyLoadedRateInTime + "%";
                    double emptyLoadedRateInDistance = calculateRate(emptyRunningDistanceInOneDay, loadedRunningDistanceInOneDay);
                    sheet2.Cells[i, j++] = emptyLoadedRateInDistance + "%";
                    j = 1;
                }

                book.SaveAs(file, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                book.Close(false, Missing.Value, Missing.Value);//关闭打开的表
            }
            xls.Quit();
            xls = null;
            GC.Collect();
        }

        private static double calculateRate(double d1,double d2)
        {
            return Math.Round(d1 / (d1 + d2)*100, Config.GetEmptyLoadedRateDecimal());
        }
    }
}
