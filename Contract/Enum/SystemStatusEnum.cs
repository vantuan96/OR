using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum SystemStatusEnum
    {
        [Description("Kích hoạt")]
        Active = 1,
        [Description("Dev")]
        Dev = 2,
        [Description("Chưa kích hoạt")]
        Retired = 3
    }
}
