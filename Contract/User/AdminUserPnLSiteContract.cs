using System;

namespace Contract.User
{
    public class AdminUserPnLSiteContract
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public int PnLId { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}