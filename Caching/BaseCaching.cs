using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using VG.Common;
using VG.EncryptLib.Logging;
using VG.General.ExceptionHandling;

namespace Caching
{
    public class BaseCaching
    {
        public static BaseCaching _instant;
        public static BaseCaching Instant
        {
            get
            {
                if (_instant == null)
                {
                    _instant = new BaseCaching();
                }
                return _instant;
            }
            set
            {
                _instant = value;
            }
        }

        /// <summary>
        /// Prefix for Redis cache name
        /// </summary>
        protected string cacheNamePrefix;

        /// <summary>
        /// Default dbId
        /// </summary>
        public static int dbId = Convert.ToInt32(ConfigurationManager.AppSettings["Redis.DefaultRedisDbId"]);

        /// <summary>
        /// Logging util
        /// </summary>
        public static LoggingUtil logUtil = new LoggingUtil();

        protected string appid { get; private set; }

        protected int uid { get; private set; }

        private MemoryCache memCache = MemoryCache.Default;

        public BaseCaching(/*string appid, int uid*/)
        {
            this.appid = "";
            this.uid = 0;
        }

        /// <summary>
        /// Lấy data thông qua memory cache
        /// </summary>
        /// <typeparam name="TInstance">Kiểu dữ liệu instance của đối tượng lấy data</typeparam>
        /// <typeparam name="TOutput">Kiểu dữ liệu return</typeparam>
        /// <param name="instance">Instance của đối tượng lấy data</param>
        /// <param name="methodName">Tên method gọi hàm lấy data</param>
        /// <param name="paramObj">Danh sách param theo đúng thứ tự của methodName</param>
        /// <param name="timeout">Thời gian timeout tính bằng giây</param>
        /// <returns></returns>
        public TOutput GetDataWithMemCache<TInstance, TOutput>(
            TInstance instance, string prefixKey, string methodName, object[] paramObj, CacheTimeout timeout)
            where TOutput : new()
        {
            TOutput ret = new TOutput();
            try
            {
                string key = StringUtil.GenerateCachingKey(typeof(TInstance).ToString(), prefixKey + "_" +methodName, paramObj);
                var data = memCache.Get(key);
                if (data != null)
                {
                    ret = (TOutput)(data);
                }
                else
                {
                    var result = typeof(TInstance).GetMethod(methodName).Invoke(instance, paramObj);
                    if (result != null)
                    {
                        data = ret = (TOutput)result;
                        memCache.Set(key, data, DateTimeOffset.Now.AddSeconds((int)timeout));
                    }
                }
            }
            catch (VGException ex)
            {
                ex.AddSource(String.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name), ex.ErrorStackTrace);
                throw ex;
            }
            catch (Exception ex)
            {
                throw new VGException(ErrorSeverity.Error,
                    ErrorCode.CouldNotReadMemCacheData,
                    string.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name),
                    ex.Message,
                    ex.StackTrace);
            }

            return ret;
        }

        public void RemoveKeyStartWith<TInstance>(string prefix)
        {
            try
            {
                string prefixKey = string.Concat(typeof(TInstance).ToString(), "_", prefix);
                var keyCaches = memCache.Where(c => c.Key.StartsWith(prefixKey)).Select(c => c.Key);
                foreach (string key in keyCaches)
                {
                    try
                    {
                        memCache.Remove(key);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// cap nhat gia tri key khi thay doi
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeOut"></param>
        public TOutput UpdateDataWithCaching<TInstance, TOutput>(TInstance instance, string prefixKey, string methodName, object[] paramObj, CacheTimeout timeOut) where TOutput : new()
        {

            TOutput result = new TOutput();
            string key = string.Concat(typeof(TInstance).ToString(), prefixKey);
            try
            {
                var data = typeof(TInstance).GetMethod(methodName).Invoke(instance, paramObj);
                if (data != null)
                {
                    result = (TOutput)data;
                    memCache.Remove(key);
                    memCache.Set(key, data, DateTimeOffset.Now.AddSeconds((int)timeOut));
                }
                return result;
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public Boolean ClearCachingAll()
        {
            try
            {
                List<string> cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
                foreach (string cacheKey in cacheKeys)
                {
                    MemoryCache.Default.Remove(cacheKey);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public Boolean ClearCachingContainKey(string withKey)
        {
            try
            {
                List<string> cacheKeys = MemoryCache.Default.Where(x=>x.Key.Contains(withKey)).Select(kvp => kvp.Key).ToList();
                foreach (string cacheKey in cacheKeys)
                {
                    MemoryCache.Default.Remove(cacheKey);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        /// <summary>
        /// Thực thi hàm xử lý tầng cache
        /// </summary>
        /// <typeparam name="TOut">Kiểu dữ liệu trả về</typeparam>
        /// <param name="exeCode">Dynamic function thực thi xử lý</param>
        /// <returns></returns>
        protected TOut ProcessCache<TOut>(Func<TOut> exeCode, string typeName, string methodName)
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
                throw new VGException(ErrorSeverity.Error, ErrorCode.CouldNotExeCacheLayer,
                    string.Format(ConstValue.ErrorSourceFormat, typeName, methodName),
                    ex.Message, ex.StackTrace);
            }
        }

        public T GetValue2<T>(string key)
        {

            try
            {
                return (T)memCache.Get(string.Format("{0}", key));
            }
            catch
            {
                return default(T);
            }
        }

        public void Add2(string key, object value,int iHourExpires=8)
        {
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(iHourExpires);
            //cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            if (memCache != null && memCache.Contains(key))
            {
                memCache.Set(key, value, cacheItemPolicy);
            }
            else
            {
                memCache.Add(key, value, cacheItemPolicy);
            }
        }

        

        #region Redis

        /// <summary>
        /// Get data from cache server by cache name
        /// </summary>
        /// <returns></returns>
        public T GetDataWithCaching<T>(string CacheKey) where T : new()
        {
            try
            {
                string key = cacheNamePrefix + CacheKey;
                byte[] byteData = RedisClient.StringGet(key);
                if (byteData != null)
                {
                    return RedisClient.ProtoBufDeserialize<T>(byteData);
                }
            }
            catch (VGException ex)
            {
                ex.AddSource(String.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name), ex.ErrorStackTrace);
                throw ex;
            }
            catch (Exception ex)
            {
                throw new VGException(ErrorSeverity.Error, ErrorCode.CouldNotExeCacheLayer,
                    string.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name),
                    ex.Message, ex.StackTrace);
            }

            return default(T);
        }

        /// <summary>
        /// Get data from cache server by cache name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CacheKey"></param>
        /// <param name="InvokeMethod"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public T GetDataWithCaching<T>(string CacheKey, Func<T> InvokeMethod, long timeout) where T : new()
        {
            T ret = new T();
            try
            {
                string key = this.cacheNamePrefix + CacheKey;
                byte[] byteData = RedisClient.StringGet(key);
                if (byteData != null && byteData.Length > 0)
                {
                    ret = RedisClient.ProtoBufDeserialize<T>(byteData);
                }
                else
                {
                    ret = InvokeMethod();
                    if (ret != null)
                    {
                        byteData = RedisClient.ProtoBufSerialize(ret);
                        RedisClient.StringSet(key, byteData, timeout);
                    }
                }
            }
            catch (VGException ex)
            {
                ex.AddSource(String.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name), ex.ErrorStackTrace);
                throw ex;
            }
            catch (Exception ex)
            {
                throw new VGException(ErrorSeverity.Error,
                    ErrorCode.CouldNotReadRedisData,
                    string.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name),
                    ex.Message,
                    ex.StackTrace);
            }

            return ret;
        }

        /// <summary>
        /// Xóa cache bằng khóa
        /// </summary>
        /// <param name="key">min key length 23</param>
        /// <param name="dbid"></param>
        public void RemoveCacheByKey(string key, int dbid = -1)
        {
            try
            {
                dbid = (dbid == -1 ? BaseCaching.dbId : dbid);

                if (key.Length > 22)
                {
                    RedisClient.DeleteKeys(dbid, key);
                }
            }
            catch (VGException ex)
            {
                ex.AddSource(String.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name), ex.ErrorStackTrace);
                throw ex;
            }
            catch (Exception ex)
            {
                throw new VGException(ErrorSeverity.Error,
                    ErrorCode.CouldNotExeCacheLayer,
                    string.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name),
                    ex.Message,
                    ex.StackTrace);
            }
        }

        /// <summary>
        /// Xóa cache bằng list regex
        /// </summary>
        /// <param name="pattern">min pattern length 23</param>
        /// <param name="dbid"></param>
        public void RemoveCacheByPattern(List<string> patterns, int dbid = 0)
        {
            try
            {
                foreach (var pattern in patterns)
                {
                    if (pattern.Length > 10)
                    {
                        RedisClient.DeleteKeys(dbid, pattern.Trim());
                    }
                }
            }
            catch (VGException ex)
            {
                ex.AddSource(String.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name), ex.ErrorStackTrace);
                throw ex;
            }
            catch (Exception ex)
            {
                throw new VGException(ErrorSeverity.Error,
                    ErrorCode.CouldNotExecuteQuery,
                    string.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name),
                    ex.Message,
                    ex.StackTrace);
            }
        }

        /// <summary>
        /// Get data from cache server by cache name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramObj"></param>
        /// <param name="CacheKey"></param>
        /// <param name="InvokeMethod"></param>
        /// <returns></returns>
        public T GetDataWithCaching<T>(object[] paramObj, string CacheKey, Func<object[], T> InvokeMethod) where T : new()
        {
            T ret = new T();
            try
            {
                string key = StringUtil.GenerateCachingKey(paramObj, this.cacheNamePrefix + CacheKey);
                byte[] byteData = RedisClient.StringGet(key);
                if (byteData != null && byteData.Length > 0)
                {
                    ret = RedisClient.ProtoBufDeserialize<T>(byteData);
                }
                else
                {
                    ret = InvokeMethod(paramObj);
                    if (ret != null)
                    {
                        byteData = RedisClient.ProtoBufSerialize(ret);
                        RedisClient.StringSet(key, byteData, RedisClient.ExpiresTime);
                    }
                }
            }
            catch (VGException ex)
            {
                ex.AddSource(String.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name), ex.ErrorStackTrace);
                throw ex;
            }
            catch (Exception ex)
            {
                throw new VGException(ErrorSeverity.Error,
                    ErrorCode.CouldNotReadRedisData,
                    string.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name),
                    ex.Message,
                    ex.StackTrace);
            }

            return ret;
        }

        /// <summary>
        /// Get data from cache server by cache name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramObj"></param>
        /// <param name="CacheKey"></param>
        /// <param name="InvokeMethod"></param>
        /// <param name="timeout">Expire in second(s)</param>
        /// <returns></returns>
        public T GetDataWithCaching<T>(object[] paramObj, string CacheKey, Func<object[], T> InvokeMethod, long timeout) where T : new()
        {
            T ret = new T();
            try
            {
                string key = StringUtil.GenerateCachingKey(paramObj, this.cacheNamePrefix + CacheKey);
                byte[] byteData = RedisClient.StringGet(key);
                if (byteData != null && byteData.Length > 0)
                {
                    ret = RedisClient.ProtoBufDeserialize<T>(byteData);
                }
                else
                {
                    ret = InvokeMethod(paramObj);
                    if (ret != null)
                    {
                        byteData = RedisClient.ProtoBufSerialize(ret);
                        RedisClient.StringSet(key, byteData, timeout);
                    }
                }
            }
            catch (VGException ex)
            {
                ex.AddSource(String.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name), ex.ErrorStackTrace);
                throw ex;
            }
            catch (Exception ex)
            {
                throw new VGException(ErrorSeverity.Error,
                    ErrorCode.CouldNotReadRedisData,
                    string.Format(ConstValue.ErrorSourceFormat, GetType().Name, MethodBase.GetCurrentMethod().Name),
                    ex.Message,
                    ex.StackTrace);
            }

            return ret;
        }

        #endregion Redis
    }

    public enum CacheTimeout
    {
        /// <summary>
        /// 300 seconds
        /// </summary>
        Medium = 300 
    }
}