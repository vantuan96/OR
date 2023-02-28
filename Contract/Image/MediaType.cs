using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Image
{
    public enum MediaType
    {
        [Description("MediaType_Image")]
        Image = 1,

        [Description("MediaType_Video")]
        Video = 2,

        [Description("MediaType_Document")]
        Document = 3
    }
}
