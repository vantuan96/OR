using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using BMS.Contract.Qcs.EvaluationCategoryGroups;

namespace BMS.Contract.Qcs.SiteZones
{
    [ProtoContract]
    public class SiteZoneInfoContract
    {
        [ProtoMember(1)]
        public int SiteId { get; set; }
        [ProtoMember(2)]
        public string SiteName { get; set; }
        [ProtoMember(3)]
        public string SiteCode { get; set; }
        [ProtoMember(4)]
        public int SiteZoneId { get; set; }
        [ProtoMember(5)]
        public string SiteZoneName { get; set; }
        [ProtoMember(6)]
        public string SiteZoneCode { get; set; }

        [ProtoMember(7)]
        public List<EvaluationCateGroupItemContract> CateGroups { get; set; }
    }
}
