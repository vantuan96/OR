using System.ComponentModel;

namespace Contract.Core
{
    public enum SystemLogType
    {
        [Description("Tất cả")]
        All = 0,

        [Description("Lỗi runtime")]
        Error = 1,

        [Description("Lỗi thực thi Sql")]
        SqlError = 2,

        [Description("Thông báo")]
        Info = 3,

        [Description("Update Db version")]
        DeployInfo = 4,
        [Description("Đồng bộ dử liệu")]
        SyncData = 5
    }
}
