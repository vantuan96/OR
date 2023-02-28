using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum DREnum
    {
        [Description("Có")]
        Yes =1,
        [Description("Không")]
        No =2,
        [Description("Cloud")]
        Cloud =3
    }
}
