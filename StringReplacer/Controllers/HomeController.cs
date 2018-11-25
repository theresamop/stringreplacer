using StringReplacer.Models;
using StringReplacer.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace StringReplacer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StringReplacer()
        {
            StringReplacerViewModel vm = new StringReplacerViewModel();
            vm.IsDotNet = "C#";
            vm.ReplacerType = 1;
            return View(vm);
        }

        [HttpPost]
        public ActionResult GenerateString(StringReplacerViewModel vm)
        {
            byte[] content = null;
            var key = Guid.NewGuid().ToString();
            var filename = string.Format("mystringreplacer.txt");
            StringReplacerService stringReplacerService = new StringReplacerService();
            //vm.ReplacerType  = 1;
            vm.FilePathInput = @"C:\Users\dell\Desktop\stringReplacer\s.txt";

            content = stringReplacerService.ProcessString(vm);

            Session[key] = new Tuple<string, byte[]>(filename, content);


            return Json(new { key, success = true });
        }

        [HttpGet]
        public ActionResult GetFile(string key)
        {
            try
            {
                var data = Session[key] as Tuple<string, byte[]>;
                Session.Remove(key);

                Response.AppendHeader("Content-Disposition", "attachment; filename=" + data.Item1);
                return File(data.Item2, MimeMapping.GetMimeMapping(data.Item1));
            }
            catch (Exception e)
            {

                throw;
            }
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