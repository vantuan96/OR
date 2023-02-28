using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;

namespace Admin.Helper
{
    public static class ExportExcelHelper
    {
        public static void FormatCenter(this ExcelWorksheet worksheet, int colnumber, int startRow, int endRow)
        {
            if (startRow <= endRow)
            {
                using (var range = worksheet.Cells[startRow, colnumber, endRow, colnumber])
                {
                    worksheet.FormatCenter(range);
                }
            }
        }
        public static void FormatCenter(this ExcelWorksheet worksheet, ExcelRange range)
        {
            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        public static void FormatDateTime(this ExcelWorksheet worksheet, int colnumber, int startRow, int endRow)
        {
            if (startRow <= endRow)
            {
                using (var range = worksheet.Cells[startRow, colnumber, endRow, colnumber])
                {
                    worksheet.FormatDateTime(range);
                }
            }
        }

        public static void FormatDate(this ExcelWorksheet worksheet, int colnumber, int startRow, int endRow)
        {
            if (startRow <= endRow)
            {
                using (var range = worksheet.Cells[startRow, colnumber, endRow, colnumber])
                {
                    worksheet.FormatDate(range);
                }
            }
        }

        public static void FormatDateTime(this ExcelWorksheet worksheet, ExcelRange range)
        {
            range.Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
        }

        public static void FormatDate(this ExcelWorksheet worksheet, ExcelRange range)
        {
            range.Style.Numberformat.Format = "dd/mm/yyyy";
        }

        public static void FormatNumber(this ExcelWorksheet worksheet, int colnumber, int startRow, int endRow)
        {
            if (startRow <= endRow)
            {
                using (var range = worksheet.Cells[startRow, colnumber, endRow, colnumber])
                {
                    worksheet.FormatNumber(range);
                }
            }
        }
        public static void FormatFloatNumber(this ExcelWorksheet worksheet, int colnumber, int startRow, int endRow)
        {
            if (startRow <= endRow)
            {
                using (var range = worksheet.Cells[startRow, colnumber, endRow, colnumber])
                {
                    worksheet.FormatFloatNumber(range);
                }
            }
        }
        public static void FormatNumber(this ExcelWorksheet worksheet, ExcelRange range)
        {
            range.Style.Numberformat.Format = "_(* #,##0_);_(* (#,##0);_(* \"-\"??_);_(@_)";
        }
        public static void FormatFloatNumber(this ExcelWorksheet worksheet, ExcelRange range)
        {
            range.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)";
        }



        public static ExcelPackage CreateExcelPackage(DataTable dt)
        {
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            if (dt != null)
            {

                if (dt.Columns[0].ColumnName == "TotalRows")
                    dt.Columns.RemoveAt(0);
                //startColumn = 1; // bỏ qua cột totalrows

                int excelRowCount = dt.Rows.Count + 1;
                using (var header = workSheet.Cells[1, 1, 1, dt.Columns.Count])
                {
                    header.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    header.Style.Font.Bold = true;
                    header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    var headerBgColor = System.Drawing.ColorTranslator.FromHtml("#5e9bd3");
                    header.Style.Fill.BackgroundColor.SetColor(headerBgColor);
                    header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var type = dt.Columns[i].DataType;

                    switch (Type.GetTypeCode(type))
                    {
                        case TypeCode.Boolean:
                        case TypeCode.Char:
                            workSheet.FormatCenter(i + 1, 2, excelRowCount);
                            break;
                        case TypeCode.DateTime:
                            workSheet.FormatDateTime(i + 1, 2, excelRowCount);
                            break;
                        case TypeCode.Byte:
                        case TypeCode.Decimal:
                        case TypeCode.Double:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.SByte:
                        case TypeCode.Single:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                        case TypeCode.String:
                            break;
                    }
                }

                workSheet.Cells[1, 1].LoadFromDataTable(dt, true);
            }

            return excel;
        }


    }
}