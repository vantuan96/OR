using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public class ORNotifyMail
    {
        public string HospitalCode { get; set; }
        public string SiteName { get; set; }
        public string Email { get; set; }
        public List<ORAnesthProgressContract> listData { get; set; }
    }
}
