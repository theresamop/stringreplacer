using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StringReplacer.Models
{
    public class StringReplacerViewModel
    {
        public string FilePathInput { get; set; }
        public string FilePathOutput { get; set; }
        public string StringDynamicInput { get; set; }
        public string StringMake { get; set; }
        public int ReplacerType { get; set; }
        public string IsDotNet { get; set; }
        public IEnumerable<SelectListItem> ReplacerTypes
        {
            get
            {
                return new List<SelectListItem>
                {

                    new SelectListItem() { Text = "SQL Table to Class", Value = "1" },
                    new SelectListItem() { Text = "Custom Pattern", Value = "2" }
                };
            }
        }

    }
}