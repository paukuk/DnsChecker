using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace DnsChecker
{
    public class Excel
    {
        string path = "";
        // excel is a reference to application
        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;
        private const double GreenColourCode = 5296274;
        private const double RedColourCode = 255;
        private const double WhiteColourCode = 16777215;
        
        private const UInt16 NumberOfRequests = 3;

        public Excel(string path, int Sheet)
        {
            this.path = path;
            wb = excel.Workbooks.Open(path);
            ws = wb.Worksheets[Sheet];            
        }

        private static Range GetNonEmptyRangeInColumn(_Application application, _Worksheet worksheet, int colIndex)
        {
            // get the intersection of the column and the used range on the sheet (this is a superset of the non-null cells)
            var tempRange = application.Intersect(worksheet.UsedRange, (Range)worksheet.Columns[colIndex]);

            // if there is no intersection, there are no values in the column
            if (tempRange == null)
                return null;

            // get complete set of values from the temp range (potentially memory-intensive)
            var value = tempRange.Value2;

            // if value is NULL, it's a single cell with no value
            if (value == null)
                return null;

            // if value is not an array, the temp range was a single cell with a value
            if (!(value is Array))
                return tempRange;

            // otherwise, the temp range is a 2D array which may have leading or trailing empty cells
            var value2 = (object[,])value;

            // get the first and last rows that contain values
            var rowCount = value2.GetLength(0);
            int firstRowIndex;
            for (firstRowIndex = 1; firstRowIndex <= rowCount; ++firstRowIndex)
            {
                if (value2[firstRowIndex, 1] != null)
                    break;
            }
            int lastRowIndex;
            for (lastRowIndex = rowCount; lastRowIndex >= firstRowIndex; --lastRowIndex)
            {
                if (value2[lastRowIndex, 1] != null)
                    break;
            }

            // if there are no first and last used row, there is no used range in the column
            if (firstRowIndex > lastRowIndex)
                return null;

            // return the range
            return worksheet.Range[tempRange[firstRowIndex, 1], tempRange[lastRowIndex, 1]];
        }

        public void ReadCells(Dictionary<int, string> dic)
        {            
            Range rangeSelection = GetNonEmptyRangeInColumn(excel, ws, 1);            
            foreach (Range row in rangeSelection.Rows)
            {
                if ((row.Value2 != null) && (row.Interior.Color == Excel.WhiteColourCode))
                {
                    dic.Add(row.Row, row.Value2.ToString());
                    if (dic.Count >= NumberOfRequests) break;
                }
            }
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(rangeSelection);
        }

        public void WriteToCell(int i, int j, string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                ws.Cells[i, j].Value2 = s;
            }
        }

        public void SetCellColour(int i, int j, double colour)
        {
            ws.Cells[i, j].Interior.Color = colour;
            if (!((j - 1) < 1))
            {
                ws.Cells[i, j-1].Interior.Color = colour;
            }
        }

        public void WriteToCells(int column, Dictionary<int, string> dic)
        {
            foreach (var cell in dic)
            {
                WriteToCell(cell.Key, column, cell.Value);
                if (cell.Value != String.Empty)
                {
                    SetCellColour(cell.Key, column, Excel.GreenColourCode);
                }
                else
                {
                    SetCellColour(cell.Key, column, Excel.RedColourCode);
                }
            }
            dic.Clear();
        }

        public void Save()
        {
            wb.Save();
        }

        public void FreeExcelResources()
        {
            // Cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();
                        
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(ws);

            wb.Close(Type.Missing, Type.Missing, Type.Missing);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(wb);

            excel.Quit();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
        }
    }
}
