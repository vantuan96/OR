using Contract.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching.Core
{
    public class UserCaching
    {
        public readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Lazy<IAuthenCaching> authenCaching = new Lazy<IAuthenCaching>(() => new AuthenCaching());
        private static UserCaching _instant { get; set; }
        public static UserCaching Instant
        {
            get
            {
                if (_instant == null)
                    _instant = new UserCaching();
                return _instant;
            }
            set
            {
                _instant = value;
            }
        }
        #region Cache User Model
        public LoginContract GetUserModel(string userKey = "",string ipClient="")
        {
            string keyCaches = string.Format("{0}", CachingConstant.CachingKey.ListUser);
            LoginContract entity = null;
            if (!string.IsNullOrEmpty(userKey) || !string.IsNullOrEmpty(ipClient))
            {
                var listResult = BaseCaching.Instant.GetValue2<List<LoginContract>>(keyCaches);
                if (listResult != null && listResult.Count > 0)
                {
                    //entity = listResult.Where(x => (!string.IsNullOrEmpty(x.NickName) && x.NickName.ToLower().Equals(userKey.ToLower()))
                    // || (!string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(userKey.ToLower()))
                    // || (!string.IsNullOrEmpty(x.TokenKey) && x.TokenKey.ToLower().Equals(userKey.ToLower()))
                    // || (!string.IsNullOrEmpty(ipClient) && !string.IsNullOrEmpty(x.ClientIp) && x.ClientIp.ToLower().Equals(ipClient.ToLower()))
                    // ).SingleOrDefault();
                    ////Neu null kiem tra trong Database
                    //if (entity == null)
                    //{
                        entity = authenCaching.Value.CheckLoginByForCheckLogin(userKey);
                        if (entity != null && entity.UserId > 0)
                        {
                            AddUserModel(entity, entity.IsLogon);
                        //}
                    }
                }
                else
                {
                    entity = authenCaching.Value.CheckLoginByForCheckLogin(userKey);
                    if (entity != null && entity.UserId > 0)
                    {
                        AddUserModel(entity, entity.IsLogon);
                    }
                }
            }
            return (entity!=null && entity.ResCode==Contract.Shared.ResponseCode.Successed)? entity: null;
        }
        public LoginContract AddUserModel(LoginContract entity, bool isLogon=false)
        {
            string keyCaches = string.Format("{0}", CachingConstant.CachingKey.ListUser);
            var listResult = BaseCaching.Instant.GetValue2<List<LoginContract>>(keyCaches);
            //log.Debug("AddUserModel step 1");
            if (listResult != null && listResult.Count > 0)
            {
                //log.Debug("AddUserModel step 2");
                // Update Object
                var entiyExist = listResult.Where(x => (!string.IsNullOrEmpty(x.NickName) && x.NickName.ToLower().Equals(entity.NickName.ToLower()))
                  || (!string.IsNullOrEmpty(x.Email) && !string.IsNullOrEmpty(entity.Email) && x.Email.ToLower().Equals(entity.Email.ToLower()))
                  || (!string.IsNullOrEmpty(entity.ClientIp) && !string.IsNullOrEmpty(x.ClientIp) && x.ClientIp.ToLower().Equals(entity.ClientIp.ToLower()))
                  ).FirstOrDefault();
                if (entiyExist != null)
                {
                    if (isLogon && string.IsNullOrEmpty(entity.TokenKey))
                    {
                        entity.TokenKey = entiyExist.TokenKey;
                    }
                    listResult[listResult.FindIndex(ind => ind.Equals(entiyExist))] = entity;
                    //log.Debug("AddUserModel step 3");
                }
                else
                {
                    listResult.Add(entity);
                    //log.Debug("AddUserModel step 4");
                }
                    
            }
            else
            {
                listResult = new List<LoginContract>();
                listResult.Add(entity);
                //log.Debug("AddUserModel step 5");
            }
            //Insert to Cache
            //Set Cache to
            BaseCaching.Instant.Add2(keyCaches, listResult, 8);
            //log.Debug("AddUserModel step 6");
            //Update into Database
            //authenCaching.Value.UpdateLoginLogout(entity, isLogon);
            //log.Debug("AddUserModel step 7");
            return entity;
        }
        #endregion Cache User Model
    }
}
