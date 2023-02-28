using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum AuthenticationMethodEnum
    {
        [Description("ActiveDirectory")]
        ActiveDirectory =1,
        [Description("DBCredentials")]
        DBCredentials =2
    }
}
