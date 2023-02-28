using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Const
{
    public enum ErrorCode
    {
        Unknow = 1,
        LoginFail =2,
        AccessTokenInvalid = 10,

        Answer_SaveSuccess = 1100,
        Answer_InvalidLayoutType = 1101
    }
}
