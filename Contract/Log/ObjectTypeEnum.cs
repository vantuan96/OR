using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Log
{
    public enum ObjectTypeEnum
    {
        [Description("Checklist")]
        CheckList =1,
        [Description("Hạng mục")]
        Item = 2,
        [Description("Khác")]
        Other = 3,
    }
}
