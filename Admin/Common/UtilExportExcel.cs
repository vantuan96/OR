using OfficeOpenXml;
using VG.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;
using OfficeOpenXml.Style;

namespace Admin.Common
{
    public class UtilExportExcel
    {       
        public void LoadFromDataTable(HttpResponseBase respone, DataTable dt, string fileName, string title = "", string sheetName = "Sheet1")
        {
            var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add(sheetName);

            var startRowIndex = 1;            
            if (!string.IsNullOrEmpty(title))
            {
                workSheet.Cells[startRowIndex, 1].Value = title;
                startRowIndex += 2;
            }

            var endRowIndex = startRowIndex + dt.Rows.Count;
            /* Formart for header */
            using (var header = workSheet.Cells[startRowIndex, 1, startRowIndex, dt.Columns.Count])
            {
                header.AutoFitColumns();
                header.Style.Font.Color.SetColor(System.Drawing.Color.White);
                header.Style.Font.Bold = true;
                header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                var headerBgColor = System.Drawing.ColorTranslator.FromHtml("#5e9bd3");
                header.Style.Fill.BackgroundColor.SetColor(headerBgColor);
                header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            // Assign borders
            var excelRange = workSheet.Cells[startRowIndex, 1, endRowIndex, dt.Columns.Count];            
            excelRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            excelRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            excelRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            excelRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Load data from datatable
            workSheet.Cells[startRowIndex, 1].LoadFromDataTable(dt, true);

            // Set auto column
            workSheet.Cells.AutoFitColumns();

            // Lưu lại vào file
            Save(excel, respone, fileName);
        }
        private void Save(ExcelPackage excel, HttpResponseBase respone, string fileName)
        {            
            using (var memoryStream = new MemoryStream())
            {
                respone.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                respone.AddHeader("content-disposition", "attachment;  filename=" + fileName.UnicodeToKoDauAndGach() + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(respone.OutputStream);
                respone.Flush();
                respone.End();
            }
        }
    }
}