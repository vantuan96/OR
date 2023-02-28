using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VG.Common
{
    public static class Extension
    {
        public static string GetDayOfWeek(this DateTime date)
        {
            string str = "";

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    str = "Chủ nhật";
                    break;
                case DayOfWeek.Monday:
                    str = "Thứ hai";
                    break;
                case DayOfWeek.Tuesday:
                    str = "Thứ ba";
                    break;
                case DayOfWeek.Wednesday:
                    str = "Thứ tư";
                    break;
                case DayOfWeek.Thursday:
                    str = "Thứ năm";
                    break;
                case DayOfWeek.Friday:
                    str = "Thứ sáu";
                    break;
                case DayOfWeek.Saturday:
                    str = "Thứ bảy";
                    break;
            }

            return str;
        }

        public static DateTime GetFirstDateOfWeek(this DateTime date)
        {
            return date.AddDays(DayOfWeek.Sunday - date.DayOfWeek);
        }
        public static bool AreFallingInSameWeek(DateTime date1, DateTime date2)
        {
            return date1.AddDays(-(int)date1.DayOfWeek) == date2.AddDays(-(int)date2.DayOfWeek);
        }
      
        /// <summary>
        /// dd/MM/yyyy hh:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToVEFullDateTime(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm:ss");
        }


        /// <summary>
        /// yyyy-MM-dd hh:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToFullDateTime(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// DateShortFormat = "dd/MM/yyyy"
        /// </summary>
        public const string DateShortFormat = "dd/MM/yyyy";

        /// <summary>
        /// DateShortFormatJS = "dd/mm/yyyy"
        /// </summary>
        public const string DateShortFormatJS = "dd/mm/yyyy";

        /// <summary>
        /// DateShortFormatJS = "mm/dd/yyyy"
        /// </summary>
        public const string DateSubmitFormatJS = "mm/dd/yyyy";

        /// <summary>
        /// DateTimeShortFormat = "dd/MM/yyyy hh:mm:ss tt"
        /// </summary>
        public const string DateTimeShortFormat = "dd/MM/yyyy hh:mm:ss tt";

        /// <summary>
        /// MinDateTime = new DateTime(1900,1,1)
        /// </summary>
        public static DateTime MinDateTime = new DateTime(1900, 1, 1);

        /// <summary>
        /// Adds the time to the DateTime object to move it to END of day.
        /// </summary>
        /// <param name="dateTime">The DateTime object to add time</param>
        /// <returns></returns>
        public static DateTime AddTimeToTheEndOfDay(this DateTime dateTime)
        {
            var result = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            return result.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public static int MonthOfWeek(int weekOfYear, int year)
        {
            GregorianCalendar gc = new GregorianCalendar();
            for (DateTime dt = new DateTime(year, 1, 1); dt.Year == year; dt = dt.AddDays(1))
            {
                if (gc.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday) == weekOfYear)
                {
                    return dt.Month;
                }
            }
            return -1;

            //DateTime jan1 = new DateTime(year, 1, 1);
            //int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            //DateTime firstThursday = jan1.AddDays(daysOffset);
            //var cal = CultureInfo.CurrentCulture.Calendar;
            //int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            //var weekNum = weekOfYear;
            //if (firstWeek <= 1)
            //{
            //    weekNum -= 1;
            //}
            //var result = firstThursday.AddDays(weekNum * 7);
            //return result.AddDays(-3).Month;
        }
        
        /// <summary>
        /// Adds/Subtract the time to the DateTime object to move it to START of day.
        /// </summary>
        /// <param name="dateTime">The DateTime object to add/subtract time</param>
        /// <returns></returns>
        public static DateTime AddTimeToTheStartOfDay(this DateTime date)
        {
            return  new DateTime(date.Year, date.Month, date.Day,0,0,0);
            
          
        }


    

        /// <summary>
        /// Convert DateTime value to short date string using format [dd/MM/yyyy] defined in <see cref="CommonHelper.DateShortFormat"/>.
        /// </summary>
        /// <param name="date">The DateTime object to convert</param>
        /// <returns></returns>
        public static string ToVnShortDateString(this DateTime date)
        {
            return date.ToString(DateShortFormat);
        }

        /// <summary>
        /// Convert DateTime value to short date string using format [dd/MM/yyyy] defined in <see cref="CommonHelper.DateShortFormat"/>.
        /// </summary>
        /// <param name="date">The DateTime object to convert</param>
        /// <returns></returns>
        public static string ToVnShortDateStringWithCheckNull(this DateTime date)
        {
            if (date != DateTime.MinValue && date != MinDateTime)
                return date.ToString(DateShortFormat);
            return "";
        }

        /// <summary>
        /// Convert DateTime value to short date time string using format [dd/MM/yyyy hh:mm:ss tt] defined in <see cref="CommonHelper.DateShortFormat"/>.
        /// </summary>
        /// <param name="dateTime">The DateTime object to convert</param>
        /// <returns></returns>
        public static string ToVnShortDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString(DateTimeShortFormat);
        }

        /// <summary>
        /// Convert DateTime value to short date time string using format [dd/MM/yyyy hh:mm:ss tt] defined in <see cref="CommonHelper.DateTimeShortFormat"/>.
        /// </summary>
        /// <param name="dateTime">The DateTime object to convert</param>
        /// <returns></returns>
        public static string ToVnShortDateTimeStringWithCheckNull(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime == MinDateTime)
                return "";

            return dateTime.ToString(DateTimeShortFormat);
        }

        ///// <summary>
        ///// Convert DateTime value to short date string using format [dd/MM/yyyy] defined in <see cref="CommonHelper.DateShortFormat"/>.
        ///// </summary>
        ///// <param name="date">The DateTime object to convert</param>
        ///// <returns></returns>
        //public static string ToVnShortDateString(this DateTime? date)
        //{
        //    if (date.HasValue && date.Value != DateTime.MinValue && date.Value != MinDateTime)
        //        return date.Value.ToVnShortDateString();
        //    return "";
        //}

        ///// <summary>
        ///// Convert DateTime value to short date time string using format [dd/MM/yyyy hh:mm:ss tt] defined in <see cref="CommonHelper.DateTimeShortFormat"/>.
        ///// </summary>
        ///// <param name="dateTime">The DateTime object to convert</param>
        ///// <returns></returns>
        //public static string ToVnShortDateTimeString(this DateTime? date)
        //{
        //    if (date.HasValue && date.Value != DateTime.MinValue && date.Value != MinDateTime)
        //        return date.Value.ToVnShortDateTimeString();
        //    return "";
        //}

        public static string ToVEShortDate(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static string ToVEShortTime(this DateTime date)
        {
            return date.ToString("HH:mm");
        }

        /// <summary>
        /// dd/MM/yyyy HH:mm
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToVEShortDateTimeNotSecond(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm");
        }
        public static string ToVEShortDateTimeNotSecondV2(this DateTime date)
        {
            return date.ToString("HH:mm dd/MM/yyyy");
        }
        public static string ToVEShortDateTimeNotSecondV3(this DateTime date)
        {
            return string.Format("{0} ngày {1} tháng {2} năm {3}", date.ToString("HH:mm"), date.Day, date.Month, date.Year);
        }

        /// <summary>
        /// Ngày đầu tiên của tháng hiện tại
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        public static Boolean EqualDate(this DateTime date)
        {
            if (date == null) return false;
            var now = DateTime.Now;
            return (date.Year == now.Year && date.Month == now.Month && date.Day == now.Day);
        }
        public static Boolean EqualMonth(this DateTime date)
        {
            if (date == null) return false;
            var now = DateTime.Now;
            return (date.Year == now.Year && date.Month == now.Month);
        }
        public static DateTime CloneTime (this DateTime date,string strTime)
        {
            var arrTime = strTime.Split(':');
            return new DateTime(date.Year, date.Month, date.Day, Int32.Parse(arrTime[0]), Int32.Parse(arrTime[1]),0);
        }
       
        /// <summary>
        /// Ngày cuối cùng của tháng hiện tại
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToLastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }
        public static DateTime GetLastDateOfWeek(this DateTime date)
        {
            return date.AddDays(DayOfWeek.Sunday - date.DayOfWeek);
        }
        public static DateTime FirstDayOfWeek(DateTime date)
        {
            DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int offset = fdow - date.DayOfWeek;
            DateTime fdowDate = date.AddDays(offset);
            return fdowDate;
        }

        public static DateTime LastDayOfWeek(DateTime date)
        {
            DateTime ldowDate = FirstDayOfWeek(date).AddDays(6);
            return ldowDate;
        }


        public static List<WeekRange> GetWeekDays(DateTime startDate, DateTime endDate)
        {
            DateTime startDateToCheck = startDate;
            DateTime dateToCheck = startDate;
            DateTime dateRangeBegin = dateToCheck;
            DateTime dateRangeEnd = endDate;

            List<WeekRange> weekRangeList = new List<WeekRange>();
            WeekRange weekRange = new WeekRange();


            while (dateToCheck <= endDate)
            {
                int week = GetWeekOfMonth(dateToCheck);//current week;

                while (startDateToCheck.Month == dateToCheck.Month && dateToCheck <= endDate)
                {

                    //dateRangeBegin = dateToCheck.AddDays(-(int)dateToCheck.DayOfWeek);
                    dateRangeBegin = dateToCheck;//.AddDays(-(int)dateToCheck.DayOfWeek);
                    //set ngày bắt đầu = startDate
                    if (dateRangeBegin < startDate)
                    {
                        dateRangeBegin = startDate;
                    }

                    dateRangeEnd = dateToCheck.AddDays(7 - (int)dateToCheck.DayOfWeek);

                    if ((dateRangeBegin.Date < dateToCheck) && (dateRangeBegin.Date.Month != dateToCheck.Month))
                    {
                        dateRangeBegin = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day);
                    }

                    if ((dateRangeEnd.Date > dateToCheck) && (dateRangeEnd.Date.Month != dateToCheck.Month))
                    {
                        DateTime dtTo = new DateTime(dateToCheck.Year, dateToCheck.Month, 1);
                        dtTo = dtTo.AddMonths(1);
                        dateRangeEnd = dtTo.AddDays(-(dtTo.Day));
                    }
                    if (dateRangeEnd.Date > endDate)
                    {
                        dateRangeEnd = new DateTime(dateRangeEnd.Year, dateRangeEnd.Month, endDate.Day);
                    }
                    weekRange = new WeekRange
                    {
                        StartDate = dateRangeBegin,
                        EndDate = dateRangeEnd,
                        Range = dateRangeBegin.Date.ToShortDateString() + '-' + dateRangeEnd.Date.ToShortDateString(),
                        Month = dateToCheck.Month,
                        Year = dateToCheck.Year,
                        Week = week++
                    };

                    weekRangeList.Add(weekRange);
                    dateToCheck = dateRangeEnd.AddDays(1);
                }
                startDateToCheck = startDateToCheck.AddMonths(1);
            }

            return weekRangeList;
        }


        public static int GetWeekOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }
        public static DateTime dtStartOR(DateTime date)
        {
            var dateBefore = date.AddDays(-1);
            return new DateTime(dateBefore.Year, dateBefore.Month, dateBefore.Day, 17,0,0);            
        }
        public static DateTime dtEndOR(DateTime date)
        {
            return new DateTime(date.Year, date.Month,date.Day,17, 0, 0);
        }

        public static Boolean RunValidateSchuduler()
        {
            DateTime date = DateTime.Now;
            var dtStart=new DateTime(date.Year, date.Month, date.Day, 17, 0, 0);
            var dtEnd = new DateTime(date.Year, date.Month, date.Day, 18, 0, 0);
            return ((date >= dtStart) && (date <= dtEnd));
        }
        public static int GetAges(this DateTime bod)
        {
            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - bod.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (bod.Date > today.AddYears(-age)) age--;
            return age;
        }
        public class WeekRange
        {
            public string Range { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int Week { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
        }
        #region Extension for string
        public static string ToFullName(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            string strFullName = str;
            int startIndex = strFullName.IndexOf('(');
            if (startIndex > 0)
            {
                strFullName = strFullName.Remove(startIndex, strFullName.Length - startIndex);
            }
            return strFullName?.Trim();
        }
        #endregion
    }

}
