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
        //移动距离的单位，m或者km
        private static string distanceUnit;

        //移动距离保留的小数位数
        private static int _decimal;

        public static string DistanceUnit { get => GetAppConfig("DistanceUnit"); }

        public static int Decimal { get => int.Parse(GetAppConfig("Decimal")); }

        private static String GetAppConfig(string strKey)
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
