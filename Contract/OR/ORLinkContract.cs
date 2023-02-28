using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public class ORLinkContract
    {
        public long Id { get; set; }
        public Guid GuidCode {get;set;}
        public string Code { get; set; }
        public DateTime LimitDate { get; set; }
        public Boolean IsActive { get; set; }
        public string IpActive { get; set; }
    }
    public class ORLinkActive
    {
        public Guid GuidCode { get; set; }
        public string ReDirectUrl { get; set; }
        public Boolean IsValidate { get; set; }
    }
}
