using System.ComponentModel;

namespace Contract.QueuePatient
{
    public enum PatientStateEnum
    {
        [Description("Mới tạo (Create)")]
        Moitao =1,
        [Description("Chuẩn bị xuống PM (On the Way)")]
        OnTheWay = 2,
        [Description("Phòng tiếp đón (Holding bay)")]
        Holding = 3,
        [Description("Trong phòng mổ (Inside OR)")]
        InOR = 4,
        [Description("Trong phòng hồi tỉnh (in PACU)")]
        OutOR = 5,
        [Description("Hoãn mổ (Cancel)")]
        Cancel = 6,
        [Description("Cấp cứu (Emergency)")]
        Emergency = 7,
        [Description("Ca mổ thêm (Arising)")]
        Other = 8,
        [Description("Lịch mổ phiên (Elective surgical schedule)")]
        LichMoPhien = 9,
        [Description("Lùi giờ (Delay)")]
        LuiGio = 10,
        [Description("Về khoa hồi sức (Back to ICU)")]
        Backhoisuc = 11,
        [Description("Về khoa nội trú (Back to Inpatient ward)")]
        Backnoitru = 12
    }

    public enum PatientStatePublicEnum
    {
        [Description("Chuẩn bị xuống PM (On the Way)")]
         OnTheWay = 2,
        [Description("Phòng tiếp đón (Holding bay)")]
        Holding = 3,
        [Description("Trong phòng mổ (Inside OR)")]
        InOR = 4,
        [Description("Trong phòng hồi tỉnh (in PACU)")]
        OutOR = 5,
        [Description("Hoãn mổ (Cancel)")]
        Cancel = 6,
        [Description("Cấp cứu (Emergency)")]
        Emergency = 7,
        [Description("Ca mổ thêm (Arising)")]
        Other = 8,
        [Description("Lịch mổ phiên (Elective surgical schedule)")]
        LichMoPhien = 9,
        [Description("Lùi giờ (Delay)")]
        LuiGio = 10,
        [Description("Về khoa hồi sức (Back to ICU)")]
        Backhoisuc = 11,
        [Description("Về khoa nội trú (Back to Inpatient ward)")]
        Backnoitru = 12
    }
}
