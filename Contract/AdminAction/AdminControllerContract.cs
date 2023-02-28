using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.AdminAction
{
    [ProtoContract]
    public class AdminControllerContract
    {
        [ProtoMember(1)]
        public int CId { get; set; }

        [ProtoMember(2)]
        public string ControllerName { get; set; }

        [ProtoMember(3)]
        public string ControllerDisplayName { get; set; }

        [ProtoMember(4)]
        public string CssIcon { get; set; }

        [ProtoMember(5)]
        public int Sort { get; set; }

        [ProtoMember(6)]
        public bool Visible { get; set; }

        [ProtoMember(7)]
        public int CreatedBy { get; set; }

        [ProtoMember(8)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(9)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(10)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(11)]
        public bool IsDeleted { get; set; }

        [ProtoMember(12)]
        public List<AdminActionFullContract> ListActions { get; set; }
    }
}
