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
        public static string GetDistanceUnit()
        {
            return GetAppConfig("DistanceUnit");
        }

        public static int GetDecimal()
        {
            return int.Parse(GetAppConfig("Decimal"));
        }

        public static string GetSheet1Name()
        {
            return GetAppConfig("sheet1Name");
        }

        public static string GetSheetColumnName(int sheetIndex,int columnIndex)
        {
            return GetAppConfig("sheet" + sheetIndex + "_column" + columnIndex);
        }

        public static int ColumnNum(int sheetIndex)
        {
            return 0;
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
