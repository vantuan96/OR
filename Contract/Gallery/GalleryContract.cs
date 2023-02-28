using ProtoBuf;
using System;
using System.Collections.Generic;
using BMS.Contract.Image;

namespace BMS.Contract.Gallery
{
    [ProtoContract]
    public class GalleryContract
    {
        [ProtoMember(1)]
        public int GalleryId { get; set; }

        [ProtoMember(2)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(3)]
        public bool IsOnsite { get; set; }

        [ProtoMember(4)]
        public int MsId { get; set; }

        [ProtoMember(5)]
        public int CreatedBy { get; set; }

        [ProtoMember(6)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(7)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(8)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(9)]
        public string Name { get; set; }

        [ProtoMember(10)]
        public List<ImageContract> Images { get; set; }

        [ProtoMember(11)]
        public bool IsPredefined { get; set; }

        [ProtoMember(12)]
        public string Key { get; set; }
    }
}