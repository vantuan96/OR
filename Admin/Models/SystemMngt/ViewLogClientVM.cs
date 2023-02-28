using System.Collections.Generic;
using Contract.SystemLog;

namespace Admin.Models.SystemMngt
{
    public class ViewLogClientVM
    {
        public List<string> LogFiles { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public int TotalCount { get; set; }

        public string FileName { get; set; }

        public FileLogLocation LogLocation { get; set; }

        public string ContentNodes { get; set; }
    }
}