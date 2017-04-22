﻿using System;
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
        public static void newExcelWithStatisticsHourly(string directory,TaxiRunningStatistics[] statisticsInOneDay)
        {
            if (statisticsInOneDay.Length == 0)
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
            string file = subDir+"\\"+statisticsInOneDay[0].StartTime.Date.ToString();
            if (!File.Exists(file))
            {
                Workbook book = xls.Workbooks.Add(Missing.Value);
                Worksheet sheet = book.Worksheets.get_Item(1);
                sheet.Name = Config.GetSheetName(1,1);
                for(int i = 1; i <= 9; i++)
                {
                    sheet.Cells[1, i] = Config.GetSheetColumnName(1,1,i);
                }
                for(int i = 2,j=1; i < 2 + statisticsInOneDay.Length; i++)
                {
                    sheet.Cells[i, j++] = statisticsInOneDay[i - 2].TaxiId;
                    sheet.Cells[i, j++] = statisticsInOneDay[i - 2].StartTime;
                    sheet.Cells[i, j++] = statisticsInOneDay[i - 2].EndTime;
                    sheet.Cells[i, j++] = statisticsInOneDay[i - 2].EmptyRunningDistance;
                    sheet.Cells[i, j++] = statisticsInOneDay[i - 2].LoadedRunningDistance;
                    sheet.Cells[i, j++] = statisticsInOneDay[i - 2].EmptyRunningDuration;
                    sheet.Cells[i, j++] = statisticsInOneDay[i - 2].LoadedRunningDuration;
                    double emptyLoadedRateInTime = Math.Round(statisticsInOneDay[i - 2].EmptyRunningDuration /
                        ((double)(statisticsInOneDay[i - 2].EmptyRunningDuration + statisticsInOneDay[i - 2].LoadedRunningDuration))*100, Config.GetEmptyLoadedRateDecimal());
                    sheet.Cells[i, j++] = emptyLoadedRateInTime + "%";
                    double emptyLoadedRateInDistance = Math.Round(statisticsInOneDay[i - 2].EmptyRunningDistance /
                        ((double)(statisticsInOneDay[i - 2].EmptyRunningDistance + statisticsInOneDay[i - 2].LoadedRunningDistance)) * 100, Config.GetEmptyLoadedRateDecimal());
                    sheet.Cells[i, j++] = emptyLoadedRateInDistance + "%";
                }
                book.SaveAs(file, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                book.Close(false, Missing.Value, Missing.Value);//关闭打开的表
            }
            xls.Quit();
            xls = null;
            GC.Collect();
        }
    }
}
