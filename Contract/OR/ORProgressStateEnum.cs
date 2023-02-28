using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public enum ORStepEnum
    {
        [Description("Đăng ký mổ")]
        Registor = 1,
        [Description("Điều phối mổ")]
        CoordinatorSurgery = 2,
        [Description("Điều phối ngây mê")]
        CoordinatorAnes = 3,
        [Description("Tiến trình mổ")]
        ProcessSurgery = 4
    }
    public enum ORProgressStateEnum
    {
        [Description("Đăng ký mổ")]
        Registor = 1,
        [Description("Phân công ekip")]
        AssignEkip = 33,
        [Description("Điều phối mổ không duyệt")]
        NoApproveSurgeryManager = 2,
        [Description("Điều phối mổ approve")]
        ApproveSurgeryManager = 3,
        [Description("Điều phối gây mê không duyệt")]
        CancelAnesthManager = 4,
        [Description("Điều phối gây mê approve")]
        ApproveAnesthManager = 5
    }

    public enum OHPatientStateEnum
    {
        [Description("Đã duyệt Mổ (Approved Surgery)")]
        //[Description("Vừa duyệt đăng ký xong (Create)")]
        DuyetMo = 3,
        [Description("Đã phân ekip (Assigned team member)")]
        AssignEkip = 33,
        [Description("Đã duyệt gây mê (Approved Anes)")]
        //[Description("Vừa duyệt đăng ký xong (Create)")]
        DuyetGayMe = 5,
        [Description("Chuẩn bị xuống PM (On the Way)")]
        OnTheWay = 6,
        [Description("Phòng tiếp đón (Holding bay)")]
        Holding = 7,
        [Description("Trong phòng mổ (Inside OR)")]
        InOR = 8,
        [Description("Trong phòng hồi tỉnh (in PACU)")]
        OutOR = 9,
        [Description("Hoãn mổ (Cancel)")]
        Cancel = 10,
        [Description("Cấp cứu (Emergency)")]
        Emergency = 11,
        [Description("Ca mổ thêm (Arising)")]
        Other = 12,
        [Description("Lịch mổ phiên (Elective surgical schedule)")]
        LichMoPhien = 13,
        [Description("Lùi giờ (Delay)")]
        LuiGio = 14,
        [Description("Về khoa hồi sức (Back to ICU)")]
        Backhoisuc = 15,
        [Description("Về khoa nội trú (Back to Inpatient ward)")]
        Backnoitru = 16,
        [Description("Hủy chỉ định")]
        CancelCharge = 110,
    }

    public enum OHPatientStatePublicEnum
    {
        [Description("Chuẩn bị xuống PM (On the Way)")]
        OnTheWay = 6,
        [Description("Phòng tiếp đón (Holding bay)")]
        Holding = 7,
        [Description("Trong phòng mổ (Inside OR)")]
        InOR = 8,
        [Description("Trong phòng hồi tỉnh (in PACU)")]
        OutOR = 9,
        [Description("Hoãn mổ (Cancel)")]
        Cancel = 10,
        [Description("Cấp cứu (Emergency)")]
        Emergency = 11,
        [Description("Ca mổ thêm (Arising)")]
        Other =12,
        [Description("Lịch mổ phiên (Elective surgical schedule)")]
        LichMoPhien = 13,
        [Description("Lùi giờ (Delay)")]
        LuiGio = 14,
        [Description("Về khoa hồi sức (Back to ICU)")]
        Backhoisuc = 15,
        [Description("Về khoa nội trú (Back to Inpatient ward)")]
        Backnoitru = 16
    }

    public enum ORLogStateEnum
    {
        [Description("Đăng ký mổ")]
        Registor = 1,
        [Description("Phân công ekip")]
        AssignEkip = 33,
        [Description("Điều phối mổ không duyệt")]
        NoApproveSurgeryManager = 2,
        [Description("Điều phối mổ approve")]
        ApproveSurgeryManager = 3,
        [Description("Điều phối gây mê không duyệt")]
        CancelAnesthManager = 4,
        [Description("Điều phối gây mê approve")]
        Moitao = 5,
        [Description("Chuẩn bị xuống PM (On the Way)")]
        OnTheWay = 6,
        [Description("Phòng tiếp đón (Holding bay)")]
        Holding = 7,
        [Description("Trong phòng mổ (Inside OR)")]
        InOR = 8,
        [Description("Trong phòng hồi tỉnh (in PACU)")]
        OutOR = 9,
        [Description("Hoãn mổ (Cancel)")]
        Cancel = 10,
        [Description("Cấp cứu (Emergency)")]
        Emergency = 11,
        [Description("Ca mổ thêm (Arising)")]
        Other = 12,
        [Description("Lịch mổ phiên (Elective surgical schedule)")]
        LichMoPhien = 13,
        [Description("Lùi giờ (Delay)")]
        LuiGio = 14,
        [Description("Về khoa hồi sức (Back to ICU)")]
        Backhoisuc = 15,
        [Description("Về khoa nội trú (Back to Inpatient ward)")]
        Backnoitru = 16,
        [Description("Hủy chỉ định")]
        CancelCharge = 110,
    }
}
