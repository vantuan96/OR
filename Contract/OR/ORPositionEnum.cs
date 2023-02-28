using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public enum ORPositionEnum
    {
        [Description("PTV chính")]
        PTVMain = 1,
        [Description("PTV phụ 1")]
        PTVSub1 = 2,
        [Description("PTV phụ 2")]
        PTVSub2 = 3,
        [Description("PTV phụ 3")]
        PTVSub3 = 4,
        [Description("PTV phụ 4")]
        PTVSub4 = 44,
        [Description("PTV phụ 5")]
        PTVSub5 = 45,
        [Description("PTV phụ 6")]
        PTVSub6 = 46,
        [Description("PTV phụ 7")]
        PTVSub7 = 47,
        [Description("PTV phụ 8")]
        PTVSub8 = 48,
        [Description("Bác sĩ CEC")]
        CECDoctor = 5,
        [Description("Điều dưỡng dụng cụ 1")]
        NurseTool1 = 6,
        [Description("Điều dưỡng dụng cụ 2")]
        NurseTool2 = 7,
        [Description("Điều dưỡng chạy ngoài 1")]
        NurseOutRun1 = 8,
        [Description("Điều dưỡng chạy ngoài 2")]
        NurseOutRun2 = 9,
        [Description("Điều dưỡng chạy ngoài 3")]
        NurseOutRun3 = 16,
        [Description("Điều dưỡng chạy ngoài 4")]
        NurseOutRun4 = 17,
        [Description("Điều dưỡng chạy ngoài 5")]
        NurseOutRun5 = 18,
        [Description("Điều dưỡng chạy ngoài 6")]
        NurseOutRun6 = 19,

        [Description("Bác sĩ gây mê")]
        MainAnesthDoctor = 10,
        [Description("Bác sĩ phụ mê 1")]
        SubAnesthDoctor = 11,
        [Description("Điều dưỡng phụ mê 1")]
        AnesthNurse1 = 12,
        [Description("Điều dưỡng phụ mê 2")]
        AnesthNurse2 =13,
        [Description("Điều dưỡng hồi tỉnh")]
        AnesthNurseRecovery = 14,

        //vutv7 bổ sung thông tin màn điều phối mổ/mê
        [Description("KTV phụ mổ")]
        KTVSubSurgery = 50,
        [Description("KTV CĐHA")]
        KTVDiagnose = 51,
        [Description("KTV chạy máy CEC")]
        KTVCEC = 52,
        [Description("BS CĐHA")]
        DoctorDiagnose = 53,
        [Description("BS sơ sinh")]
        DoctorNewBorn = 54,
        [Description("Nữ hộ sinh")]
        Midwives = 55,
        [Description("BS khám gây mê")]
        Anesthesiologist = 56,
        [Description("Bác sĩ phụ mê 2")]
        SubAnesthDoctor2 = 57,
    }
}
