using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum OrginTypeEnum
    {
        [Description("Nội bộ")]
        InHouse =1,
        [Description("Thuê ngoài")]
        Vendor =2
    }
}
