using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.QueuePatient
{
    public class QueuePatientSearchParam
    {
        public string Keyword { get; set; }
        public int p { get; set; }
        public int PageSize { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int StateId { get; set; }
        public int RoomId { get; set; }
        public int HpServiceId { get; set; }
        public Boolean export { get; set; }


        public QueuePatientSearchParam()
        {
            p = 1;
            PageSize = 17;
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
        }
    }
}
