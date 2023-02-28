using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace DataAccess.Shared
{
    public class BaseExec
    {
        private readonly Microsoft.Practices.EnterpriseLibrary.Data.Database _db;

        public BaseExec(string connectionString = "ConnString.PatientQueue")
        {
            if (ConfigurationManager.ConnectionStrings[connectionString] != null)
            {
                _db = new DatabaseProviderFactory().Create(connectionString);
            }
        }

        public Microsoft.Practices.EnterpriseLibrary.Data.Database Dbase
        {
            get { return _db; }
        }

        public IEnumerable<T> ExecStoredProc<T>(string storeName, object[] paramObj) where T : new()
        {
            try
            {
                return _db.CreateSprocAccessor(storeName, MapBuilder<T>.MapAllProperties().Build()).Execute(paramObj).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<T> ExecSqlString<T>(string sqlString, object[] paramObj) where T : new()
        {
            try
            {
                return _db.CreateSqlStringAccessor(sqlString, MapBuilder<T>.MapAllProperties().Build()).Execute(paramObj).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ExecStoredProc(string storeName, object[] paramObj)
        {
            try
            {
                return _db.ExecuteNonQuery(storeName, paramObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public T ExecScalar<T>(CommandType commandType, string commandText)
        {
            return (T)Convert.ChangeType(_db.ExecuteScalar(commandType, commandText), typeof(T));
        }
        public T ExecScalar<T>(string storedProcedureName, params object[] parameterValues)
        {
            return (T)Convert.ChangeType(_db.ExecuteScalar(storedProcedureName, parameterValues), typeof(T));
        }

        public IEnumerable<T> ExecStoredProc<T>(string storeName, object[] paramObj, IRowMapper<T> rowMapper) where T : new()
        {
            try
            {
                return _db.CreateSprocAccessor(storeName, rowMapper).Execute(paramObj).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecNonQuery(CommandType commandType, string commandText)
        {
            return _db.ExecuteNonQuery(commandType, commandText);
        }

        #region GenerateRowMapper

        /// <summary>
        /// Convert xml string data to list object T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRecord"></param>
        /// <param name="strMappedColumn"></param>
        /// <returns></returns>
        protected T XMLToObject<T>(IDataRecord dataRecord, string strMappedColumn) where T : new()
        {
            T ret = new T();
            try
            {
                string xmlString = dataRecord.GetString(dataRecord.GetOrdinal(strMappedColumn));
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                StringReader rdr = new StringReader(xmlString);
                ret = (T)serializer.Deserialize(rdr);
            }
            catch (Exception ex)
            {
            }
            return ret;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TReturn">Store return object</typeparam>
        /// <typeparam name="TChild">Store return child object</typeparam>
        /// <param name="strMappedColumn">Column name with xml as child object</param>
        /// <param name="childType">Child object type</param>
        /// <returns></returns>
        public IRowMapper<TReturn> GenerateRowMapper<TReturn, TChild>(string strMappedColumn, PropertyInfo childType)
            where TReturn : new()
            where TChild : new()
        {
            return MapBuilder<TReturn>.MapAllProperties()
                .Map(childType)
                .WithFunc(n => XmlToObject<TChild>(n, strMappedColumn))
                .Build();
        }

        protected T XmlToObject<T>(IDataRecord dataRecord, string strMappedColumn) where T : new()
        {
            try
            {
                int indexColumn = dataRecord.GetOrdinal(strMappedColumn);
                if (dataRecord.IsDBNull(indexColumn))
                    return new T();

                var xmlString = dataRecord.GetString(indexColumn);
                var serializer = new XmlSerializer(typeof(T));
                var rdr = new StringReader(xmlString);
                return (T)serializer.Deserialize(rdr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Generate row mapper for 2 columns
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TChild1">First mapped type of TReturn property</typeparam>
        /// <typeparam name="TChild2">Second mapped type of TReturn property</typeparam>
        /// <param name="strMappedColumn1">First type mapping column</param>
        /// <param name="childType1">First TReturn mapping property</param>
        /// <param name="strMappedColumn2">Second type mapping column</param>
        /// <param name="childType2">Second TReturn mapping property</param>
        /// <returns></returns>
        protected IRowMapper<TReturn> GenerateRowMapper<TReturn, TChild1, TChild2>
            (string strMappedColumn1, PropertyInfo childType1,
            string strMappedColumn2, PropertyInfo childType2)
            where TReturn : new()
            where TChild1 : new()
            where TChild2 : new()
        {
            try
            {
                return MapBuilder<TReturn>.MapAllProperties()
                    .Map(childType1)
                    .WithFunc(n => XMLToObject<TChild1>(n, strMappedColumn1))
                    .Map(childType2)
                    .WithFunc(n => XMLToObject<TChild2>(n, strMappedColumn2))
                    .Build();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetPropertyInfo

        public PropertyInfo GetPropertyInfo<T>(string fieldName)
        {
            return typeof(T).GetProperty(fieldName);
        }

        /// <summary>
        /// Get property info of any type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lambda"></param>
        /// <returns></returns>
        protected PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> lambda)
        {
            var member = lambda.Body as MemberExpression;
            return member.Member as PropertyInfo;
        }

        #endregion GetPropertyInfo

        #region TestConnection
        /// <summary>
        /// Kiểm tra có kết nối được đến Database hay không
        /// </summary>
        /// <returns></returns>
        public bool TestConnection()
        {
            try
            {
                using (var conn = _db.CreateConnection())
                {
                    conn.Open(); // throws if invalid
                    conn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        protected string ObjectToXml<T>(List<T> obj) //where T : new()
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<T>));
            System.IO.StringWriter sww = new System.IO.StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, obj);
            var xml = sww.ToString(); // Your xml
            return xml;
        }
        protected IRowMapper<TReturn> GenerateRowMapperExt2Child<TReturn, TChild1, TChild2>
               (string strMappedColumn1, PropertyInfo childType1,
                string strMappedColumn2, PropertyInfo childType2
                )
             where TReturn : new()
             where TChild1 : new()
             where TChild2 : new()
        {
            return MapBuilder<TReturn>.MapAllProperties()
                .Map(childType1)
                .WithFunc(n => XMLToObject<TChild1>(n, strMappedColumn1))
                .Map(childType2)
                .WithFunc(n => XMLToObject<TChild2>(n, strMappedColumn2))
                .Build();
        }
        protected IRowMapper<TReturn> GenerateRowMapperExt3Child<TReturn, TChild1, TChild2, TChild3>
              (string strMappedColumn1, PropertyInfo childType1,
               string strMappedColumn2, PropertyInfo childType2,
               string strMappedColumn3, PropertyInfo childType3
               )
            where TReturn : new()
            where TChild1 : new()
            where TChild2 : new()
            where TChild3 : new()
        {
            return MapBuilder<TReturn>.MapAllProperties()
                .Map(childType1)
                .WithFunc(n => XMLToObject<TChild1>(n, strMappedColumn1))
                .Map(childType2)
                .WithFunc(n => XMLToObject<TChild2>(n, strMappedColumn2))
                .Map(childType3)
                .WithFunc(n => XMLToObject<TChild3>(n, strMappedColumn3))
                .Build();
        }
    }
}
