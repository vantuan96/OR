namespace Contract.User
{
    public class LoginRequestContract
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string GCM_RegId { get; set; }

        public string IOS_FCM_RegId { get; set; }

        public string IpAddress { get; set; } // không cần truyền

        public string UserAgent { get; set; } // không cần truyền
    }
}
