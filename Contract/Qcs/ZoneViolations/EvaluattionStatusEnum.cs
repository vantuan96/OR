using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Qcs.ZoneViolations
{
    public enum EvaluattionStatusEnum
    {
        [Description("Chưa đánh giá")]
        ChuaDanhGia =1,
        [Description("Không đánh giá")]
        KhongDanhGia =2,
        [Description("Không đạt")]
        KhongDat =3,
        [Description("Đạt")]
        Dat =4
    }
}
