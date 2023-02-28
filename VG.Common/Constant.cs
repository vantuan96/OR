using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VG.Common
{
    public class Constant
    {
        #region Log
        public readonly static string Log_Type_Info = "Log_Info";
        public readonly static string Log_Type_Debug = "Log_Debug";
        public readonly static string Log_Type_Error = "Log_Error";
        #endregion
        #region Service Type
        /// <summary>
        /// Dịch vụ
        /// </summary>
        public readonly static string SERVICE_TYPE_SRV = "SRV";
        /// <summary>
        /// Thuốc / VTTH
        /// </summary>
        public readonly static string SERVICE_TYPE_INV = "INV";
        /// <summary>
        /// Gói/Package
        /// </summary>
        public readonly static string SERVICE_TYPE_PCK = "PCK";
        #endregion .Service Type
        #region Surgery type value
        public readonly static List<string> ListCodeIsProcedure = new List<string>() { "e15.03", "thủ thuật", "procedure" };
        public readonly static List<string> ListCodeIsSurgical = new List<string>() { "e15.02", "phẫu thuật", "surgical" };
        public readonly static List<string> ListCodeIsSurgicalProcedure = new List<string>() { "e15", "phẫu thuật", "surgical", "procedure", "thủ thuật" };
        public readonly static List<string> ListGroupCodeNameIsSurgicalProcedure = new List<string>() { "E10.03", "E10.02", "e15.02", "e15.03", "e03.02", "e03.03", "e30.03", "e36.02", "e36.03", "e42.03", "phẫu thuật", "surgical", "procedure", "thủ thuật" };
        /// <summary>
        /// Gây mê - giảm đau
        /// </summary>
        public readonly static List<string> ListGroupCodeIsAnesth = new List<string>() { "E30.03" };
        #endregion
        public readonly static List<int> ListStateAllowCoordinator = new List<int>() { 1, 3, 5, 6, 7, 8, 9, 11, 12, 13, 15, 16, 33 };
        public readonly static List<int> ListStateNotCheckChargeId = new List<int>() { 0, 1,33 };
    }
}
