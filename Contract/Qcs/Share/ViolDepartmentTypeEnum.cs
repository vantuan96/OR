using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Qcs.Share
{
    public enum ViolDepartmentTypeEnum
    {
        [Description("Phòng/Ban/Khối")]
        Department = 0,
        [Description("BQL")]
        Management = 1
    }
}
