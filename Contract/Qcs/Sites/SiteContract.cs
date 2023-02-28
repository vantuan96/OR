using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BMS.Contract.Qcs.Sites
{
    [ProtoContract]
    public class SiteContract
    {
        [ProtoMember(1)]
        public int SiteId { get; set; }
        [ProtoMember(2)]
        public string SiteName { get; set; }
        [ProtoMember(3)]
        public string SiteCode { get; set; }
        [ProtoMember(4)]
        public string SiteAddress { get; set; }
        [ProtoMember(5)]
        public System.DateTime CreatedDate { get; set; }
        [ProtoMember(6)]
        public int CreatedBy { get; set; }
        [ProtoMember(7)]
        public System.DateTime LastUpdatedDate { get; set; }
        [ProtoMember(8)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(9)]
        public bool IsDeleted { get; set; }
    }
}
