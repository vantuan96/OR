using ProtoBuf;

namespace BMS.Contract.ArticleCategory
{
    [ProtoContract]
    public class UpdateCategorySortContract
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
