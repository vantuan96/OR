using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public partial class HospitalSiteContract
    {
        public string Id { get; set; }
        public string SiteName { get; set; }
        public string SiteNameFull { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int AreaId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
