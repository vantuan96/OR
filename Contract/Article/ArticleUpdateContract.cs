using ProtoBuf;

namespace BMS.Contract.Article
{
    [ProtoContract]
    public class ArticleUpdateContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(6)]
        public bool IsHot { get; set; }

        [ProtoMember(7)]
        public bool IsFocus { get; set; }

        [ProtoMember(8)]
        public int Status { get; set; }

        [ProtoMember(9)]
        public int Type { get; set; }

        [ProtoMember(10)]
        public int[] Tags { get; set; }

        [ProtoMember(11)]
        public bool IsVipPromotion { get; set; }

        [ProtoMember(13)]
        public System.DateTime StartDate { get; set; }

        [ProtoMember(14)]
        public System.DateTime EndDate { get; set; }

        [ProtoMember(15)]
        public int CityId { get; set; }

        [ProtoMember(16)]
        public int MsId { get; set; }

        [ProtoMember(19)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(20)]
        public System.DateTime LastUpdatedDate { get; set; }

        [ProtoMember(21)]
        public int ArticleCategoryId { get; set; }

        [ProtoMember(22)]
        public string ArticleCategoryOthersId { get; set; }

        [ProtoMember(23)]
        public int[] AdminTags { get; set; }

        [ProtoMember(24)]
        public string Key { get; set; }
    }
}