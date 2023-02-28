namespace Contract.User
{
    public class AdminUserPnLContract
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public int PnLId { get; set; }
        public string PnLName { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}