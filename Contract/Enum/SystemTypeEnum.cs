using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum SystemTypeEnum
    {
        [Description("SAP")]
        SAP =1,
        [Description("Non SAP")]
        NonSAP =2
    }
}
