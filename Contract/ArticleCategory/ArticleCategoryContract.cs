using ProtoBuf;
using System;
using System.Collections.Generic;

namespace BMS.Contract.ArticleCategory
{
    [ProtoContract]
    public class ArticleCategoryContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public int ApprovalStatus { get; set; }
        [ProtoMember(3)]
        public bool IsOnsite { get; set; }
        [ProtoMember(4)]
        public int MsId { get; set; }
        [ProtoMember(5)]
        public Nullable<int> ParentId { get; set; }
        [ProtoMember(6)]
        public int CreatedBy { get; set; }
        [ProtoMember(7)]
        public System.DateTime CreatedDate { get; set; }
        [ProtoMember(8)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(9)]
        public System.DateTime LastUpdatedDate { get; set; }
        [ProtoMember(10)]
        public string Name { get; set; }

        [ProtoMember(11)]
        public string ParentName { get; set; }

        [ProtoMember(12)]
        public string Key { get; set; }

        [ProtoMember(13)]
        public bool IsPredefined { get; set; }

    }
}
