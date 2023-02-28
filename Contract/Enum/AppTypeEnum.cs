using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Enum
{
    public enum AppTypeEnum
    {
        [Description("Win App")]
        WinApp =1,
        [Description("Web App")]
        WebApp =2,
        [Description("Thin App")]
        ThinApp =3,
        [Description("Mobile App")]
        MobileApp =4,
        [Description("SAS")]
        SAS =5,
        [Description("Other")]
        Other =6
       
    }
}
