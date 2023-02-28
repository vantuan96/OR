using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{

    public enum SystemStateEnum
    {
        [Description("Kích hoạt")]
        Active =1,
        [Description("Không")]
        NoActive =2
    }

    public enum SearchStateEnum
    {
        [Description("Kích hoạt")]
        Active = 0,
        [Description("Không")]
        NoActive = 1
    }
    public enum DisPlayEnum
    {
        [Description("Hiển thị")]
        Show = 1,
        [Description("Ẩn")]
        Hide = 0
    }
}
