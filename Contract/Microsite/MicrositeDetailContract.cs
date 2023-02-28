using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Contract.Microsite
{
    [ProtoContract]
    public class MicrositeDetailContract
    {
        [ProtoMember(1)]
        public int MsId { get; set; }

        [ProtoMember(2)]
        public int Status { get; set; }

        [ProtoMember(3)]
        public bool IsRootSite { get; set; }

        [ProtoMember(4)]
        public int CreatedBy { get; set; }

        [ProtoMember(5)]
        public string CreatedByName { get; set; }

        [ProtoMember(6)]
        public DateTime CreatedDate {get;set;}

        [ProtoMember(7)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(8)]
        public string LastUpdatedByName { get; set; }

        [ProtoMember(9)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(11)]
        public List<MicrositeContentContract> ListMicrositeContent { get; set; }

        [ProtoMember(12)]
        public string ReferenceCode { get; set; }

        [ProtoMember(13)]
        public int MstId { get; set; }

        [ProtoMember(14)]
        public string GalleryKey { get; set; }
    }
}
