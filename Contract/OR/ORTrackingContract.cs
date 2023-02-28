using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    [ProtoContract]
    public class ORTrackingContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public int UserId { get; set; }

        [ProtoMember(3)]
        public string FullName { get; set; }

        [ProtoMember(4)]
        public string Email { get; set; }

        [ProtoMember(5)]
        public string ContentTracking { get; set; }

        [ProtoMember(6)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(7)]
        public int State { get; set; }
        [ProtoMember(8)]
        public string StateName { get; set; }
        [ProtoMember(8)]
        public int ORId { get; set; }
        
    }
}
