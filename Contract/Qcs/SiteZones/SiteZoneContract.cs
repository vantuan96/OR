using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BMS.Contract.Qcs.SiteZones
{
    [ProtoContract]
    public class SiteZoneContract
    {
        [ProtoMember(1)]
        public int SiteZoneId { get; set; }

        [ProtoMember(2)]
        public string SiteZoneName { get; set; }

        [ProtoMember(3)]
        public string SiteZoneCode { get; set; }

        [ProtoMember(4)]
        public List<int> EvaluationCateGroupId { get; set; }

        [ProtoMember(5)]
        public int SiteId { get; set; }
        
    }   
}
