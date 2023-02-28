using System.ComponentModel;

namespace Contract.Core
{
    public enum SourceType
    {
        [Description("Tất cả")]
        All = 0,

        [Description("Api")]
        ApiTool = 1,

        [Description("Web")]
        Web = 2,

        [Description("Database")]
        Database = 3
       
    }
}
