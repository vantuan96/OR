using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum StabilityEnum
    {
        [Description("Cao")]
        High =1,
        [Description("Trung bình ")]
        Medium =2,
        [Description("Thấp")]
        Low =3
    }
}
