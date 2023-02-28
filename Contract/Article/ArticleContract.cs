﻿using ProtoBuf;
using System.Collections.Generic;
using BMS.Contract.AdminTag;
using BMS.Contract.ArticleCategory;
using BMS.Contract.Tag;

namespace BMS.Contract.Article
{
    [ProtoContract]
    public class ArticleContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(3)]
        public string Title { get; set; }

        [ProtoMember(4)]
        public string ImageUrl { get; set; }

        [ProtoMember(5)]
        public string ImageMobileUrl { get; set; }

        [ProtoMember(6)]
        public bool IsHot { get; set; }

        [ProtoMember(7)]
        public bool IsFocus { get; set; }

        [ProtoMember(8)]
        public int Status { get; set; }

        [ProtoMember(9)]
        public int Type { get; set; }

       
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

        [ProtoMember(17)]
        public int CreatedBy { get; set; }

        [ProtoMember(18)]
        public System.DateTime CreatedDate { get; set; }

        [ProtoMember(19)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(20)]
        public System.DateTime LastUpdatedDate { get; set; }
       
        [ProtoMember(21)]
        public int ArticleCategoryId { get; set; }

        [ProtoMember(22)]
        public string ArticleCategoryName { get; set; }

        [ProtoMember(23)]
        public string ArticleCategoryOthersId { get; set; }

        [ProtoMember(24)]
        public string Title2 { get; set; }

        [ProtoMember(25)]
        public string TitleWithoutAccent { get; set; }

        [ProtoMember(26)]
        public string Title2WithoutAccent { get; set; }

        [ProtoMember(27)]
        public List<TagContract> Tags { get; set; }

        [ProtoMember(28)]
        public List<AdminTagContract> AdminTags { get; set; }

        [ProtoMember(29)]
        public string Key { get; set; }


    }
}