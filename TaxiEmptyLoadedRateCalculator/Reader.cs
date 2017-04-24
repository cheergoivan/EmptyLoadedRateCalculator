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
    interface Reader
    {
        TaxiRunningSnapshot[] Read(string file);
    }


    class ReaderUsingInteropExcel:Reader
    {
        public TaxiRunningSnapshot[] Read(String file)
        {
            Application excel = new Application();//lauch excel application
            TaxiRunningSnapshot[] result=null;
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
                int rows = ws.UsedRange.Cells.Rows.Count; //得到行数
                //int columnsint = mySheet.UsedRange.Cells.Columns.Count;//得到列数
                result = new TaxiRunningSnapshot[rows - 1];
                //取得数据范围区域 (不包括标题列) 
                Range taxiIdRange = ws.Cells.get_Range("A2", "A" + rows);
                Range timestampRange = ws.Cells.get_Range("B2", "B" + rows);   
                Range lngsRange = ws.Cells.get_Range("C2", "C" + rows);
                Range latRange = ws.Cells.get_Range("D2", "D" + rows);
                Range speedRange = ws.Cells.get_Range("E2", "E" + rows);
                Range taxiStateRange = ws.Cells.get_Range("G2", "G" + rows);
                Console.Write("正在读取文件 "+file+": ");
                int cursorLeft = Console.CursorLeft, cursorTop = Console.CursorTop;
                for (int i = 1; i <= rows-1; i++)
                {
                    if (i % 100 == 0)
                    {
                        Console.SetCursorPosition(cursorLeft, cursorTop);
                        Console.Write("{0}%", (int)(i/((double)rows)*100));

                    }
                    TaxiRunningSnapshot snapshot = new TaxiRunningSnapshot();
                    snapshot.TaxiId= (string)((object[,])taxiIdRange.Value2)[i, 1];
                    snapshot.Timestamp = DateTime.FromOADate((double)((object[,])timestampRange.Value2)[i, 1]);
                    snapshot.Location = new Location((double)((object[,])lngsRange.Value2)[i, 1],(double)((object[,])latRange.Value2)[i, 1]);
                    snapshot.Speed = (double)((object[,])speedRange.Value2)[i, 1];
                    snapshot.TaxiState= (string)((object[,])taxiStateRange.Value2)[i, 1]=="空车"?TaxiState.Empty:TaxiState.Loaded;
                    result[i-1] = snapshot;                
                }
            }
            excel.Quit();
            excel = null;
            GC.Collect();
            return result;
    } 
    }
}
