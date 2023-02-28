using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace VG.Common
{
    public static class XmlUtil
    {

        /// <summary>
        /// Chuyển object thành chuỗi xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToXML<T>(T obj)// where T : new()
        {
            try
            {
                XmlSerializer xsSubmit = new XmlSerializer(typeof(T));

                XmlDocument doc = new XmlDocument();

                System.IO.StringWriter sww = new System.IO.StringWriter();
                XmlWriter writer = XmlWriter.Create(sww);
                xsSubmit.Serialize(writer, obj);
                var xml = sww.ToString(); // Your xml
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        /// <summary>
        /// Convert xml string data to list object T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRecord"></param>
        /// <param name="strMappedColumn"></param>
        /// <returns></returns>
        public static T XMLToObject<T>(IDataRecord dataRecord, string strMappedColumn) where T : new()
        {
            T ret = new T();
            try
            {
                string xmlString = dataRecord.GetString(dataRecord.GetOrdinal(strMappedColumn));
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                StringReader rdr = new StringReader(xmlString);
                ret = (T)serializer.Deserialize(rdr);
            }
            catch (Exception ex) { throw ex; }
            return ret;
        }



        /// <summary>
        /// Convert List T type to XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToXML<T>(List<T> obj) //where T : new()
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<T>));

            XmlDocument doc = new XmlDocument();

            System.IO.StringWriter sww = new System.IO.StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, obj);
            var xml = sww.ToString(); // Your xml
            return xml;
        }
    }
}
