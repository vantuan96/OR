using ProtoBuf;
using System;

namespace BMS.Contract.FE
{
    [ProtoContract]
    public class BannerContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(4)]
        public int PositionId { get; set; }

        [ProtoMember(3)]
        public int BannerGroupId { get; set; }

        [ProtoMember(5)]
        public int ImageWitdh { get; set; }

        [ProtoMember(6)]
        public int ImageHeight { get; set; }

        [ProtoMember(7)]
        public DateTime StartDate { get; set; }

        [ProtoMember(8)]
        public DateTime EndDate { get; set; }

        [ProtoMember(9)]
        public int Status { get; set; }

        [ProtoMember(10)]
        public bool IsOnsite { get; set; }

        [ProtoMember(11)]
        public double ImageWidthNatural { get; set; }

        [ProtoMember(12)]
        public double ImageHeightNatural { get; set; }

        [ProtoMember(13)]
        public int MsId { get; set; }

        [ProtoMember(14)]
        public int CreatedBy { get; set; }

        [ProtoMember(15)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(16)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(17)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(18)]
        public string BannerGroupName { get; set; }

        [ProtoMember(19)]
        public string BannerPositionName { get; set; }

        [ProtoMember(20)]
        public int Sort { get; set; }

        [ProtoMember(21)]
        public BannerContentContract BannerContent { get; set; }
    }


    [ProtoContract]
    public class BannerContentContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string TargetUrl { get; set; }

        [ProtoMember(3)]
        public string BannerUrl { get; set; }

        [ProtoMember(4)]
        public string BannerUrlMobile { get; set; }

        [ProtoMember(5)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(6)]
        public int CreatedBy { get; set; }

        [ProtoMember(7)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(8)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(9)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(10)]
        public string Name { get; set; }

        [ProtoMember(11)]
        public string Description { get; set; }

        [ProtoMember(12)]
        public string LangShortName { get; set; }

    }
}