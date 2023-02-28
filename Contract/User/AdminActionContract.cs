using ProtoBuf;
using System.ComponentModel;

namespace Contract.User
{
    [ProtoContract]
    public class AdminActionContract
    {
        [ProtoMember(1)]
        [Description("Tên Controller")]
        public string ControllerName { get; set; }

        [ProtoMember(2)]
        public string ControllerDisplayName { get; set; }

        [ProtoMember(3)]
        public string ControllerCssIcon { get; set; }

        [ProtoMember(4)]
        public int ControllerSort { get; set; }

        [ProtoMember(5)]
        public string ActionName { get; set; }

        [ProtoMember(6)]
        public string ActionDisplayName { get; set; }

        [ProtoMember(7)]
        public string ActionCssIcon { get; set; }

        [ProtoMember(8)]
        public bool IsDefault { get; set; }

        [ProtoMember(9)]
        public bool IsShowMenu { get; set; }

        [ProtoMember(10)]
        public int ActionSort { get; set; }
    }
}
