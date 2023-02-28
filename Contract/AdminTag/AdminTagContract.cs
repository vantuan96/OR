using ProtoBuf;

namespace BMS.Contract.AdminTag
{
    [ProtoContract]
    public class AdminTagContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(22)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public string Description { get; set; }

        [ProtoMember(4)]
        public bool IsPredefined { get; set; }

        [ProtoMember(5)]
        public int Sort { get; set; }

        [ProtoMember(6)]
        public int CreatedBy { get; set; }

        [ProtoMember(7)]
        public System.DateTime CreatedDate { get; set; }

        [ProtoMember(8)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(9)]
        public System.DateTime LastUpdatedDate { get; set; }
    }
}