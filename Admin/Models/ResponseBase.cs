using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class ResponseBase
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public object data { get; set; }
    }
}