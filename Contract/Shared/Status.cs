using System.ComponentModel;

namespace Contract.Shared
{
    public enum Status
    {
        [Description("Đang mở")]
        Active = 1,

        [Description("Đang khóa")]
        Deactive = 2
    }

    public enum OnlineStatus
    {
        [Description("Online")]
        Online = 1,

        [Description("Offline")]
        Offline = 2
    }

    public enum ReasonType
    {
        [Description("Checkbox")]
        Checkbox = 1,

        [Description("Ghi chú")]
        Textbox = 2
    }
}