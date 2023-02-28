using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.Shared
{
    public enum EnumErrorCode
    {
        IsConstructing = 0,
        Denied = 1001,
        CouldNotLoadDataFromAuthen = 2000,
        DeniedSystem = 2001,
        CouldNotLoadDataFromDealService = 6001,
        CouldNotLoadDataFromAddressService = 6002,
        Exception = 6003,
        ValidateAntiForgeryTokenFailed = 6004,
        DefaultPageNotFounded = 7001,
        MicrositeNotFounded = 7002,
    }
}