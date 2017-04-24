using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace TaxiEmptyLoadedRateCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("请输入包含excel数据文件的目录：");
            String directory=Console.ReadLine();
            if(!Directory.Exists(directory))
            {
                Console.WriteLine("输入的不是一个合法目录！");
            }
            else
            {
                DirectoryInfo folder = new DirectoryInfo(directory);
                foreach (FileInfo file in folder.GetFiles())
                {
                   if (file.FullName.EndsWith("xlsx"))
                    {
                        Reader reader = new ReaderUsingInteropExcel();

                        TaxiRunningSnapshot[] snapshots = reader.Read(file.FullName);

                        List<List<TaxiRunningStatistics>> result = TaxiRunningSnapshotHandler.analyse(snapshots);
                        foreach (List<TaxiRunningStatistics> statisticsInOneDay in result)
                        {
                            Writer.newExcelWithStatisticsHourly(directory, statisticsInOneDay);
                        }
                    }
                                    
                }
            }
            
            /*
            TaxiRunningSnapshot[]  snapshots=Reader.Read(@"C:\Users\john\Desktop\test\6155.xlsx");
            List<List<TaxiRunningStatistics>> result=TaxiRunningSnapshotHandler.analyse(snapshots);
            foreach(List<TaxiRunningStatistics> statisticsInOneDay in result)
            {
                Writer.newExcelWithStatisticsHourly(@"C:\Users\john\Desktop\test", statisticsInOneDay);
            }
            */
            Console.Read();
        }
    }
}
