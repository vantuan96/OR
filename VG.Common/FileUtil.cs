using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VG.Common
{
    public static class FileUtil
    {
        /// <summary>
        /// Create log directory
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists((path)))
            {
                Directory.CreateDirectory((path));
            }
        }

        /// <summary>
        /// Create log file
        /// </summary>
        /// <param name="fileLogPath">file full path</param>
        public static void CreateXmlFile(string fileLogPath, string localName)
        {
            // create only if not extists
            if (!File.Exists(fileLogPath))
            {
                using (XmlWriter xmlWriter = new XmlTextWriter(fileLogPath, Encoding.UTF8))
                {
                    xmlWriter.WriteStartDocument(true);
                    xmlWriter.WriteElementString(localName, "");
                }
            }
        }

        /// <summary>
        /// Remove log file
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <returns></returns>
        public static bool RemoveLogFile(string filePath)
        {
            filePath = filePath + ".log";
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static bool ValidateImageFileExt(string ext)
        {
            string validExts = ",png,jpg,jpeg,rar,";
            if (validExts.IndexOf("," + ext.ToLower() + ",") >= 0)
            {
                return true;
            }
            return false;
        }
    }
}
