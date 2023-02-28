using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.AdminAction
{
    [ProtoContract]
    public class AdminGroupActionMapContract
    {
        [ProtoMember(1)]
        public int GaId { get; set; }

        [ProtoMember(2)]
        public int AId { get; set; }

        [ProtoMember(3)]
        public int CreatedBy { get; set; }

        [ProtoMember(4)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(5)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(6)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(7)]
        public bool IsDeleted { get; set; }
    }
}
