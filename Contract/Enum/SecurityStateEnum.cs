using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum SecurityStateEnum
    {
        [Description("Red")]
        Red =1,
        [Description("Green")]
        Green =2,
        [Description("Amber")]
        Amber =3
    }
}
