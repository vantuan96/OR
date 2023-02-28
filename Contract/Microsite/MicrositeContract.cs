using ProtoBuf;
using System;

namespace Contract.Microsite
{
    [ProtoContract]
    public class MicrositeContract
    {
        [ProtoMember(1)]
        public int MsId { get; set; }

        [ProtoMember(9)]
        public string Title { get; set; }

        [ProtoMember(2)]
        public int Status { get; set; }

        [ProtoMember(3)]
        public bool IsRootSite { get; set; }

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

        [ProtoMember(10)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(11)]
        public string ReferenceCode { get; set; }

        [ProtoMember(12)]
        public int MstId { get; set; }

        [ProtoMember(13)]
        public string MstName { get; set; }

        [ProtoMember(14)]
        public string ImageUrl { get; set; }

        [ProtoMember(15)]
        public string GalleryKey { get; set; }
    }
}