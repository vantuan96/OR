using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Admin.Helper
{
    public class FileUploadResultModel
    {
        public FilenameInfo success { get; set; }
    }

    public class FilenameInfo
    {

        public string filename { get; set; }
        public string domain { get; set; }
        public string folder { get; set; }
    }

    public static class UploadHelper
    {
        /// <summary>
        /// Upload file to server and return new path
        /// </summary>
        /// <param name="server">Server address to store file</param>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <returns>Path of file on server</returns>
        public static string UploadByteDataFile(string server, string fileName, byte[] data)
        {
            byte[] responseArray = UploadFiles(server, fileName, data);
            string result = System.Text.Encoding.UTF8.GetString(responseArray);
            var uploadresult = JsonConvert.DeserializeObject<FileUploadResultModel>(result);
            var filePath = string.Empty;
            if (uploadresult.success != null)
            {
                filePath = uploadresult.success.folder + "/" + uploadresult.success.filename;
            }

            return filePath;
        }

        private static byte[] UploadFiles(string address, string fileName, byte[] fileBinary)
        {
            var request = WebRequest.Create(address);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                Stream stream = new MemoryStream(fileBinary);
                // Write the files
                var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"upload\"; filename=\"{1}\"{2}", fileName, fileName, Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", "application/octet-stream", Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);
                stream.CopyTo(requestStream);
                buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);

                var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var stream = new MemoryStream())
            {
                if (responseStream != null) responseStream.CopyTo(stream);
                return stream.ToArray();
            }
        }
    }
}