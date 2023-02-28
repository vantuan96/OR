using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum CheckListStateEnum
    {
        [Description("Chờ thực hiện")]
        PrepareExecute =1,
        [Description("Chờ phê duyệt")]
        Execute=2,
        [Description("Đã duyệt")]
        ApproveOk=3,
        [Description("Không duyệt")]
        Reject =4
    }
}
