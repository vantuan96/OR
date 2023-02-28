using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VG.General.ExceptionHandling;

namespace Business
{
    public class BaseBusiness
    {
        protected string appid { get; private set; }
        protected int uid { get; private set; }

        private string _defaultLanguageCode = null;

        protected string defaultLanguageCode
        {
            get
            {
                if (_defaultLanguageCode == null)
                {
                    _defaultLanguageCode = "vi";
                }

                return _defaultLanguageCode;
            }
        }

        public BaseBusiness(string appid, int uid)
        {
            this.appid = appid;
            this.uid = uid;
        }

        public T ProcessRequest<T>()
        {
            return default(T);
        }

        /// <summary>
        /// Thực thi hàm xử lý tầng Business
        /// </summary>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="exeCode"></param>
        /// <returns></returns>
        protected Tout ProcessBiz<Tout>(Func<Tout> exeCode, string typeName, string methodName)
        {
            try
            {
                return exeCode.Invoke();
            }
            catch (VGException ex)
            {
                ex.AddSource(String.Format(ConstValue.ErrorSourceFormat, typeName, methodName), ex.ErrorStackTrace);
                throw ex;
            }
            catch (Exception ex)
            {
                throw new VGException(ErrorSeverity.Error, ErrorCode.CouldNotExeBizLayer,
                    string.Format(ConstValue.ErrorSourceFormat, typeName, methodName),
                    ex.Message, ex.StackTrace);
            }
        }

        public string ConvertLocationPath2Name(string input, List<Location> allLocation)
        {
            var listNode = input.Split('.');
            var locationName = "";
            foreach (var item in listNode)
            {
                locationName += allLocation.Single(n => n.LocationId.ToString() == item).NameVN + " > ";
            }

            return locationName.TrimEnd(" > ".ToCharArray());
        }
    }
}