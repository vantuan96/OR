using ProtoBuf;
using System.Collections.Generic;

namespace BMS.Contract.Menu
{
    [ProtoContract]
    public class MenuItemDetailContract
    {
        [ProtoMember(1)]
        public int MenuItemId { get; set; }
        [ProtoMember(2)]
        public int MsId { get; set; }
        [ProtoMember(3)]
        public int Status { get; set; }
        [ProtoMember(4)]
        public bool IsOnsite { get; set; }
        [ProtoMember(5)]
        public int Sort { get; set; }
        [ProtoMember(6)]
        public List<MenuItemContentContract> ListMenuItemContent { get; set; }

        [ProtoMember(7)]
        public bool IsPredefined { get; set; }

        [ProtoMember(8)]
        public bool IsTemplate { get; set; }
    }
}
