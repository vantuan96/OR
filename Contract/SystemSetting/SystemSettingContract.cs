using ProtoBuf;
using System;

namespace Contract.SystemSetting
{
    [ProtoContract]
    public class SystemSettingContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Key { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; }

        [ProtoMember(4)]
        public string Value { get; set; }

        [ProtoMember(5)]
        public string Description { get; set; }

        [ProtoMember(6)]
        public bool IsAdminConfig { get; set; }

        [ProtoMember(7)]
        public int CreatedBy { get; set; }

        [ProtoMember(8)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(9)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(10)]
        public DateTime LastUpdatedDate { get; set; }
    }
}