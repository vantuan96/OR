using ProtoBuf;
using System;

namespace BMS.Contract.Banner
{
    [ProtoContract]
    public class BannerContentContract
    {
        [ProtoMember(1)]
        public int ContentId { get; set; }

        [ProtoMember(22)]
        public int BannerId { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; }

        [ProtoMember(4)]
        public string TargetUrl { get; set; }

        [ProtoMember(5)]
        public string BannerUrl { get; set; }

        [ProtoMember(6)]
        public string BannerUrlMobile { get; set; }

        [ProtoMember(7)]
        public string Description { get; set; }

        [ProtoMember(8)]
        public string LangShortName { get; set; }

        [ProtoMember(9)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(10)]
        public int CreatedBy { get; set; }

        [ProtoMember(11)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(122)]
        public int MsId { get; set; }
    }
}