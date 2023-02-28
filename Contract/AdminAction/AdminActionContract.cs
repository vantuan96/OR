using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.AdminAction
{
    [ProtoContract]
    public class AdminActionFullContract
    {
        [ProtoMember(1)]
        public int AId { get; set; }
        
        [ProtoMember(2)]
        public string ActionName { get; set; }

        [ProtoMember(3)]
        public string ActionDisplayName { get; set; }

        [ProtoMember(4)]
        public string CssIcon { get; set; }

        [ProtoMember(5)]
        public bool ShowMenuStatus { get; set; }

        [ProtoMember(6)]
        public bool PublicStatus { get; set; }

        [ProtoMember(7)]
        public int Sort { get; set; }

        [ProtoMember(8)]
        public bool Visible { get; set; }

        [ProtoMember(9)]
        public bool IsDefault { get; set; }

        [ProtoMember(10)]
        public int CreatedBy { get; set; }

        [ProtoMember(11)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(12)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(13)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(14)]
        public bool IsDeleted { get; set; }
    }
}
