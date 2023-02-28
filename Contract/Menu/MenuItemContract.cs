using ProtoBuf;

namespace BMS.Contract.Menu
{
    [ProtoContract]
    public class MenuItemContract
    {
        [ProtoMember(1)]
        public int MenuItemId { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(4)]
        public bool IsOnsite { get; set; }

        [ProtoMember(5)]
        public int Sort { get; set; }

        [ProtoMember(6)]
        public int ParentId { get; set; }

        [ProtoMember(7)]
        public bool IsPredefined { get; set; }

        [ProtoMember(8)]
        public int? TemplateId { get; set; }
    }
}
