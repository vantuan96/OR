using Business.Core;
using Contract.Core;
using Contract.OR.SyncData;
using Contract.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VG.Common;
using VG.EncryptLib.Logging;

namespace Business.API
{
    public interface ISyncGateWay
    {
        #region Anesthesia report
        BenhNhanOR GetBenhNhanORInfo(string pId, int sourceClientId);
        BenhNhanOR GetPatientInfo(string pId);
        Examination GetPatientVisitORInfo(string pId, string visitCode, int sourceClientId);
        List<HISChargeModel> GetCharges(string chargeids);
        string GetDiagnosisByCharge(string chargeId);
        int UpdateDimsToReCalculate(string chargeId);

        #endregion

    }
    public class SyncGateWay : ISyncGateWay
    {
        #region private  function
        private static T Execute<T>(string url, object objConvert) where T : new()
        {
            try
            {
                string jsonContent = objConvert != null ? Newtonsoft.Json.JsonConvert.SerializeObject(objConvert) : string.Empty;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(jsonContent);
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", "Bearer " + ConfigUrl.Authen_BearAPI);
                request.Accept = "application/json";
                request.ContentLength = byteArray.Length;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                var response = request.GetResponse() as HttpWebResponse;
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    try
                    {
                        try
                        {
                            (new LogBusiness(string.Empty, 0)).InsertLog(responseText, SystemLogType.SyncData, SourceType.ApiTool);
                        }
                        catch (Exception ex)
                        {
                            // ghi ra file xml
                            string exMessage = "Could not call api insert runtime log\r\n:" + ex.Message;
                            (new LoggingUtil()).WriteExceptionToXmlFile(new Exception(exMessage, ex.InnerException), LogSource.HttpClientAPI);
                        }
                    }
                    catch (Exception) { }
                    var x = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText);


                    return x;
                }
            }
            catch (WebException ex)
            {
                (new LogBusiness(string.Empty, 0)).InsertLog(ex.Message, SystemLogType.Error, SourceType.ApiTool);
            }
            return default(T);
        }
        protected static JToken RequestAPI(string url_postfix, string json_collection, string json_item, out bool isThrowEx, int mnTimeout = 3, string apiToken="")
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", !string.IsNullOrEmpty(apiToken)? apiToken : ConfigurationManager.AppSettings["OR_API_SERVER_TOKEN"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                string url = string.Format("{0}{1}", ConfigUrl.UrlSyncHis, url_postfix);
                string raw_data = string.Empty;
                try
                {
                    client.Timeout = TimeSpan.FromMinutes(mnTimeout);
                    var response = client.GetAsync(url);
                    raw_data = response.Result.Content.ReadAsStringAsync().Result;
                    //if (response.Result.StatusCode != HttpStatusCode.OK)
                    //    HandleError(url, raw_data);
                    //else
                    //    HandleSuccess(url);

                    JObject json_data = JObject.Parse(raw_data);
                    var log_response = json_data.ToString();
                    //CustomLog.apigwlog.Info(new
                    //{
                    //    URI = url,
                    //    Response = log_response,
                    //});
                    try
                    {
                        if (!string.IsNullOrEmpty(json_collection))
                        {
                            JToken customer_data = json_data[json_collection][json_item];
                            isThrowEx = false;
                            return customer_data;
                        }
                        else
                        {
                            JToken customer_data = json_data[json_item];
                            isThrowEx = false;
                            return customer_data;
                        }
                    }
                    catch
                    {
                        isThrowEx = true;
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    var log_response = string.Format("{0}\n{1}", ex.ToString(), raw_data);
                    //HandleError(url, log_response);
                    //CustomLog.apigwlog.Info(new
                    //{
                    //    URI = url,
                    //    Response = log_response,
                    //});
                    isThrowEx = true;
                    return null;
                }
            }
        }


        #endregion

        #region Anesthesia Report
        public BenhNhanOR GetBenhNhanORInfo(string pId, int sourceClientId)
        {
            try
            {
                var val = Execute<BenhNhanORSync>(ConfigUrl.GetUrlListVisitOR(pId, sourceClientId), null);
                #region "test"
                #endregion
                if (val == null || val.DSBenhNhan == null || val.DSBenhNhan.BenhNhan == null) return null;


                if (val.DSBenhNhan.BenhNhan.VisitSyncs != null && val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync != null && val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.Any())
                {
                    var defaultItem = val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.FirstOrDefault(c => c.VISIT_CODE.Equals("0"));
                    if (defaultItem != null)
                    {
                        val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.Remove(defaultItem);
                        defaultItem = val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.FirstOrDefault(c => c.MA_BN.Equals("0"));
                        if (defaultItem != null)
                        {
                            val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.Remove(defaultItem);
                        }
                    }
                    val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync = val.DSBenhNhan.BenhNhan.VisitSyncs.VisitSync.OrderByDescending(c => c.NGAY_VAO).ToList();
                }
                return val.DSBenhNhan.BenhNhan;
            }
            catch (Exception ex)
            {
                (new LogBusiness(string.Empty,0)).InsertLog(ex.Message, SystemLogType.Error, SourceType.ApiTool);
                return null;
            }
        }
        public BenhNhanOR GetPatientInfo(string pId)
        {
            try
            {
                BenhNhanOR val = null;
                var valDynamic = Execute<dynamic>(ConfigUrl.GetUrlPatientByPID(pId), null);
                if (valDynamic != null && valDynamic.BenhNhan!=null)
                {
                    DateTime? NgaySinh = null;
                    DateTime dob = DateTime.MinValue;
                    DateTime.TryParse(valDynamic.BenhNhan.NgaySinh.ToString(), out dob);
                    if (dob != DateTime.MinValue)
                        NgaySinh = dob;
                    val = new BenhNhanOR() { 
                        MA_BN=pId,
                        HO_TEN=valDynamic.BenhNhan.TenBenhNhan,
                        DIA_CHI = valDynamic.BenhNhan.DiaChi,
                        EMAIL=valDynamic.BenhNhan.Email,
                        //GIOI_TINH=new List<string>(){"M","T"}.Contains(valDynamic.BenhNhan.GioiTinh.ToString()) ?1:0,
                        //GIOI_TINH=Convert.ToInt32(valDynamic.BenhNhan.GIOI_TINH_OneVinmec.ToString()),
                        NGAY_SINH = NgaySinh,
                        PHONE=valDynamic.BenhNhan.SoDienThoai,
                        QUOC_TICH=valDynamic.BenhNhan.QuocTich
                    };
                }
                val.GIOI_TINH = val.GIOI_TINH == 0 ? 2 : val.GIOI_TINH;
                if (val.NGAY_SINH != null)
                    val.TUOI = val.NGAY_SINH.Value.GetAges().ToString();
                return val;
            }
            catch (Exception ex)
            {
                (new LogBusiness(string.Empty, 0)).InsertLog(ex.Message, SystemLogType.Error, SourceType.ApiTool);
                return null;
            }
        }
        public Examination GetPatientVisitORInfo(string pId, string visitCode, int sourceClientId)
        {
            try
            {
                var valSync = Execute<ExaminationSync>(ConfigUrl.GetPatientORInfo(pId, visitCode, sourceClientId), null);
                var val = valSync.ThongTin;
                if (val == null) return default(Examination);
                return valSync.ThongTin;
            }
            catch (Exception ex)
            {
                (new LogBusiness(string.Empty, 0)).InsertLog(ex.Message, SystemLogType.Error, SourceType.ApiTool);
                return null;
            }
        }
        #endregion
        #region Service
        /// <summary>
        /// Get List Service
        /// </summary>
        /// <param name="serviceCode"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static List<HISServiceModel> GetService(string serviceCode, string serviceName)
        {
            //#region For statistic performance
            //var start_time = DateTime.Now;
            //TimeSpan tp;
            //#endregion .For statistic performance

            string url_postfix = string.Format(
                "PMSVinmecCom/1.0.0/getServiceInformation?ServiceCode={0}&ServiceName={1}"
                , serviceCode
                , serviceName
            );
            bool bThrowEx = false;
            var response = RequestAPI(url_postfix, "Entries", "Entry", out bThrowEx, ConfigUrl.CF_ApiTimeout_minutes, ConfigurationManager.AppSettings["PMS_API_SERVER_TOKEN"]);

            //#region Log Performace
            //tp = DateTime.Now - start_time;
            //CustomLog.performancejoblog.Info(string.Format("API.GetService[serviceCode:{0}; ServiceName:{1}]: step processing spen time in {2} (ms)", serviceCode, serviceName, tp.TotalMilliseconds));
            //#endregion .Log Performace

            if (response != null)
                return response.Select(e => new HISServiceModel
                {
                    ServiceId = e["ServiceId"]?.ToObject<Guid?>(),
                    ServiceType = e["ServiceType"]?.ToString(),
                    ServiceGroupCode = e["ServiceGroupCode"]?.ToString(),
                    ServiceGroupViName = e["ServiceGroupName"]?.ToString(),
                    ServiceGroupEnName = e["ServiceGroupNameE"]?.ToString(),
                    ServiceCode = e["ServiceCode"]?.ToString(),
                    ServiceViName = e["ServiceName"]?.ToString(),
                    ServiceEnName = e["ServiceNameE"]?.ToString(),
                    IsActive = e["ActiveFlag"] != null ? Convert.ToBoolean(e["ActiveFlag"].ToString()) : false
                }).ToList();
            return new List<HISServiceModel>();
        }
        #endregion
        #region Get Charge info
        public List<HISChargeModel> GetCharges(string chargeids)
        {
            List<HISChargeModel> listReturn = new List<HISChargeModel>();
            int currentPage = 1;
            #region For statistic performance
            var start_time = DateTime.Now;
            TimeSpan tp;
            #endregion .For statistic performance

            string url_postfix = string.Format(
                "{0}/1.0.0/getChargeDetailsById?1=1",
                ConfigUrl.ORVersion
            );
        StepPaging:
            string url_charges = string.Empty;
            if (!string.IsNullOrEmpty(chargeids))
            {
                #region paging
                string[] arrCharges = chargeids.Split(';');
                List<string> listChargeIds = arrCharges.ToList();
                var chargeIds2Get = listChargeIds.Skip((currentPage - 1) * 20)
                .Take(20);
                #endregion
                if (chargeIds2Get.Any())
                {
                    string strCharges = string.Join(";", chargeIds2Get?.Select(x => x)?.ToList());
                    url_charges = string.Format("&ChargeId={0}", strCharges);
                }
                else
                {
                    goto StepReturn;
                }
            }
            //IFormatProvider viVNDateFormat = new CultureInfo("vi-VN").DateTimeFormat;
            bool bThrowEx = false;
            var response = RequestAPI(string.Format("{0}{1}", url_postfix, url_charges), "Entries", "Entry", out bThrowEx, ConfigUrl.CF_ApiTimeout_minutes);

            //List VisitType Package
            if (response != null)
            {
                listReturn.AddRange(response.Where(x => !listReturn.Any(el => el.ItemId == x["ItemId"]?.ToObject<Guid?>() && el.ChargeId == x["ChargeId"].ToObject<Guid>())).Select(e => new HISChargeModel
                {
                    ItemId = e["ItemId"]?.ToObject<Guid?>(),
                    ItemCode = e["ItemCode"]?.ToString(),
                    ChargeId = e["ChargeId"].ToObject<Guid>(),
                    NewChargeId = e["NewChargeId"]?.ToObject<Guid?>(),
                    ChargeSessionId = e["ChargeSessionId"]?.ToObject<Guid?>(),
                    ChargeDate = e["ChargeDate"]?.ToObject<DateTime?>(),
                    ChargeCreatedDate = e["ChargeCreatedDate"]?.ToObject<DateTime?>(),
                    ChargeUpdatedDate = e["ChargeUpdatedDate"]?.ToObject<DateTime?>(),
                    ChargeDeletedDate = e["ChargeDeletedDate"]?.ToObject<DateTime?>(),
                    ChargeStatus = e["ChargeStatus"]?.ToString(),
                    VisitType = e["VisitType"]?.ToString(),
                    VisitCode = e["VisitCode"]?.ToString(),
                    VisitDate = e["VisitDate"]?.ToObject<DateTime?>(),
                    InvoicePaymentStatus = e["InvoicePaymentStatus"]?.ToString(),
                    HospitalId = e["HospitalId"].ToObject<Guid>(),
                    HospitalCode = e["HospitalCode"]?.ToString(),
                    PID = e["PID"]?.ToString(),
                    CustomerId = e["CustomerId"]?.ToObject<Guid?>(),
                    CustomerName = e["CustomerName"]?.ToString(),
                    UnitPrice = e["UnitPrice"]?.ToObject<double?>(),
                    Quantity = (int?)e["Quantity"]?.ToObject<double?>()
                }));
            }
            if (!string.IsNullOrEmpty(chargeids))
            {
                currentPage++;
                goto StepPaging;
            }
        StepReturn:
            return listReturn;
        }
        public static List<HISChargeModel> GetChargesV2(string chargeids)
        {
            List<HISChargeModel> listReturn = new List<HISChargeModel>();
            int currentPage = 1;
            #region For statistic performance
            var start_time = DateTime.Now;
            TimeSpan tp;
            #endregion .For statistic performance

            string url_postfix = string.Format(
                "{0}/1.0.0/getChargeDetailsById?1=1",
                ConfigUrl.ORVersion
            );
        StepPaging:
            string url_charges = string.Empty;
            if (!string.IsNullOrEmpty(chargeids))
            {
                #region paging
                string[] arrCharges = chargeids.Split(';');
                List<string> listChargeIds = arrCharges.ToList();
                var chargeIds2Get = listChargeIds.Skip((currentPage - 1) * 20)
                .Take(20);
                #endregion
                if (chargeIds2Get.Any())
                {
                    string strCharges = string.Join(";", chargeIds2Get?.Select(x => x)?.ToList());
                    url_charges = string.Format("&ChargeId={0}", strCharges);
                }
                else
                {
                    goto StepReturn;
                }
            }
            //IFormatProvider viVNDateFormat = new CultureInfo("vi-VN").DateTimeFormat;
            bool bThrowEx = false;
            var response = RequestAPI(string.Format("{0}{1}", url_postfix, url_charges), "Entries", "Entry", out bThrowEx, ConfigUrl.CF_ApiTimeout_minutes);

            //List VisitType Package
            if (response != null)
            {
                listReturn.AddRange(response.Where(x => !listReturn.Any(el => el.ItemId == x["ItemId"]?.ToObject<Guid?>() && el.ChargeId == x["ChargeId"].ToObject<Guid>())).Select(e => new HISChargeModel
                {
                    ItemId = e["ItemId"]?.ToObject<Guid?>(),
                    ItemCode = e["ItemCode"]?.ToString(),
                    ChargeId = e["ChargeId"].ToObject<Guid>(),
                    NewChargeId = e["NewChargeId"]?.ToObject<Guid?>(),
                    ChargeSessionId = e["ChargeSessionId"]?.ToObject<Guid?>(),
                    ChargeDate = e["ChargeDate"]?.ToObject<DateTime?>(),
                    ChargeCreatedDate = e["ChargeCreatedDate"]?.ToObject<DateTime?>(),
                    ChargeUpdatedDate = e["ChargeUpdatedDate"]?.ToObject<DateTime?>(),
                    ChargeDeletedDate = e["ChargeDeletedDate"]?.ToObject<DateTime?>(),
                    ChargeStatus = e["ChargeStatus"]?.ToString(),
                    VisitType = e["VisitType"]?.ToString(),
                    VisitCode = e["VisitCode"]?.ToString(),
                    VisitDate = e["VisitDate"]?.ToObject<DateTime?>(),
                    InvoicePaymentStatus = e["InvoicePaymentStatus"]?.ToString(),
                    HospitalId = e["HospitalId"].ToObject<Guid>(),
                    HospitalCode = e["HospitalCode"]?.ToString(),
                    PID = e["PID"]?.ToString(),
                    CustomerId = e["CustomerId"]?.ToObject<Guid?>(),
                    CustomerName = e["CustomerName"]?.ToString(),
                    UnitPrice = e["UnitPrice"]?.ToObject<double?>(),
                    Quantity = (int?)e["Quantity"]?.ToObject<double?>()
                }));
            }
            if (!string.IsNullOrEmpty(chargeids))
            {
                currentPage++;
                goto StepPaging;
            }
        StepReturn:
            return listReturn;
        }
        #endregion
        #region Get from EMR
        public string GetDiagnosisByCharge(string chargeId)
        {
            string url_postfix = string.Format(
                "{0}/1.0.0/getChargeDetailsFromEMR?ChargeId={1}",
                ConfigUrl.ORVersion,
                chargeId
            );
            bool bThrowEx = false;
            var response = RequestAPI(url_postfix, "Entries", "Entry", out bThrowEx, ConfigUrl.CF_ApiTimeout_minutes);
            if (response != null)
            {
                return response["InitialDiagnosis"]?.ToString();
            }
            return string.Empty;
        }
        #endregion

        #region Update Dims to re-Calculate
        public int UpdateDimsToReCalculate(string chargeId)
        {
            string url_postfix = string.Format(
                "DimsVinmecCom/1.0.0/UpdateToReCalculate?chargeid={0}"
                ,chargeId
            );
            bool bThrowEx = false;
            var response = RequestAPI(url_postfix, "", "Data", out bThrowEx, ConfigUrl.CF_ApiTimeout_minutes,apiToken: ConfigurationManager.AppSettings["DIMS_API_SERVER_TOKEN"]);
            if (response != null)
            {
                var returnValue = response["Status"].ToString();
                return Convert.ToInt32(returnValue);
            }
            else
            {
                return 0;
            }
        }
        #endregion .Update Dims to re-Calculate
    }
}
