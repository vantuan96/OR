using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Qcs.ZoneViolations
{
    public  enum ViolationStatusEnum
    {
        [Description("Mới tạo")]
        Create =1,
        [Description("Đang xử lý")]
        InProcessing =2,
        [Description("Tạm dừng")]
        TempStop =3,
        [Description("Hoàn thành")]
        Finish =4,
        [Description("Xin duyệt hạn")]
        ToApproveDate =5,
        [Description("Phê duyệt hạn")]
        ApprovedDate =6
    }
}
