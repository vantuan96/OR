using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum RoomTypeEnum
    {
        [Description("Phòng Mổ")]
        Surgery = 1,
        [Description("Phòng Sanh")]
        Birth = 2,
        [Description("Phòng Nội Soi")]
        Endoscopy = 3,
        [Description("Phòng mổ cấp cứu")]
        Emergency = 5,
        [Description("Phòng ghi nhận doanh thu")]
        Approve4Pay = 6,
        [Description("Khác")]
        Other =4
    }
}
