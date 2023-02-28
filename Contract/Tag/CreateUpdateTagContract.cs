using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Tag
{

    [ProtoContract]
    public class CreateUpdateTagContract
    {
        [ProtoMember(1)]
        public int TagId { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public int ApprovalStatus { get; set; }

        //[ProtoMember(4)]
        //public int MsId { get; set; }

        [ProtoMember(5)]
        public int UserId { get; set; }

        //[ProtoMember(6)]
        //public int Status { get; set; }

        //[ProtoMember(7)]
        //public string LangShortName { get; set; }

    }
}
