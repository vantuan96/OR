using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.QueuePatient
{
    public enum SexEnum
    {
        [Description("Nam")]
        Nam = 1,
        [Description("Nữ")]
        Nữ = 2,
        [Description("Chưa xác định")]
        ChuaXacDinh = 0      
    }
}
