using System.Data;

namespace Admin.Models.Report
{
    public class WeekInfoModel
    {
        public string Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public WeekInfoModel(DataRow row)
        {
            Week = row["WeekName"].ToString();
            Month = int.Parse(row["MonthID"].ToString());
            Year = int.Parse(row["YearID"].ToString());
        }
    }
}