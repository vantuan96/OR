using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public class HpServiceContract
    {
        public int Id { get; set; }
        public string Oh_Code { get; set; }
        public string Name { get; set; }
        public int CleaningTime { get; set; }
        public int PreparationTime { get; set; }
        public int AnesthesiaTime { get; set; }
        public int OtherTime { get; set; }
        public string Description { get; set; }
        public string IdMapping { get; set; }
        public int SourceClientId { get; set; }
        public int Sort { get; set; }
        public int Type { get; set; }
        #region Infor for Charge
        public string OrderID { get; set; }
        public string ChargeDetailId { get; set; }
        public string DepartmentCode { get; set; }
        public string LocationName { get; set; }
        //vutv7
        public string ChargeDate { get; set; }
        public string ChargeBy { get; set; }
        #endregion
    }

    public class HpServiceSite
    {
        public int Id { get; set; }
        public string Oh_Code { get; set; }
        public string Name { get; set; }
        public int CleaningTime { get; set; }
        public int PreparationTime { get; set; }
        public int AnesthesiaTime { get; set; }
        public int OtherTime { get; set; }
        public string Description { get; set; }
        public string IdMapping { get; set; }
        public int SourceClientId { get; set; }
        public int Type { get; set; }
        public int Sort { get; set; }
        public int TotalRecords { get; set; }
        public List<SiteShortContract> listSites { get; set; }

        public HpServiceSite()
        {
            listSites = new List<SiteShortContract>();
        }
    }
    public  class SiteShortContract
    {
        public string Id { get; set; }
        public string SiteName { get; set; }
        public string HospitalCode { get; set; }
    }
    
}
