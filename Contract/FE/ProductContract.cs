using ProtoBuf;
using System;
using System.Collections.Generic;

namespace BMS.Contract.FE
{
    [ProtoContract]
    public class ProductContract
    {
        [ProtoMember(1)]
        public int ProductId { get; set; }

        [ProtoMember(3)]
        public int Status { get; set; }

        [ProtoMember(4)]
        public int? GalleryId { get; set; }

        [ProtoMember(5)]
        public string ImageUrl { get; set; }

        [ProtoMember(6)]
        public int MsId { get; set; }

        [ProtoMember(7)]
        public int ProductCategoryId { get; set; }

        [ProtoMember(8)]
        public string ProductCategoryName { get; set; }
        
        [ProtoMember(11)]
        public int CreatedBy { get; set; }

        [ProtoMember(12)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(13)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(14)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(15)]
        public ProductContentContract Content { get; set; }

        [ProtoMember(16)]
        public List<ProductAttributeContract> ListAttribute { get; set; }

    }


    [ProtoContract]
    public class ProductContentContract
    {
        [ProtoMember(1)]
        public int ProductContentId { get; set; }

        [ProtoMember(2)]
        public string NameLine1 { get; set; }

        [ProtoMember(3)]
        public int ProductId { get; set; }

        [ProtoMember(4)]
        public string LangShortName { get; set; }

        [ProtoMember(5)]
        public string ShortDescription { get; set; }

        [ProtoMember(6)]
        public string Description { get; set; }

        [ProtoMember(7)]
        public string Body { get; set; }

        [ProtoMember(8)]
        public string RewriteUrl { get; set; }

        [ProtoMember(9)]
        public string MetaTitle { get; set; }

        [ProtoMember(10)]
        public string MetaDescription { get; set; }

        [ProtoMember(11)]
        public string MetaKeyword { get; set; }

        [ProtoMember(16)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(17)]
        public string NameLine2 { get; set; }

    }

    [ProtoContract]
    public class ProductAttributeContract
    {
        [ProtoMember(1)]
        public int AttrId { get; set; }
       
        [ProtoMember(7)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(8)]
        public bool IsOnsite { get; set; }

        [ProtoMember(9)]
        public bool IsThumbnail { get; set; }

        [ProtoMember(10)]
        public bool IsRequired { get; set; }

        [ProtoMember(11)]
        public string ImageUrl { get; set; }

        [ProtoMember(12)]
        public int Sort { get; set; }

        [ProtoMember(14)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public string Value { get; set; }

        [ProtoMember(15)]
        public string Key { get; set; }
    }


 

}
