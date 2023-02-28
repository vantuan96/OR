using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BMS.Contract.Qcs.Report
{
    [ProtoContract]
    public class CapBySiteStatisticsReportContract
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int TotalError { get; set; }
        public int TotalNotReport { get; set; }
        public int TotalReported { get; set; }
        public int TotalOverDate { get; set; }
        public int TotalFinish { get; set; }
    }
}
