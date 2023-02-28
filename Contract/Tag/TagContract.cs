using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Tag
{
    [ProtoContract]
    public class TagContract
    {
        [ProtoMember(1)]
        public int TagId { get; set; }


        [ProtoMember(3)]
        public int ApprovalStatus { get; set; }


        [ProtoMember(5)]
        public int CreatedBy { get; set; }


        [ProtoMember(7)]
        public System.DateTime CreatedDate { get; set; }

        [ProtoMember(8)]
        public int LastUpdatedBy { get; set; }


        [ProtoMember(10)]
        public System.DateTime LastUpdatedDate { get; set; }

        [ProtoMember(11)]
        public string Name { get; set; }

        [ProtoMember(12)]
        public string RewriteUrl { get; set; }

        [ProtoMember(13)]
        public string LangShortName { get; set; }


    }
}
