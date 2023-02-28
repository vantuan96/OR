using System.ComponentModel;

namespace Contract.User
{
    public enum AdminRole
    {
        [Description("Super Admin")]
        SuperAdmin = 1,

        [Description("Admin")]
        Admin = 2,
        [Description("Surgery View Calendar")]
        ViewSurgeryCalendar = 4,
        [Description("Surgery Create")]
        CreateSurgery = 5,
        [Description("Surgery Calendar Management (OH)")]
        ManagSurgery = 6,
        [Description("Anesthesia Management (OH)")]
        ManagAnes = 7,
        [Description("Surgery Management(OH)")]
        ManagAdminSurgery = 8,
        [Description("Surgery View Calendar (OH)")]
        ViewSurgeryCalendarOH = 9,
        [Description("Anesthesia Process Management (OH)")]
        ManagAnesProcessStep = 10,
        [Description("Search Patient (OH)")]
        SearchPatient = 11,
    }
}
