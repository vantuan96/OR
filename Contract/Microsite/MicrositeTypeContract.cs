using ProtoBuf;

namespace Contract.Microsite
{
    [ProtoContract]
    public class MicrositeTypeContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(222)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(4)]
        public bool IsOnsite { get; set; }

        [ProtoMember(5)]
        public int CreatedBy { get; set; }

        [ProtoMember(6)]
        public System.DateTime CreatedDate { get; set; }

        [ProtoMember(7)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(8)]
        public System.DateTime LastUpdatedDate { get; set; }
    }
}