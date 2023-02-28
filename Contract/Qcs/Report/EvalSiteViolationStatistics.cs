using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Qcs.Report
{
    [ProtoContract]
    public class EvalSiteViolationStatisticsContract
    {
        /// <summary>
        /// Mã Microsite (mã trung tâm thương mại)
        /// </summary>
        [ProtoMember(1)]
        public int MsId { get; set;  }

        /// <summary>
        /// Tên microsite (tên trung tâm thương mại)
        /// </summary>
        [ProtoMember(2)]
        public string MsTitle { get; set; }

        /// <summary>
        /// Tổng số lỗi
        /// </summary>
        [ProtoMember(3)]
        public int TotalViols { get; set; }

        /// <summary>
        /// Tổng số lỗi đã giải trình
        /// </summary>
        [ProtoMember(4)]
        public int TotalViolExplaned { get; set; }

        /// <summary>
        /// Tổng số lỗi chưa giải trình
        /// </summary>
        [ProtoMember(5)]
        public int TotalViolNotExplaned { get; set; }

        /// <summary>
        /// Tổng số lỗi chưa đến hạn giải trình
        /// </summary>
        [ProtoMember(6)]
        public int TotalViolDateExplanationNotExpired { get; set; }
        
        /// <summary>
        /// Tổng số lỗi quá hạn giải trình
        /// </summary>
        [ProtoMember(7)]
        public int TotalViolDateExplanationExpired { get; set; }        

        /// <summary>
        /// Tổng số lỗi quá hạn xử lý và hoàn thành
        /// </summary>
        [ProtoMember(8)]
        public int TotalViolDateExplanationExpiredWithCompleteStatus { get; set; }

        /// <summary>
        /// Tổng số lỗi tạm dừng
        /// </summary>
        [ProtoMember(9)]
        public int TotalViolWithStopStatus { get; set; }

        /// <summary>
        /// Tổng số hoàn thành
        /// </summary>
        [ProtoMember(10)]
        public int TotalViolWithCompleteStatus { get; set; }

        public decimal PercentViolDateExplanationExpiredCompletedStatus { get { return (TotalViols == 0 ? 0 : Math.Round(((decimal)TotalViolDateExplanationExpiredWithCompleteStatus / (decimal)TotalViols * 100), 2)); } }

        public decimal PercentViolCompletedStatus { get { return (TotalViols == 0 ? 0 : Math.Round(((decimal)TotalViolWithCompleteStatus / (decimal)TotalViols * 100), 1)); } }

        public string DisplayViolWithCompleteStatus
        {
            get 
            {
                return string.Format("{0}({1}%)", TotalViolWithCompleteStatus, PercentViolCompletedStatus);                
            }
        }
        
        public string DisplayViolDateExplanationExpiredWithCompleteStatus
        {
            get
            {
                return string.Format("{0}({1}%)", TotalViolDateExplanationExpiredWithCompleteStatus, PercentViolDateExplanationExpiredCompletedStatus);                
            }
        }
    }
}
