using ProtoBuf;
using System;

namespace BMS.Contract.Image
{
    [ProtoContract]
    public class ImageContentContract
    {
        [ProtoMember(1)]
        public int ImageContentId { get; set; }
        [ProtoMember(2)]
        public int ImageId { get; set; }
        [ProtoMember(3)]
        public string Title { get; set; }
        [ProtoMember(4)]
        public string ShortDescription { get; set; }
        [ProtoMember(5)]
        public string Description { get; set; }
        [ProtoMember(6)]
        public string TargetUrl { get; set; }
        [ProtoMember(7)]
        public string LangShortName { get; set; }
        [ProtoMember(8)]
        public string AltText { get; set; }
        [ProtoMember(9)]
        public string MetaTitle { get; set; }
        [ProtoMember(10)]
        public string MetaDescription { get; set; }
        [ProtoMember(11)]
        public string MetaKeyword { get; set; }
        [ProtoMember(12)]
        public int CreatedBy { get; set; }
        [ProtoMember(13)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(14)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(15)]
        public DateTime LastUpdatedDate { get; set; }

        public bool IsNotSetValue
        {
            get
            {
                return string.IsNullOrEmpty(Title)
                    && string.IsNullOrEmpty(ShortDescription)
                    && string.IsNullOrEmpty(Description)
                    && string.IsNullOrEmpty(TargetUrl)
                    && string.IsNullOrEmpty(AltText)
                    && string.IsNullOrEmpty(MetaTitle)
                    && string.IsNullOrEmpty(MetaDescription)
                    && string.IsNullOrEmpty(MetaKeyword);
            }
        }
    }
}
