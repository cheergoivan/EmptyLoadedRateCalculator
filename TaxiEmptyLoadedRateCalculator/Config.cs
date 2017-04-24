using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TaxiEmptyLoadedRateCalculator
{
    class Config
    {
        public static int GetPredictableRange()
        {
            return int.Parse(GetAppConfig("PredictableRange"));
        }

        public static int GetDistanceDecimal()
        {
            return int.Parse(GetAppConfig("DistanceDecimal"));
        }

        public static int GetEmptyLoadedRateDecimal()
        {
            return int.Parse(GetAppConfig("EmptyLoadedRateDecimal"));
        }

        public static string GetSheetName(int excelIndex,int sheetIndex)
        {
            return GetAppConfig("excel" + excelIndex + "_sheet" + sheetIndex + "_name");
        }

        public static string GetSheetColumnName(int excelIndex,int sheetIndex,int columnIndex)
        {
            return GetAppConfig("excel"+excelIndex+"_sheet" + sheetIndex + "_column" + columnIndex);
        }

        public static String GetAppConfig(string strKey)
        {
            string[] keys = ConfigurationManager.AppSettings.AllKeys;
            foreach (string key in keys)
            {
                if (key == strKey)
                {
                    return ConfigurationManager.AppSettings[strKey];
                }
            }
            return null;
        }


    }
}
