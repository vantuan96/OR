using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Qcs.Share
{
    public enum SourceClientEnum
    {
        [Description("Chủ đề")]
        Subject =1,
        [Description("Định kỳ")]
        Calendar =2
    }
}
