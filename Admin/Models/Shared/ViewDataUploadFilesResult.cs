using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.Shared
{
    public class ViewDataUploadFilesResult
    {
        public string Thumbnail_url { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
    }
}