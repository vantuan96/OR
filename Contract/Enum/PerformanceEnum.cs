using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum PerformanceEnum
    {
        [Description("Ổn định")]
        YesStability = 1,
        [Description("Không ổn định")]
        NoStability = 2
    }
}
