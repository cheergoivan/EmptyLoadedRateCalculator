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
    class Reader
    {
        public static TaxiRunningSnapshot[] read(String file)
        {
            Application excel = new Application();//lauch excel application
            if (excel == null)
            {
                Console.Write("Can't access excel!");
            }
            else
            {
                excel.Visible = false; excel.UserControl = true;

                // 以只读的形式打开EXCEL文件
                Workbook wb = excel.Application.Workbooks.Open(file, Missing.Value, true, Missing.Value, Missing.Value, Missing.Value,
                 Missing.Value, Missing.Value, Missing.Value, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                //取得第一个工作薄
                Worksheet ws = (Worksheet)wb.Worksheets.get_Item(1);


                //取得总记录行数   (包括标题列)
                int rowsint = ws.UsedRange.Cells.Rows.Count; //得到行数
                                                             //int columnsint = mySheet.UsedRange.Cells.Columns.Count;//得到列数


                //取得数据范围区域 (不包括标题列) 
                Range rng1 = ws.Cells.get_Range("B2", "B" + rowsint);   //item


                Range rng2 = ws.Cells.get_Range("C2", "C" + rowsint); //Customer
                object[,] arryItem = (object[,])rng1.Value2;   //get range's value
                object[,] arryCus = (object[,])rng2.Value2;
                //将新值赋给一个数组
                string[,] arry = new string[10, 2];
                for (int i = 1; i <= 10; i++)
                {
                    DateTime d = DateTime.FromOADate((double)arryItem[i, 1]);
                    Console.WriteLine(arryItem[i, 1].GetType());
                    //Item_Code列
                    arry[i - 1, 0] = arryItem[i, 1].ToString();
                    //Customer_Name列
                    arry[i - 1, 1] = arryCus[i, 1].ToString();
                }
                for (int i = 0; i < arry.GetLength(0); i++)
                {
                    for (int j = 0; j < arry.GetLength(1); j++)
                    {
                        Console.WriteLine("array[{0},{1}]={2}", i, j, arry[i, j]);
                    }
                }
            }
            excel.Quit();
            excel = null;
            GC.Collect();

    } 
    }
}
