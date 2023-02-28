using System.ComponentModel;

namespace BMS.Contract.Qcs.Share
{
    public enum ViolationRecordStatusEnum
    {
        [Description("Mới")]
        ViolationRecordStatus_New = 1,
        [Description("KSCL thẩm định")]
        ViolationRecordStatus_KSCLThamdinh = 2,
        [Description("NS nhận")]
        ViolationRecordStatus_NSnhan = 3
    }
}
