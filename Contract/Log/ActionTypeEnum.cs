using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Log
{
    public enum ActionTypeEnum
    {
        [Description("Tạo mới")]
        Created =1,
        [Description("Chỉnh sửa")]
        Edit = 2,
        [Description("Xóa")]
        Delete = 3,
        [Description("Khác")]
        Orther = 4,
        [Description("Cập nhật kết quả hạng mục")]
        FinishItems = 5,
        [Description("Thay đổi trạng thái checklist")]
        ChangeStateCheckList = 6
    }
}
