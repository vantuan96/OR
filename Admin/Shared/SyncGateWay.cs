using Contract.OR.SyncData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using Caching.Core;
using Contract.Core;
using VG.EncryptLib.Logging;
using Contract.Shared;

namespace Admin.Shared
{
    //public interface ISyncGateWay
    //{
    //    #region Anesthesia report
    //    BenhNhanOR GetBenhNhanORInfo(string pId, int sourceClientId);
    //    Examination GetPatientVisitORInfo(string pId, string visitCode, int sourceClientId);
    //    #endregion

    //}
    //public class SyncGateWay : ISyncGateWay
    //{
    //    #region private  function
    //    private static T Execute<T>(string url, object objConvert) where T : new()
    //    {
    //        try
    //        {
    //            string jsonContent = objConvert != null ? Newtonsoft.Json.JsonConvert.SerializeObject(objConvert) : string.Empty;
    //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    //            request.Method = "GET";
    //            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
    //            Byte[] byteArray = encoding.GetBytes(jsonContent);
    //            request.PreAuthenticate = true;
    //            request.Headers.Add("Authorization", "Bearer " + ConfigUrl.Authen_BearAPI);
    //            request.Accept = "application/json";
    //            request.ContentLength = byteArray.Length;
    //            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
    //            var response = request.GetResponse() as HttpWebResponse;
    //            using (var streamReader = new StreamReader(response.GetResponseStream()))
    //            {
    //                var responseText = streamReader.ReadToEnd();
    //                try
    //                {
    //                    try
    //                    {
    //                        (new LogCaching()).InsertLog(responseText, SystemLogType.SyncData, SourceType.ApiTool);
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        // ghi ra file xml
    //                        string exMessage = "Could not call api insert runtime log\r\n:" + ex.Message;
    //                        (new LoggingUtil()).WriteExceptionToXmlFile(new Exception(exMessage, ex.InnerException), LogSource.HttpClientAPI);
    //                    }
    //                }
    //                catch (Exception) { }
    //                var x = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText);


    //                return x;
    //            }                
    //        }
    //        catch (WebException ex)
    //        {
    //            (new LogCaching()).InsertLog(ex.Message, SystemLogType.Error, SourceType.ApiTool);
    //        }
    //        return default(T);
    //    }



    //    #endregion

    //    #region Anesthesia Report
    //    public BenhNhanOR GetBenhNhanORInfo(string pId, int sourceClientId)
    //    {
    //        try
    //        {
    //            var val = Execute<BenhNhanORSync>(ConfigUrl.GetUrlListVisitOR(pId, sourceClientId), null);
    //            #region "test"
    //            #endregion
    //            if (val == null || val.DSBenhNhan == null || val.DSBenhNhan.BenhNhan == null) return null;


    //            if (val.DSBenhNhan.BenhNhan.VisitSyncs != null && val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync != null && val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.Any())
    //            {
    //                var defaultItem = val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.FirstOrDefault(c => c.VISIT_CODE.Equals("0"));
    //                if (defaultItem != null)
    //                {
    //                    val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.Remove(defaultItem);
    //                    defaultItem = val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.FirstOrDefault(c => c.MA_BN.Equals("0"));
    //                    if (defaultItem != null)
    //                    {
    //                        val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.Remove(defaultItem);
    //                    }
    //                }
    //                val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync = val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.OrderByDescending(c => c.NGAY_VAO).ToList();
    //            }
    //            return val.DSBenhNhan.BenhNhan;
    //        }
    //        catch (Exception ex)
    //        {
    //            (new LogCaching()).InsertLog(ex.Message, SystemLogType.Error, SourceType.ApiTool);
    //            return null;
    //        }
    //    }

    //    public Examination GetPatientVisitORInfo(string pId, string visitCode, int sourceClientId)
    //    {
    //        try
    //        {
    //            var valSync = Execute<ExaminationSync>(ConfigUrl.GetPatientORInfo(pId, visitCode, sourceClientId), null);
    //            var val = valSync.ThongTin;
    //            if (val == null) return default(Examination);
    //            return valSync.ThongTin;
    //        }
    //        catch (Exception ex)
    //        {
    //            (new LogCaching()).InsertLog(ex.Message, SystemLogType.Error, SourceType.ApiTool);
    //            return null;
    //        }
    //    }
    //    #endregion
    //}
}
