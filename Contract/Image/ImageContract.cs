using ProtoBuf;
using System;

namespace BMS.Contract.Image
{
    [ProtoContract]
    public class ImageContract
    {
        [ProtoMember(1)]
        public int ImageId { get; set; }

        [ProtoMember(2)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(3)]
        public bool IsOnsite { get; set; }

        [ProtoMember(4)]
        public string TargetUrl { get; set; }

        [ProtoMember(5)]
        public string ImageUrl { get; set; }

        [ProtoMember(6)]
        public int CreatedBy { get; set; }

        [ProtoMember(7)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(8)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(9)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(10)]
        public int MediaType { get; set; }

        [ProtoMember(11)]
        public string VideoUrl { get; set; }

        [ProtoMember(12)]
        public int Sort { get; set; }
    }
}