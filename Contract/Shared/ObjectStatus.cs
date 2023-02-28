using System.ComponentModel;

namespace Contract.Shared
{
    public enum ObjectStatus
    {
        [Description("ObjectStatus_New")]
        New = 1,

        [Description("ObjectStatus_Onsite")]
        Onsite = 2,

        [Description("ObjectStatus_WaitingOnsite")]
        WaitingOnsite = 3,

        [Description("ObjectStatus_Deactive")]
        Deactive = 4,

        //[Description("ObjectStatus_Deleted")]
        //Deleted = 5
    }

    public enum OnsiteStatus
    {
        [Description("OnsiteStatus_False")]
        False = 0,

        [Description("OnsiteStatus_True")]
        True = 1,
    }

    public enum ApprovalStatus
    {
        [Description("ApprovalStatus_New")]
        New = 1,

        [Description("ApprovalStatus_Approved")]
        Approved = 2,

        [Description("ApprovalStatus_Reject")]
        Reject = 3,
    }
}