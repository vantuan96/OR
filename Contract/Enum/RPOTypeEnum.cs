using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum RPOTypeEnum
    {
        [Description("LastTransaction")]
        LastTransaction =1,
        [Description("LastBackup")]
        LastBackup =2
    }
}
