using ProtoBuf;
using System;
using System.Collections.Generic;

namespace BMS.Contract.Banner
{
    [ProtoContract]
    public class BannerListItemContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public int PositionId { get; set; }

        [ProtoMember(3)]
        public int BannerGroupId { get; set; }

        [ProtoMember(4)]
        public string Name { get; set; }

        [ProtoMember(5)]
        public string TargetUrl { get; set; }

        [ProtoMember(6)]
        public string BannerUrl { get; set; }

        [ProtoMember(7)]
        public string BannerUrlMobile { get; set; }

        [ProtoMember(8)]
        public int ImageWitdh { get; set; }

        [ProtoMember(9)]
        public int ImageHeight { get; set; }

        [ProtoMember(10)]
        public DateTime StartDate { get; set; }

        [ProtoMember(11)]
        public DateTime EndDate { get; set; }

        [ProtoMember(12)]
        public int Status { get; set; }

        [ProtoMember(13)]
        public bool IsOnsite { get; set; }

        [ProtoMember(14)]
        public double ImageWidthNatural { get; set; }

        [ProtoMember(15)]
        public double ImageHeightNatural { get; set; }

        [ProtoMember(16)]
        public int MsId { get; set; }

        [ProtoMember(17)]
        public int CreatedBy { get; set; }

        [ProtoMember(18)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(19)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(20)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(21)]
        public string BannerGroupName { get; set; }

        [ProtoMember(22)]
        public string BannerPositionName { get; set; }

        [ProtoMember(23)]
        public int Sort { get; set; }

        [ProtoMember(24)]
        public List<int> PositionIds { get; set; }
    }
}