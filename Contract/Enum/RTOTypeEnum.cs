using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum RTOTypeEnum
    {
        [Description("2 Hours")]
        Hour2 =1,
        [Description("4 Hours")]
        Hour4 =2,
        [Description("8 Hours")]
        Hour8 =3,
        [Description("24 Hours")]
        Hour24 =4

    }
}
