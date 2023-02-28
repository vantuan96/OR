using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public class ORMappingEkipContract
    {
        public long Id { get; set; }
        public string HospitalCode { get; set; }
        public int UId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone{ get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public int ORAnesthProgessId { get; set; }
        public int TypePageId { get; set; }
      
    }
}
