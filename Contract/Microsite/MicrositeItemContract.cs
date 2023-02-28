using ProtoBuf;

namespace Contract.Microsite
{
    [ProtoContract]
    public class MicrositeItemContract
    {
        [ProtoMember(1)]
        public int MsId { get; set; }

        [ProtoMember(2)]
        public string Title { get; set; }

        [ProtoMember(3)]
        public bool IsRootSite { get; set; }

        [ProtoMember(4)]
        public string ReferenceCode { get; set; }

        [ProtoMember(5)]
        public int StatusId { get; set; }
    }
}
