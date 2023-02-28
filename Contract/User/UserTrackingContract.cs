using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.User
{
    [ProtoContract]
    public class UserTrackingContract
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
    }
}
