using ProtoBuf;

namespace Contract.User
{
    [ProtoContract]
    public class ClientInfoContract
    {
        [ProtoMember(1)]
        public string Browser { get; set; }

        [ProtoMember(2)]
        public string Platform { get; set; }

        [ProtoMember(3)]
        public string Version { get; set; }

        [ProtoMember(4)]
        public string Type { get; set; }

        [ProtoMember(5)]
        public string IPAddress { get; set; }

        [ProtoMember(6)]
        public string MobileDeviceModel { get; set; }

        [ProtoMember(7)]
        public bool IsMobileDevice { get; set; }
    }
}
