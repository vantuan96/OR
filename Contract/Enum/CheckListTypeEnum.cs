using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum CheckListTypeEnum
    {
        [Description("Hàng ngày")]
        Daily =1,
        [Description("Mỗi tuần")]
        Weekly =2,
        [Description("Mỗi tháng")]
        Monthly =3,
        [Description("Một lần")]
        OnlyOne = 4

    }
}
