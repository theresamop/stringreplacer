using StringReplacer.Models;
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
            return View();
        }

        [HttpPost]
        public ActionResult GenerateString(StringReplacerViewModel vm)
        {
            byte[] content = null;
            var key = Guid.NewGuid().ToString();
            var filename = string.Format("mystringreplacer.txt");

            string[] lines = System.IO.File.ReadAllLines(vm.FilePathInput);
            System.IO.Stream stream = new System.IO.MemoryStream();
            StringBuilder sb = new StringBuilder();
           
            foreach (string line in lines)
            {


                var stringSPlit = line.Split(' ');
                if (stringSPlit.Any() && stringSPlit[0] != null)
                {
                    
                    var newL = stringSPlit[0].Replace("\t", "").Replace("[", "").Replace("]", "");
                         
                    string newString = "public string " +  newL + " { get; set; }";

                    sb.AppendLine(newString);
                }

                   
            }

           content = Encoding.ASCII.GetBytes(sb.ToString());
            
           
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