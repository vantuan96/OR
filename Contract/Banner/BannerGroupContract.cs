using ProtoBuf;
using System;

namespace BMS.Contract.Banner
{
    [ProtoContract]
    public class BannerGroupContract
    {
        [ProtoMember(1)]
        public int BannerGroupId { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; }

        [ProtoMember(4)]
        public int CreatedBy { get; set; }

        [ProtoMember(5)]
        public string CreatedByName { get; set; }

        [ProtoMember(6)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(7)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(8)]
        public string LastUpdatedByName { get; set; }

        [ProtoMember(9)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(10)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(11)]
        public string CreatedByEmail { get; set; }

        [ProtoMember(12)]
        public string LastUpdatedByEmail { get; set; }

        [ProtoMember(13)]
        public int MsId { get; set; }
    }
}