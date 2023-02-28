using ProtoBuf;
using System.Collections.Generic;

namespace BMS.Contract.FE
{
    [ProtoContract]
    public class MicrositeItemContract
    {
        [ProtoMember(1)]
        public int MsId { get; set; }

        [ProtoMember(2)]
        public MicrositeContentContract Content { get; set; }

        [ProtoMember(3)]
        public bool IsRootSite { get; set; }

        [ProtoMember(4)]
        public string ReferenceCode { get; set; }

        [ProtoMember(5)]
        public List<AddressContract> Addresses { get; set; }

        public int CityRegionId
        {
            get
            {
                if (Addresses == null || Addresses.Count == 0)
                    return int.MaxValue;

                return Addresses[0].RegionId ?? int.MaxValue;
            }
        }

        public int CityId
        {
            get
            {
                if (Addresses == null || Addresses.Count == 0)
                    return int.MaxValue;

                return Addresses[0].CityId;
            }
        }

        public string Title
        {
            get
            {
                if (Content == null)
                    return string.Empty;

                return Content.Title;
            }
        }
    }

    [ProtoContract]
    public class MicrositeContract
    {
        [ProtoMember(1)]
        public int MsId { get; set; }

        [ProtoMember(2)]
        public MicrositeContentContract Content { get; set; }

        [ProtoMember(3)]
        public bool IsRootSite { get; set; }

        [ProtoMember(4)]
        public string ReferenceCode { get; set; }

        [ProtoMember(5)]
        public List<AddressContract> Addresses { get; set; }

        [ProtoMember(6)]
        public string MicrositeTypeName { get; set; }

        [ProtoMember(7)]
        public string GalleryKey { get; set; }

        [ProtoMember(8)]
        public List<string> GalleryImageUrls { get; set; }

        [ProtoMember(9)]
        public int MicrositeTypeSort { get; set; }

        public int CityRegionId
        {
            get
            {
                if (Addresses == null || Addresses.Count == 0)
                    return int.MaxValue;

                return Addresses[0].RegionId ?? int.MaxValue;
            }
        }

        public int CityId
        {
            get
            {
                if (Addresses == null || Addresses.Count == 0)
                    return int.MaxValue;

                return Addresses[0].CityId;
            }
        }

        public string Title
        {
            get
            {
                if (Content == null)
                    return string.Empty;

                return Content.Title;
            }
        }
    }

    [ProtoContract]
    public class MicrositeContentContract
    {
        [ProtoMember(1)]
        public string Title { get; set; }

        [ProtoMember(2)]
        public string Rewrite { get; set; }

        [ProtoMember(3)]
        public string ShortDescription { get; set; }

        [ProtoMember(4)]
        public string Description { get; set; }

        [ProtoMember(5)]
        public string ImageUrl { get; set; }

        [ProtoMember(6)]
        public string ImageMobileUrl { get; set; }

        [ProtoMember(7)]
        public string MetaTitle { get; set; }

        [ProtoMember(8)]
        public string MetaKeyword { get; set; }

        [ProtoMember(9)]
        public string MetaDescription { get; set; }

        [ProtoMember(10)]
        public string BannerUrl { get; set; }
    }
   
}
