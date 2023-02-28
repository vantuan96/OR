using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string content)
        {
            Application appWord = new Application();
            string filePath = Server.MapPath(Url.Content("~/TemplateFiles/"));
            string pdfFileName = "DocTo.pdf";
            string wordFleName = "Template1.pdf";

            System.IO.File.Copy(filePath + "Template.docx", filePath + wordFleName, true);


            appWord.Visible = false;
            Document wordDoc = appWord.Documents.Open(filePath + wordFleName);
            wordDoc.Activate();
            FindAndReplace(appWord, "##companyname##", content);
            
            wordDoc.ExportAsFixedFormat(filePath + pdfFileName, WdExportFormat.wdExportFormatPDF);
            wordDoc.Close();
            appWord.Quit();

            byte[] filedata = System.IO.File.ReadAllBytes(filePath + pdfFileName);
            string contentType = MimeMapping.GetMimeMapping(filePath + pdfFileName);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = pdfFileName,
                Inline = true,
            };

            System.IO.File.Delete(filePath + wordFleName);
            System.IO.File.Delete(filePath + pdfFileName);

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }

        private void FindAndReplace(Microsoft.Office.Interop.Word.Application WordApp, object findText, object replaceWithText)
        {
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object nmatchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
            object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
            WordApp.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord, ref matchWildCards, ref matchSoundsLike,
            ref nmatchAllWordForms, ref forward,
            ref wrap, ref format, ref replaceWithText,
            ref replaceAll, ref matchKashida,
            ref matchDiacritics, ref matchAlefHamza,
            ref matchControl);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}