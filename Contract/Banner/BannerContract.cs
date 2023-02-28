using ProtoBuf;
using System;
using System.Collections.Generic;

namespace BMS.Contract.Banner
{
    [ProtoContract]
    public class BannerContract
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public string TargetUrl { get; set; }

        [ProtoMember(3)]
        public DateTime StartDate { get; set; }

        [ProtoMember(4)]
        public DateTime EndDate { get; set; }

        [ProtoMember(5)]
        public string BannerUrl { get; set; }

        [ProtoMember(6)]
        public string BannerUrlMobile { get; set; }

        [ProtoMember(7)]
        public int ImageWitdh { get; set; }

        [ProtoMember(8)]
        public int ImageHeight { get; set; }

        [ProtoMember(9)]
        public int PositionId { get; set; }

        [ProtoMember(10)]
        public int BannerGroupId { get; set; }

        [ProtoMember(11)]
        public string Description { get; set; }

        [ProtoMember(12)]
        public double ImageWidthNatural { get; set; }

        [ProtoMember(13)]
        public double ImageHeightNatural { get; set; }

        [ProtoMember(14)]
        public int MsId { get; set; }

        [ProtoMember(15)]
        public int CreatedBy { get; set; }

        [ProtoMember(16)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(17)]
        public int Status { get; set; }

        [ProtoMember(18)]
        public int Id { get; set; }

        [ProtoMember(19)]
        public bool IsOnsite { get; set; }

        [ProtoMember(20)]
        public List<BannerContentContract> BannerContents { get; set; }

        [ProtoMember(21)]
        public int Sort { get; set; }

        [ProtoMember(22)]
        public List<int> PositionIds { get; set; }
    }
}