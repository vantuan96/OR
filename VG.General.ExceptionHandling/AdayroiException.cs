using System;
using System.ComponentModel;
using System.Reflection;

namespace VG.General.ExceptionHandling
{
    public enum ErrorSeverity
    {
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        Fatal = 5
    }

    public enum ErrorCode
    {
        /// <summary>
        /// Lỗi không xác định
        /// </summary>
        [Description("Lỗi không xác định")]
        UnknownError,

        /// <summary>
        /// Không thể mở kết nối tới CSDL
        /// </summary>
        [Description("Không thể mở kết nối tới CSDL")]
        CouldNotOpenDBConnection,

        /// <summary>
        /// Không thể thực thi truy vấn đến CSDL
        /// </summary>
        [Description("Không thể thực thi truy vấn dữ liệu")]
        CouldNotExecuteQuery,

        /// <summary>
        /// Không đọc được dữ liệu trong cache
        /// </summary>
        [Description("Lỗi đọc dữ liệu")]
        CouldNotReadRedisData,

        /// <summary>
        /// Lỗi không kết nối được máy chủ redis
        /// </summary>
        [Description("Lỗi không kết nối được máy chủ caching")]
        CouldNotConnectCacheServer,

        /// <summary>
        /// Lỗi không kết nối được api server
        /// </summary>
        [Description("Lỗi không kết nối đến api server")]
        CouldNotConnectApiServer,

        /// <summary>
        /// Xảy ra lỗi. Vui lòng thử lại
        /// </summary>
        [Description("Xảy ra lỗi. Vui lòng thử lại")]
        RuntimeError,

        /// <summary>
        /// Lỗi tại tầng Biz
        /// </summary>
        [Description("Lỗi tại tầng Biz")]
        CouldNotExeBizLayer,

        /// <summary>
        /// Lỗi tại tầng Biz
        /// </summary>
        [Description("Lỗi tại tầng Cache")]
        CouldNotExeCacheLayer,

        /// <summary>
        /// Không đọc được dữ liệu trong cache
        /// </summary>
        [Description("Lỗi đọc dữ liệu từ memory cache")]
        CouldNotReadMemCacheData,
    }

    public class ConstValue
    {
        public static string ErrorSourceFormat = "{0}.{1}";
        public static string SystemMessageFormat = "{0}-{1}-{2}";
    }

    public static class EnumUtil
    {
        /// <summary>
        /// Get description annotation of an enum value
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum element)
        {
            Type type = element.GetType();

            MemberInfo[] memberInfo = type.GetMember(element.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return element.ToString();
        }
    }

    public class VGException : Exception
    {
        public ErrorSeverity ErrorSeverity { get; set; }

        public ErrorCode ErrorCode { get; set; }

        public string ErrorSource { get; set; }

        public string ErrorDescription { get; set; }

        public string ErrorStackTrace { get; set; }

        /// <summary>
        /// Friendly message
        /// </summary>
        public string UserMessage
        {
            get
            {
                return ErrorCode.GetDescription();
            }
        }

        /// <summary>
        /// System message, use for logging
        /// </summary>
        public string SystemMessage
        {
            get
            {
                return string.Format(ConstValue.SystemMessageFormat, ErrorCode, ErrorSource, ErrorStackTrace);
            }
        }

        /// <summary>
        /// Construct new AdayroiException
        /// </summary>
        /// <param name="errorSeverity"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorSource"></param>
        /// <param name="errorDescription"></param>
        /// <param name="errorStackTrace"></param>
        public VGException(ErrorSeverity errorSeverity, ErrorCode errorCode, string errorSource, string errorDescription, string errorStackTrace)
        {
            this.ErrorSeverity = errorSeverity;
            this.ErrorCode = errorCode;
            this.ErrorSource = errorSource;
            this.ErrorDescription = errorDescription;
            this.ErrorStackTrace = errorStackTrace;
        }

        public VGException(Exception ex, ErrorCode errCode=ErrorCode.RuntimeError): this(ErrorSeverity.Error,
                    ErrorCode.RuntimeError,
                    ex.Source,
                    ex.Message,
                    ex.StackTrace)
        {}

        /// <summary>
        /// Append stack trace
        /// </summary>
        /// <param name="errorSource"></param>
        public void AddSource(string errorSource, string errorStackTrace)
        {
            this.ErrorSource = string.IsNullOrEmpty(ErrorSource) ? errorSource : string.Format("{0}-{1}", errorSource, ErrorSource);
            this.ErrorStackTrace = string.IsNullOrEmpty(ErrorStackTrace) ? errorStackTrace : string.Format("{0}-{1}", errorSource, ErrorStackTrace);
        }

        /// <summary>
        /// Build message string from a AdayroiException
        /// </summary>
        /// <param name="ex">AdayroiException</param>
        /// <returns></returns>
        public override string ToString()
        {
            return /*DateTime.Now.ToString()
                + "\r\n */
                "UserMessage: \r\n\t" + this.UserMessage
                + "\r\n\r\n Message: \r\n\t" + this.Message
                + "\r\n\r\n ErrorDescription: \r\n\t" + this.ErrorDescription
                + "\r\n\r\n StackTrace: \r\n\t" + this.StackTrace
                + "\r\n\r\n ErrorStackTrace: \r\n\t" + this.ErrorStackTrace
                + "\r\n\r\n Source: \r\n\t" + this.ErrorSource;
        }
    }
}