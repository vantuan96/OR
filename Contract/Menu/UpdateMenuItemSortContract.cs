using ProtoBuf;

namespace BMS.Contract.Menu
{
    [ProtoContract]
    public class UpdateMenuItemSortContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(3)]
        public int ParentId { get; set; }
        [ProtoMember(4)]
        public int Order { get; set; }

        [ProtoMember(5)]
        public bool IsOnsite { get; set; }

    }
}
