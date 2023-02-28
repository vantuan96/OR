using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Product
{
    [ProtoContract]
    public class CreateUpdateProductContentContract
    {
        [ProtoMember(1)]
        public int ProductId { get; set; }

        [ProtoMember(2)]
        public string LangShortName { get; set; }

        [ProtoMember(3)]
        public string NameLine1 { get; set; }

        [ProtoMember(4)]
        public string ShortDescription { get; set; }

        [ProtoMember(5)]
        public string Description { get; set; }

        [ProtoMember(6)]
        public string Body { get; set; }

        [ProtoMember(7)]
        public string RewriteUrl { get; set; }

        [ProtoMember(8)]
        public string MetaTitle { get; set; }

        [ProtoMember(9)]
        public string MetaDescription { get; set; }

        [ProtoMember(10)]
        public string MetaKeyword { get; set; }

        [ProtoMember(11)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(12)]
        public int ProductContentId { get; set; }

        [ProtoMember(13)]
        public string NameLine2 { get; set; }


    }
}
