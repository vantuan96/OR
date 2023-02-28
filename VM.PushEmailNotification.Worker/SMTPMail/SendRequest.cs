using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OREmailNoti.SMTPMail
{
    [ProtoContract]
    public class SendResponse
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public string Message { get; set; }
    }
    [ProtoContract]
    public class SendResponseResult
    {
        [ProtoMember(1)]
        public SendResponse Status { get; set; }
        [ProtoMember(2)]
        public string Email { get; set; }

    }
    [ProtoContract]
    public class SendResponseBookingResult
    {
        [ProtoMember(1)]
        public SendResponse Status { get; set; }
        [ProtoMember(2)]
        public int ID { get; set; }
    }
    [ProtoContract]
    public class VinDomainObj
    {
        [ProtoMember(1)]
        public int VDID { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string SyntaxMail { get; set; }
    }
}
