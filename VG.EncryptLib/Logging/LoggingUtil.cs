using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VG.Common;

namespace VG.EncryptLib.Logging
{
    public enum LogSource
    {
        DBConnect = 1,
        Caching = 2,
        SerializeObj = 3,
        HttpClientAPI = 4,
        Runtime = 5,
        APIListener = 6,
        Application = 7,
        Biz = 8,
        WindowService = 9
    }

    /// <summary>
    /// Logging error to file 
    /// Must define Configuration key: ADR.IsLogLocal, ADR.LoggingDir
    /// </summary>
    public sealed class LoggingUtil
    {
        private string path = ConfigurationManager.AppSettings["VG.LoggingDir"];
        private bool isLog = true;
        private string dateFormat = "yyyy_MM_dd_hh_tt";
        private string logFileExt = ".log";

        public LoggingUtil()
        {
            if (ConfigurationManager.AppSettings["VG.IsLogLocal"] != null)
            {
                isLog = ConfigurationManager.AppSettings["VG.IsLogLocal"] == "1";
            }
            else
            {
                isLog = false;
            }
        }

        /// <summary>
        /// Saves error message to log file.
        /// </summary>
        /// <param name="e" >Exception to write down into XML file</param>
        public void WriteExceptionToXmlFile(System.Exception e, LogSource source, string TargetSite = null, string StackTrace = null)
        {
            if (!isLog) { return; }

            try
            {
                string strOriginal = null;
                string FileLog;

                string subpath = LogSourceToPath(source);

                FileUtil.CreateDirectory(path);

                FileLog = (path + subpath + "Log-" + DateTime.Now.ToString(dateFormat) + logFileExt);

                FileUtil.CreateXmlFile(FileLog, XML_ERRORLOG);

                XmlTextReader xmlReader = new XmlTextReader(FileLog);
                xmlReader.WhitespaceHandling = WhitespaceHandling.None;
                //Moves the reader to the root element.
                // skip <?xml version="1.0" ...
                xmlReader.MoveToContent();
                strOriginal = xmlReader.ReadInnerXml();
                xmlReader.Close();
                // write new document
                using (XmlTextWriter xmlWriter = new XmlTextWriter(FileLog, System.Text.Encoding.UTF8))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement(XML_ERRORLOG);
                    // write original content
                    xmlWriter.WriteRaw(strOriginal);

                    WriteException(e, xmlWriter, TargetSite, StackTrace);
                    xmlWriter.WriteEndElement();
                }
            }
            catch (Exception ex) { }
        }

        #region Browse Log File

        /// <summary>
        /// List log directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<LogFile> DirLogFolder(string path)
        {
            List<LogFile> logs = new List<LogFile>();

            try
            {
                var folders = System.IO.Directory.GetDirectories(path).ToList().Select(x => new LogFile
                {
                    FileName = new DirectoryInfo(x).Name.Replace(logFileExt, ""),
                    FileType = 1,
                    Files = System.IO.Directory.GetFiles(path + new DirectoryInfo(x).Name + "/").ToList().Select(y => new LogFile
                    {
                        FileName = Path.GetFileName(y).Replace(logFileExt, ""),
                        FileType = 2
                    }).ToList()
                }).ToList();

                logs.AddRange(folders);

                //List file in root
                var files = System.IO.Directory.GetFiles(path).ToList();
                logs.AddRange(files.Select(x => new LogFile
                {
                    FileName = Path.GetFileName(x).Replace(logFileExt, ""),
                    FileType = 2
                }).ToList());
            }
            catch (Exception)
            { }

            return logs;
        }

        /// <summary>
        /// Read log file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public LogFileContent GetLogFileContent(string filePath)
        {
            LogFileContent content = new LogFileContent();

            if (File.Exists(filePath))
            {
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(filePath))
                {
                    String line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.AppendLine(line);
                    }
                }
                string allines = sb.ToString();

                content.FileContent = (allines);
                content.FileName = Path.GetFileName(filePath);
            }

            return content;
        }

        #endregion Browse Log File

        #region Private method

        /// <summary>
        /// root element for error log
        /// </summary>
        private const string XML_ERRORLOG = "errorlog";

        /// <summary>
        /// base element for exceptions
        /// </summary>
        private const string XML_EXCEPTION = "exception";

        /// <summary>
        /// time when exception raised
        /// </summary>
        private const string XML_EXCEPTION_TIME = "time";

        /// <summary>
        /// description for exception
        /// </summary>
        private const string XML_EXCEPTION_DESCRIPTION = "description";

        /// <summary>
        /// method where exception raised
        /// </summary>
        private const string XML_EXCEPTION_METHOD = "method";

        /// <summary>
        /// help link - not supported yet!
        /// </summary>
        private const string XML_EXCEPTION_HELPLINK = "helplink";

        /// <summary>
        /// trace
        /// </summary>
        private const string XML_EXCEPTION_TRACE = "trace";

        /// <summary>
        /// Write exception into xml log file
        /// </summary>
        /// <param name="e">Exception</param>
        /// <param name="xmlWriter"></param>
        /// <param name="TargetSite"></param>
        /// <param name="StackTrace"></param>
        private void WriteException(Exception e, XmlWriter xmlWriter, string TargetSite = null, string StackTrace = null)
        {
            if (!isLog) { return; }

            xmlWriter.WriteStartElement(XML_EXCEPTION);
            xmlWriter.WriteElementString(XML_EXCEPTION_TIME, System.DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt"));
            // write the description of exeption
            xmlWriter.WriteElementString(XML_EXCEPTION_DESCRIPTION, e.Message + e.Source);

            // method where exception was thrown
            if (e.TargetSite != null)
                xmlWriter.WriteElementString(XML_EXCEPTION_METHOD, e.TargetSite.ToString());
            else if (TargetSite != null)
                xmlWriter.WriteElementString(XML_EXCEPTION_METHOD, TargetSite);

            // help link
            if (e.HelpLink != null)
                xmlWriter.WriteElementString(XML_EXCEPTION_HELPLINK, e.HelpLink);

            // call stack trace
            xmlWriter.WriteStartElement(XML_EXCEPTION_TRACE);

            if (StackTrace != null && string.IsNullOrEmpty(e.StackTrace))
            {
                xmlWriter.WriteString(StackTrace);
            }
            else
            {
                xmlWriter.WriteString(e.StackTrace);
            }

            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Get path by log source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string LogSourceToPath(LogSource source)
        {
            string subpath;
            switch (source)
            {
                case LogSource.DBConnect:
                    {
                        subpath = "DBConnectionErr_";
                        break;
                    }
                case LogSource.Caching:
                    {
                        subpath = "Caching_";
                        break;
                    }
                case LogSource.HttpClientAPI:
                    {
                        subpath = "HttpClientAPI_";
                        break;
                    }
                case LogSource.SerializeObj:
                    {
                        subpath = "SerializeObj_";
                        break;
                    }
                case LogSource.APIListener:
                    {
                        subpath = "APIListener_";
                        break;
                    }
                case LogSource.Runtime:
                    {
                        subpath = "Runtime_";
                        break;
                    }

                case LogSource.Application:
                    {
                        subpath = "Application_";
                        break;
                    }
                case LogSource.WindowService:
                    {
                        subpath = "WindowService_";
                        break;
                    }
                default:
                    {
                        subpath = "Other_";
                        break;
                    }
            }
            return subpath;
        }

        #endregion Private method
    }
}