using Contract.OR;
using System.Web;

namespace Admin.Shared
{
    public class ORVisitLink
    {
        public ORLinkActive visitCurrent { get; private set; }
        public string Token { get; set; }
        #region "Implement"
        public static ORVisitLink Instance;
        protected ORVisitLink() { }
        static ORVisitLink() { }
        public ORVisitLink(string token)
        {
            if (GetInstance(token) == null)
            {
                Instance = new ORVisitLink();
                Instance.Token = token;
                Instance.visitCurrent = null;
                UpdateInstance(Instance);
            }
            else
            {
                Instance = (ORVisitLink)GetInstance(token);
            }
        }
        #endregion

        #region private method 
        /// <summary>
        /// cập nhật instance
        /// </summary>
        /// <param name="Instance"></param>
        private void UpdateInstance(ORVisitLink Instance)
        {
            HttpContext.Current.Application[Instance.Token + "-VinmecORVisitLinkCurrent"] = Instance;
        }
        private object GetInstance(string token)
        {
            return HttpContext.Current.Application[token + "-VinmecORVisitLinkCurrent"];
        }
        public void ResetVisit()
        {
            Instance.visitCurrent = null;
            UpdateInstance(Instance);
        }
        public void SetCurrentVisit(ORLinkActive visit)
        {
            Instance.visitCurrent = visit;
            UpdateInstance(Instance);
        }
        public ORLinkActive GetCurrentVisit()
        {
            return Instance.visitCurrent;
        }
        #endregion
    }
}
