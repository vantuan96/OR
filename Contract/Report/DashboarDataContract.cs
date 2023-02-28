using System.Collections.Generic;

namespace Contract.Report
{
    public class SystemDashBoard
    {
        public int SystemId { get; set; }
        public string SystemName { get; set; }
        public int TotalCheckList { get; set; } 
        public int TotalFinish { get; set; }
        public int TotalInprocess { get; set; }
        public int OverDeadline { get; set; }
    }
    public class CheckListDashboard
    {
         public int CheckListType { get; set; }
         public string CheckListTypeName { get; set; }
         public int TotalCheckList { get; set; }
         public int TotalFinish { get; set; }
         public int TotalInprocess { get; set; }
         public int OverDeadline { get; set; }
    }
    public class CheckListInfoDashboard
    {

        public int ReportCheckListTypeId { get; set; }
        public string ReportCheckListTypeName { get; set; }
        public int TotalCheckList { get; set; }
        public int TotalFinishCheckList { get; set; }
    }


    
}
