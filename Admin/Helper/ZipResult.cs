using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Helper
{
    public class ZipResult : ActionResult
    {
        private IEnumerable<string> _files;
        private string _fileName;

        public string FileName
        {
            get
            {
                return _fileName ?? "file.zip";
            }
            set { _fileName = value; }
        }

        public ZipResult(params string[] files)
        {
            this._files = files;
        }

        public ZipResult(IEnumerable<string> files)
        {
            this._files = files;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            using (ZipFile zf = new ZipFile())
            {
                int i = 1;
                foreach (var item in _files)
                {

                    if(item.IndexOf("googlefeed")>0){
                        zf.AddFile(item, "googlefeed");
                    }
                    else if (item.IndexOf("fbfeed") > 0)
                    {
                        zf.AddFile(item, "fbfeed");
                    }
                    else{
                        zf.AddFile(item, "");
                    }
                    i++;
                }
                
                context.HttpContext.Response.ContentType = "application/zip";
                context.HttpContext.Response.AppendHeader("content-disposition", "attachment; filename=" + FileName);
                zf.Save(context.HttpContext.Response.OutputStream);
            }
        }
    }
}