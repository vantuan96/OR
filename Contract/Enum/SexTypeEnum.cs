using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum SexTypeEnum
    {
        [Description("Nam")]
        Nam = 1,
        [Description("Nữ")]
        Nu = 2,
        [Description("Không xác định")]
        Undefined = 3
    }
}
