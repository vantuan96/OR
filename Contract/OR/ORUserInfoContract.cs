using System.ComponentModel;

namespace Contract.OR
{
    public class ORUserInfoContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int PositionId { get; set; }
        public string Phone { get; set; }
        public string PositionName { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        //linhht
        public string Username { get; set; }
    }
}
