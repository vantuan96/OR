using Contract.OR;
using System.Web;

namespace Admin.Shared
{
    public class ORVisitBase
    {
        public ORVisitModel visitCurrent { get; private set; }
        public string KeyValue { get; set; }
        #region "Implement"
        public static ORVisitBase Instance;
        //public ORVisitBase Instance;
        protected ORVisitBase() { }
        static ORVisitBase() { }
        public ORVisitBase(string token, string currentUser)
        {
            string strKey = $"{token}_{currentUser}";
            if (GetInstance(strKey) == null)
            {
                Instance = new ORVisitBase();
                Instance.KeyValue = strKey;
                Instance.visitCurrent = null;
                UpdateInstance(Instance);
            }
            else
            {
                Instance = (ORVisitBase)GetInstance(strKey);
            }
        }
        #endregion

        #region private method 
        /// <summary>
        /// cập nhật instance
        /// </summary>
        /// <param name="Instance"></param>
        private void UpdateInstance(ORVisitBase Instance)
        {
            HttpContext.Current.Application[Instance.KeyValue + "-VinmecORVisitCurrent"] = Instance;
            //HttpContext.Current.Application["-VinmecORVisitCurrent"] = Instance;
        }
        private object GetInstance(string strKey)
        {
            return HttpContext.Current.Application[strKey + "-VinmecORVisitCurrent"];
            //return HttpContext.Current.Application["-VinmecORVisitCurrent"];
        }
        public void ResetVisit()
        {
            Instance.visitCurrent = null;
            UpdateInstance(Instance);
        }
        public void SetCurrentVisit(ORVisitModel visit)
        {
            Instance.visitCurrent = visit;
            UpdateInstance(Instance);
        }
        public ORVisitModel GetCurrentVisit()
        {
            return Instance.visitCurrent;

        }
        #endregion
    }
}
