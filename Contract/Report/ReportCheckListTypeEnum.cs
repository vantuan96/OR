using System.ComponentModel;

namespace Contract.Report
{
    public enum ReportCheckListTypeEnum
    {
        [Description("Hôm nay")]
        Today=1,
        [Description("Tuần này")]
        CurrentWeek=2,
        [Description("Tháng này")]
        CurrentMonth = 3
    }
}
