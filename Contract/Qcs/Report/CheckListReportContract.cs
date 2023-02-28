using ProtoBuf;
using System.Collections.Generic;
using System.Linq;
using BMS.Contract.Qcs;
using BMS.Contract.Qcs.EvaluationSiteZones;

namespace BMS.Contract.Report
{

    [ProtoContract]
    public class CheckListReportContract
    {
        /// <summary>
        /// danh sach ky danh gia
        /// </summary>
        [ProtoMember(1)]
        public List<EvaluationCalendarInfoContract> ListEvaluationCalendar { get; set; }


        /// <summary>
        /// danh sach sitezone
        /// </summary>
        [ProtoMember(2)]
        public List<EvaluationSiteZoneContact> ListSiteZone { get; set; }



    }


    [ProtoContract]
    public class EvaluationSiteZoneContact
    {

        [ProtoMember(1)]
        public int SiteZoneId { get; set; }

        /// <summary>
        /// tong so danh sach danh gia
        /// </summary>
        //[ProtoMember(2)]
        public int TotalCriteria //{ get; set; }
        {
            get
            {
                return EvaluationCriterias != null ? EvaluationCriterias.Count() : 0;
            }
        }

        /// <summary>
        /// tong so diem
        /// </summary>
        //[ProtoMember(3)]
        public int TotalCriteriaPoint //{ get; set; }
        {
            get
            {
                return EvaluationCriterias != null ? EvaluationCriterias.Select(r => r.Point).DefaultIfEmpty(0).Sum() : 0;
            }
        }


        [ProtoMember(4)]
        public int SiteId { get; set; }


        /// <summary>
        /// tong so danh sach danh gia
        /// </summary>
        //[ProtoMember(5)]
        public int TotalValidCriteria //{ get; set; }
        {
            get
            {
                return CheckedEvaluationCriterias != null ? CheckedEvaluationCriterias.Where(r=>r.StatusId != (int)EvaluationStatus.NotYetEvaluated).Count() : 0;
            }
        }


        /// <summary>
        /// Tổng số điểm đã đánh giá
        /// </summary>
        //[ProtoMember(7)]
        public int TotalValidCriteriaPoint
        {
            get
            {
                return CheckedEvaluationCriterias != null ?  CheckedEvaluationCriterias.Where(r => r.StatusId != (int)EvaluationStatus.NotYetEvaluated)
                                            .Select(r => r.Point).DefaultIfEmpty(0).Sum() : 0;
            }
        }

        /// <summary>
        /// tong so lỗi
        /// </summary>
        //[ProtoMember(6)]
        public int TotalInvalidCriteria// { get; set; }
        {
            get
            {
                return CheckedEvaluationCriterias != null ? CheckedEvaluationCriterias.Where(r => r.StatusId == (int)EvaluationStatus.NotPass).Count() : 0;
            }
        }

        /// <summary>
        /// tong so diem lỗi
        /// </summary>
        //[ProtoMember(7)]
        public int TotalInvalidCriteriaPoint// { get; set; }
        {
            get
            {

                return CheckedEvaluationCriterias != null ?  CheckedEvaluationCriterias.Where(r => r.StatusId == (int)EvaluationStatus.NotPass)
                                            .Select(r => r.Point).DefaultIfEmpty(0).Sum() : 0;
            }
        }
       

        [ProtoMember(10)]
        public List<EvaluationCriteriaContract> EvaluationCriterias { get; set; }


        [ProtoMember(11)]
        public List<CheckedEvaluationCriteriaContract> CheckedEvaluationCriterias { get; set; }

    }


    [ProtoContract]
    public class EvaluationCriteriaContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public int Point { get; set; }

    }


    [ProtoContract]
    public class CheckedEvaluationCriteriaContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public int Point { get; set; }

        [ProtoMember(3)]
        public int StatusId { get; set; }

    }

}