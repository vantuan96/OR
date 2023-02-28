using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Qcs.EvaluationSiteZones
{
    public enum EvaluationCalendarStatusEnum
    {
        [Description("Đang xử lý")]
        EvaluationCalendarStatusEnum_IsProcessing = 1,
        [Description("Đã xử lý xong")]
        EvaluationCalendarStatusEnum_Closed = 2
    }
}
