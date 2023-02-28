using System;
using System.IO;

namespace Admin.SMTPMail
{
    public class CreateLogFiles
    {
        private string sLogFormat;
        private string sErrorTime;
        private System.DateTime startTime;
        public CreateLogFiles()
        {
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sDay + "_" + sMonth + "_" + sYear;
            this.startTime = DateTime.Now;
        }
        public void ErrorLog(string sPathName, string sErrMsg, string source)
        {
            try
            {
                if (!Directory.Exists(sPathName)) Directory.CreateDirectory(sPathName);
                StreamWriter sw = new StreamWriter(sPathName + sErrorTime + ".txt", true);
                sw.WriteLine(sLogFormat + sErrMsg);
                sw.WriteLine(source);
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
            catch (Exception) { }
        }
        private string calculateTime()
        {
            return (DateTime.Now - this.startTime).ToString();
        }


    }
}
