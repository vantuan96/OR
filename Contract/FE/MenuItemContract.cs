using ProtoBuf;

namespace BMS.Contract.FE
{
    [ProtoContract]
    public class MenuItemContract
    {
        [ProtoMember(1)]
        public int MenuItemId { get; set; }

        [ProtoMember(2)]
        public bool IsPredefined { get; set; }

        [ProtoMember(3)]
        public MenuItemContentContract Content { get; set; }

        [ProtoMember(4)]
        public MenuItemContentContract ContentDefaultLanguage { get; set; }

        [ProtoMember(5)]
        public int Sort { get; set; }

        [ProtoMember(6)]
        public int ParentId { get; set; }
    }

    [ProtoContract]
    public class MenuItemContentContract
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public string Url { get; set; }

        [ProtoMember(3)]
        public string MetaTitle { get; set; }

        [ProtoMember(4)]
        public string MetaDescription { get; set; }

        [ProtoMember(5)]
        public string MetaKeyword { get; set; }
    }
}
