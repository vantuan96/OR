using ProtoBuf;

namespace BMS.Contract.Menu
{
    [ProtoContract]
    public class MenuItemContentContract
    {
        [ProtoMember(1)]
        public int MsId { get; set; }
        [ProtoMember(2)]
        public bool IsTemplate { get; set; }
        [ProtoMember(3)]
        public int MenuItemId { get; set; }
        [ProtoMember(4)]
        public string LangShortName { get; set; }
        [ProtoMember(5)]
        public string Name { get; set; }
        [ProtoMember(6)]
        public string RedirectUrl { get; set; }
        [ProtoMember(7)]
        public string MetaTitle { get; set; }
        [ProtoMember(8)]
        public string MetaDescription { get; set; }
        [ProtoMember(9)]
        public string MetaKeyword { get; set; }
        [ProtoMember(10)]
        public int MenuItemContentId { get; set; }
        [ProtoMember(11)]
        public bool IsPredefined { get; set; }
    }
}
