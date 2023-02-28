using System;
namespace Contract.Log
{
    public class LogObjectContract
    {
        public long Id { get; set; }
        public long ObjectId { get; set; }
        public int OldState { get; set; }
        public int NewState { get; set; }
        public int ObjectTypeId { get; set; }
        public int ActionId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserAssigned { get; set; }
        public string OldInformation { get; set; }
        public string NewInformation { get; set; }
        public string CreatedByName { get; set; }
    }
}
