using Contract.User;

namespace Business.Functions
{
    public class SessionBusiness
    {
        private static SessionBusiness instant;
        public static SessionBusiness Instant
        {
            get
            {
                if (instant == null)
                    instant = new SessionBusiness();
                return instant;
            }
        }

        public MemberExtendContract CurrentUser
        {
            get
            {
                var item = Share.ShareFunctions.GetSession<MemberExtendContract>("adr.User");
                if (item == null)
                    return new MemberExtendContract();
                return item;
            }
            set
            {
                Share.ShareFunctions.SetSession("adr.User", value);
            }
        }
    }
}
