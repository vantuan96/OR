using ProtoBuf;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace Caching
{
    public class RedisCurrentConnection
    {
        public ConnectionMultiplexer Connection { get; set; }

        public int Index { get; set; }
    }

    #region Consumer client

    [ProtoContract]
    public class ConsumerRedisStore
    {
        [ProtoMember(1)]
        public string ConsumerID { get; set; }

        [ProtoMember(2)]
        public string AppSecret { get; set; }

        [ProtoMember(3)]
        public string AppPrivateXML { get; set; }

        [ProtoMember(4)]
        public string AppPrivatePEM { get; set; }

        [ProtoMember(5)]
        public bool Active { get; set; }
    }

    /// <summary>
    /// Must define configuration keys: Redis.ConsumerHostSlaver, Redis.ConsumerPortSlaver, Redis.ConsummerRedisDbId
    /// </summary>
    public sealed class RedisReadConsumerClient
    {
        private static readonly string ServerIp = ConfigurationManager.AppSettings["Redis.ConsumerHostSlaver"];
        private static readonly int ServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["Redis.ConsumerPortSlaver"]);
        private static int DbId = 0;
        private const int IoTimeOut = 50000;
        private const int SyncTimeout = 50000;

        private static SocketManager _socketManager;
        private ConnectionMultiplexer _connection;
        private static volatile RedisReadConsumerClient _instance;

        public static readonly object SyncLock = new object();
        public static readonly object SyncConnectionLock = new object();

        public static RedisReadConsumerClient Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RedisReadConsumerClient();
                        }
                    }
                }

                return _instance;
            }
        }

        private RedisReadConsumerClient()
        {
            _socketManager = new SocketManager(GetType().Name);
            _connection = GetNewConnection();
        }

        private static ConnectionMultiplexer GetNewConnection()
        {
            var config = ConfigurationOptions.Parse(ServerIp + ":" + ServerPort);
            config.KeepAlive = 5;
            config.SyncTimeout = SyncTimeout;
            config.AbortOnConnectFail = false;
            config.AllowAdmin = true;
            config.ConnectTimeout = IoTimeOut;
            config.SocketManager = _socketManager;
            //var connection = ConnectionMultiplexer.Connect(config/*, logger*/);
            var connection = ConnectionMultiplexer.ConnectAsync(config);
            var muxer = connection.Result;
            return muxer;
        }

        public ConnectionMultiplexer GetConnection
        {
            get
            {
                lock (SyncConnectionLock)
                {
                    if (_connection == null)
                        _connection = GetNewConnection();
                    if (!_connection.IsConnected)
                        _connection = GetNewConnection();

                    if (_connection.IsConnected)
                        return _connection;
                    return _connection;
                }
            }
        }

        public static IDatabase CurrentConnection
        {
            get
            {
                if (ConfigurationManager.AppSettings["Redis.ConsummerRedisDbId"] != null)
                {
                    DbId = int.Parse(ConfigurationManager.AppSettings["Redis.ConsummerRedisDbId"].ToString());
                }

                var connection = RedisReadConsumerClient.Current.GetConnection.GetDatabase(DbId);
                return connection;
            }
        }
    }

    public sealed class RedisLogerClient
    {
        private static string ServerIp = "10.220.61.103:6377";
        private static int Port = 6602;
        private const int IoTimeOut = 50000;
        private const int SyncTimeout = 50000;

        private static SocketManager _socketManager;
        private ConnectionMultiplexer _connection;
        private static volatile RedisLogerClient _instance;

        public static readonly object SyncLock = new object();
        public static readonly object SyncConnectionLock = new object();

        public static RedisLogerClient Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RedisLogerClient();
                        }
                    }
                }

                return _instance;
            }
        }

        private RedisLogerClient()
        {
            _socketManager = new SocketManager(GetType().Name);
            _connection = GetNewConnection();
        }

        private static ConnectionMultiplexer GetNewConnection()
        {
            var config = ConfigurationOptions.Parse(ServerIp);
            config.KeepAlive = 5;
            config.SyncTimeout = SyncTimeout;
            config.AbortOnConnectFail = false;
            config.AllowAdmin = true;
            config.ConnectTimeout = IoTimeOut;
            config.SocketManager = _socketManager;
            //var connection = ConnectionMultiplexer.Connect(config/*, logger*/);
            var connection = ConnectionMultiplexer.ConnectAsync(config);
            var muxer = connection.Result;
            return muxer;
        }

        public ConnectionMultiplexer GetConnection
        {
            get
            {
                lock (SyncConnectionLock)
                {
                    if (_connection == null)
                        _connection = GetNewConnection();
                    if (!_connection.IsConnected)
                        _connection = GetNewConnection();

                    if (_connection.IsConnected)
                        return _connection;
                    return _connection;
                }
            }
        }
    }

    #endregion Consumer client

    #region Internal client

    /// <summary>
    /// Must define configuration keys: AcquiredTimeout, Redis.HostMaster, Redis.PortMaster
    /// </summary>
    public sealed class RedisWriteClient
    {
        private static readonly double AcquiredTimeout = double.Parse(ConfigurationManager.AppSettings["Redis.AcquiredTimeout"] ?? "10");
        private static readonly string lockPrefix = "adr:lock:";
        private static string shaLuaLock = GetKeySHA();
        private static readonly string ServerIp = ConfigurationManager.AppSettings["Redis.HostMaster"];
        private static readonly int ServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["Redis.PortMaster"]);
        private const int IoTimeOut = 50000;
        private const int SyncTimeout = 50000;
        private static readonly object Log4 = new object();
        private const string shaLuaGroupLock = "";
        private static SocketManager _socketManager;
        private ConnectionMultiplexer _connection;
        private static volatile RedisWriteClient _instance;

        public static readonly object SyncLock = new object();
        public static readonly object SyncConnectionLock = new object();

        public static RedisWriteClient Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RedisWriteClient();
                        }
                    }
                }

                return _instance;
            }
        }

        private RedisWriteClient()
        {
            _socketManager = new SocketManager(GetType().Name);
            _connection = GetNewConnection();
        }

        private static ConnectionMultiplexer GetNewConnection()
        {
            var config = ConfigurationOptions.Parse(ServerIp + ":" + ServerPort);
            config.KeepAlive = 5;
            config.SyncTimeout = SyncTimeout;
            config.AbortOnConnectFail = false;
            config.AllowAdmin = true;
            config.ConnectTimeout = IoTimeOut;
            config.SocketManager = _socketManager;
            //var connection = ConnectionMultiplexer.Connect(config/*, logger*/);
            var connection = ConnectionMultiplexer.ConnectAsync(config);
            var muxer = connection.Result;
            return muxer;
        }

        public ConnectionMultiplexer GetConnection
        {
            get
            {
                lock (SyncConnectionLock)
                {
                    if (_connection == null)
                        _connection = GetNewConnection();
                    if (!_connection.IsConnected)
                        _connection = GetNewConnection();

                    if (_connection.IsConnected)
                        return _connection;
                    return _connection;
                }
            }
        }

        public static IDatabase CurrentConnection
        {
            get
            {
                var connection = RedisWriteClient.Current.GetConnection.GetDatabase(2);
                return connection;
            }
        }

        public static bool Lock(string key, string uuid)
        {
            //var lockKey = lockPrefix + key;
            //var acquiredTimeOut = TimeSpan.FromSeconds(AcquiredTimeout);
            //RedisValue token = Environment.MachineName + uuid;
            //bool result = false;
            //while (!result)
            //{
            //    result = CurrentConnection.LockTake(lockKey, token, acquiredTimeOut);
            //    if (!result)
            //    {
            //        Thread.Sleep(100);
            //    }
            //}
            string[] arrKey = new string[1] { key };
            return LockGroup(arrKey, uuid);
        }

        public static bool LockTryOneTime(string key, string uuid)
        {
            GetKeySHA();
            RedisValue[] values = new RedisValue[4];
            values[0] = (RedisValue)"1";
            values[1] = (RedisValue)(Environment.MachineName + uuid);
            values[2] = (RedisValue)AcquiredTimeout;
            values[3] = (RedisValue)(lockPrefix + key);
            int result = (int)CurrentConnection.ScriptEvaluate(shaLuaLock, null, values);
            return result != 0;
        }

        public static bool UnLock(string key, string uuid)
        {
            //var lockKey = lockPrefix + key;
            //RedisValue token = Environment.MachineName + uuid;
            //return CurrentConnection.LockRelease(lockKey, token);
            string[] arrKey = new string[1] { key };
            return UnLockGroup(arrKey, uuid);
        }

        public static bool LockGroup(string[] keys, string uuid)
        {
            if (keys == null || keys.Length == 0)
            {
                return false;
            }
            GetKeySHA();
            RedisValue[] values = new RedisValue[keys.Length + 3];
            values[0] = (RedisValue)"1";
            values[1] = (RedisValue)(Environment.MachineName + uuid);
            values[2] = (RedisValue)AcquiredTimeout;
            for (int i = 0; i < keys.Length; i++)
            {
                values[i + 3] = (RedisValue)(lockPrefix + keys[i]);
            }
            int result = 0;
            do
            {
                result = (int)CurrentConnection.ScriptEvaluate(shaLuaLock, null, values);
                if (result == 0)
                {
                    Thread.Sleep(100);
                }
            } while (result == 0);
            return true;
        }

        public static bool UnLockGroup(string[] keys, string uuid)
        {
            if (keys == null || keys.Length == 0)
            {
                return false;
            }
            GetKeySHA();
            RedisValue[] values = new RedisValue[keys.Length + 2];
            values[0] = (RedisValue)"0";
            values[1] = (RedisValue)(Environment.MachineName + uuid);
            for (int i = 0; i < keys.Length; i++)
            {
                values[i + 2] = (RedisValue)(lockPrefix + keys[i]);
            }
            CurrentConnection.ScriptEvaluate(shaLuaLock, null, values);
            return true;
        }

        private static string GetKeySHA()
        {
            shaLuaLock = RedisReadClient.CurrentConnection.StringGet("adr:getlock:luasha");
            return shaLuaLock;
        }

        public static void DeleteKeys(int dbid, string _pattern)
        {
            var server = RedisWriteClient.Current.GetConnection.GetServer(ServerIp + ":" + ServerPort);
            var redisClient = RedisWriteClient.Current.GetConnection.GetDatabase(dbid);
            lock (Log4)
            {
                var listKey = server.Keys(dbid, _pattern);
                var redisKeys = listKey as RedisKey[] ?? listKey.ToArray();
                if (listKey != null && redisKeys.Any())
                {
                    foreach (var key in redisKeys)
                    {
                        redisClient.KeyDelete(key);
                    }
                }
            }
        }

        public static List<string> GetKeys(int dbid, string _pattern)
        {
            var server = RedisWriteClient.Current.GetConnection.GetServer(ServerIp + ":" + ServerPort);
            var redisClient = RedisWriteClient.Current.GetConnection.GetDatabase(dbid);
            lock (Log4)
            {
                var listKey = server.Keys(dbid, _pattern);
                var redisKeys = listKey as RedisKey[] ?? listKey.ToArray();
                if (redisKeys.Count() > 0)
                {
                    return redisKeys.Select(x => x.ToString()).ToList();
                }
                return null;
            }
        }
    }

    /// <summary>
    /// Must define configuration keys: Redis.HostSlaver, Redis.PortSlaver
    /// </summary>
    public sealed class RedisReadClient
    {
        public static readonly string ServerIp = ConfigurationManager.AppSettings["Redis.HostSlaver"];
        private static readonly int ServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["Redis.PortSlaver"]);
        private const int IoTimeOut = 50000;
        private const int SyncTimeout = 50000;

        private static SocketManager _socketManager;
        private ConnectionMultiplexer _connection;
        private static volatile RedisReadClient _instance;

        public static readonly object SyncLock = new object();
        public static readonly object SyncConnectionLock = new object();

        public static RedisReadClient Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RedisReadClient();
                        }
                    }
                }

                return _instance;
            }
        }

        private RedisReadClient()
        {
            _socketManager = new SocketManager(GetType().Name);
            _connection = GetNewConnection();
        }

        private static ConnectionMultiplexer GetNewConnection()
        {
            var config = ConfigurationOptions.Parse(ServerIp + ":" + ServerPort);
            config.KeepAlive = 5;
            config.SyncTimeout = SyncTimeout;
            config.AbortOnConnectFail = false;
            config.AllowAdmin = true;
            config.ConnectTimeout = IoTimeOut;
            config.SocketManager = _socketManager;
            //var connection = ConnectionMultiplexer.Connect(config/*, logger*/);
            var connection = ConnectionMultiplexer.ConnectAsync(config);
            var muxer = connection.Result;
            return muxer;
        }

        public ConnectionMultiplexer GetConnection
        {
            get
            {
                lock (SyncConnectionLock)
                {
                    if (_connection == null)
                        _connection = GetNewConnection();
                    if (!_connection.IsConnected)
                        _connection = GetNewConnection();

                    if (_connection.IsConnected)
                        return _connection;
                    return _connection;
                }
            }
        }

        public static IDatabase CurrentConnection
        {
            get
            {
                var connection = RedisReadClient.Current.GetConnection.GetDatabase(2);
                return connection;
            }
        }

        public static IDatabase CurrentConnectionLua
        {
            get
            {
                var connection = RedisReadClient.Current.GetConnection.GetDatabase(8);
                return connection;
            }
        }
    }

    /// <summary>
    /// Must define configuration keys: Redis.DefaultRedisDbId, Redis.MinExpires, Redis.HostSlaver, Redis.PortSlaver, 
    /// RedisConsumerHostSlaver, RedisConsumerPortSlaver, ADR.ConsummerRedisDbId
    /// </summary>
    public class RedisClient
    {
        public static int DbId = Convert.ToInt32(ConfigurationManager.AppSettings["Redis.DefaultRedisDbId"]);
        public static long ExpiresTime = Convert.ToInt32(ConfigurationManager.AppSettings["Redis.MinExpires"]);
        public static object Log1 = new object();
        public static object Log2 = new object();
        public static object Log3 = new object();
        public static object LogSetConnection = new object();
        public static object LogGetConnection = new object();
        public static object LogConsumerGetConnection = new object();

        public static byte[] ProtoBufSerialize(Object item)
        {
            lock (Log1)
            {
                if (item != null)
                {
                    try
                    {
                        var ms = new MemoryStream();
                        Serializer.Serialize(ms, item);
                        var rt = ms.ToArray();
                        return rt;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unable to serialize object", ex);
                    }
                }
                else
                {
                    throw new Exception("Object serialize is null");
                }
            }
        }

        public static T ProtoBufDeserialize<T>(byte[] byteArray)
        {
            lock (Log2)
            {
                if (byteArray != null && byteArray.Length > 0)
                {
                    try
                    {
                        var ms = new MemoryStream(byteArray);
                        return Serializer.Deserialize<T>(ms);
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("Unable to deserialize object", ex);
                        return default(T);
                    }
                }
                else
                {
                    //throw new Exception("Object Deserialize is null or empty");
                    return default(T);
                }
            }
        }

        public static List<T> ByteArrayToListObject<T>(byte[][] multiDataList)
        {
            lock (Log3)
            {
                if (multiDataList == null)
                    return new List<T>();

                var results = new List<T>();
                foreach (var multiData in multiDataList)
                {
                    results.Add(ProtoBufDeserialize<T>(multiData));
                }
                return results;
            }
        }

        //#region Redis Get

        public static byte[] GetConsumerInfo(string key)
        {
            var redisClient = RedisReadConsumerClient.Current.GetConnection;
            lock (LogConsumerGetConnection)
            {
                var byteData = redisClient.GetDatabase(DbId).StringGet(key);// redisClient.Connection.Strings.Get(DbId, key).Result;
                return byteData;
            }
        }

        public static byte[] StringGet(string key, int dbid = -1)
        {
            if (dbid == -1) dbid = DbId;

            var redisClient = RedisReadClient.Current.GetConnection;
            lock (LogGetConnection)
            {
                var byteData = redisClient.GetDatabase(dbid).StringGet(key);
                return byteData;
            }
        }

        public static void StringSet(string key, long value, int dbid = -1)
        {
            if (dbid == -1) dbid = DbId;

            var redisClient = RedisWriteClient.Current.GetConnection;
            lock (LogSetConnection)
            {
                redisClient.GetDatabase(dbid).StringSet(key, value);
            }
        }

        public static void StringSet(string key, string value, int dbid = -1)
        {
            if (dbid == -1) dbid = DbId;

            var redisClient = RedisWriteClient.Current.GetConnection;
            lock (LogSetConnection)
            {
                redisClient.GetDatabase(dbid).StringSet(key, value);
            }
        }

        public static void StringSet(string key, byte[] data, int dbid = -1)
        {
            if (dbid == -1) dbid = DbId;

            var redisClient = RedisWriteClient.Current.GetConnection;
            lock (LogSetConnection)
            {
                redisClient.GetDatabase(dbid).StringSet(key, data);
            }
        }

        public static void StringSet(string key, byte[] data, long expirySeconds, int dbid = -1)
        {
            if (dbid == -1) dbid = DbId;

            var redisClient = RedisWriteClient.Current.GetConnection;
            lock (LogSetConnection)
            {
                var expire = DateTime.Now.AddSeconds(expirySeconds);
                redisClient.GetDatabase(dbid).StringSet(key, data, expire.Subtract(DateTime.Now));
            }
        }

        public static void ListRightPush(string key, byte[] data, int dbid = -1)
        {
            if (dbid == -1) dbid = DbId;

            var redisClient = RedisWriteClient.Current.GetConnection;
            lock (LogSetConnection)
            {
                redisClient.GetDatabase(dbid).ListRightPush(key, data);
            }
        }

        public static byte[][] ListRange(string key, long start, long stop, int dbid = -1)
        {
            if (dbid == -1) dbid = DbId;

            var redisClient = RedisReadClient.Current.GetConnection;
            lock (LogGetConnection)
            {
                var byteData = redisClient.GetDatabase(dbid).ListRange(key, start, stop);
                return byteData.Select(r => (byte[])r).ToArray();
            }
        }

        public static long ListLength(string key, int dbid = -1)
        {
            if (dbid == -1) dbid = DbId;

            var redisClient = RedisReadClient.Current.GetConnection;
            lock (LogGetConnection)
            {
                var length = redisClient.GetDatabase(dbid).ListLength(key);
                return length;
            }
        }

        public static byte[] ListLeftPop(string key, int dbid = -1)
        {
            if (dbid == -1) dbid = DbId;

            var redisClient = RedisReadClient.Current.GetConnection;
            lock (LogGetConnection)
            {
                return redisClient.GetDatabase(dbid).ListLeftPop(key);
            }
        }

        /// <summary>
        /// Delete cache by regex pattern
        /// </summary>
        /// <param name="dbid"></param>
        /// <param name="_pattern"></param>
        public static void DeleteKeys(int dbid, string _pattern)
        {
            lock (LogSetConnection)
            {
                RedisWriteClient.DeleteKeys(dbid, _pattern);
            }
        }

        /// <summary>
        /// Delete cache by regex pattern in Default DbId (0)
        /// </summary>
        /// <param name="_pattern"></param>
        public static void DeleteKeys(string _pattern)
        {
            lock (LogSetConnection)
            {
                RedisWriteClient.DeleteKeys(DbId, _pattern);
            }
        }

        /// <summary>
        /// Delete cache by regex pattern in Default DbId (0)
        /// </summary>
        /// <param name="_pattern"></param>
        public static List<string> GetKeyByPattern(string _pattern, int dbid)
        {
            lock (LogSetConnection)
            {
                return RedisWriteClient.GetKeys(dbid, _pattern);
            }
        }

        //#endregion Redis Set

        /// <summary>
        /// Delete cache key
        /// </summary>
        /// <param name="dbid"></param>
        /// <param name="key"></param>
        public static void DeleteKey(int dbid, string key)
        {
            var redisClient = RedisWriteClient.Current.GetConnection;
            lock (LogSetConnection)
            {
                redisClient.GetDatabase(dbid).KeyDelete(key);
            }
        }

        /// <summary>
        /// Delete cache key in Default DbId (0)
        /// </summary>
        /// <param name="dbid"></param>
        /// <param name="key"></param>
        public static void DeleteKey(string key)
        {
            var redisClient = RedisWriteClient.Current.GetConnection;
            lock (LogSetConnection)
            {
                redisClient.GetDatabase(DbId).KeyDelete(key);
            }
        }

    }

    #endregion Internal client    
}
