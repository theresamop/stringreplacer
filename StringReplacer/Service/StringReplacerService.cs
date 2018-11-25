using StringReplacer.Common;
using StringReplacer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static StringReplacer.Common.Enums;

namespace StringReplacer.Service
{
    public class StringReplacerService
    {

        public byte[] ProcessString(StringReplacerViewModel vm)
        {
            string[] lines = System.IO.File.ReadAllLines(vm.FilePathInput);
            System.IO.Stream stream = new System.IO.MemoryStream();
            string sbuilder = "";

            if ((int)ReplacerType.SQLTableToCSClass == vm.ReplacerType)
            {
                sbuilder = BuilDSQLTableToCSClass(vm, lines);
            }

            return Encoding.ASCII.GetBytes(sbuilder);

        }

        private string BuilDSQLTableToCSClass(StringReplacerViewModel vm, string[] lines)
        {
            StringBuilder stringBuilder = new StringBuilder();
            vm.StringMake = @"public {0} {1} {{get;set;}}";
            foreach (string line in lines)
            {

                if (!string.IsNullOrEmpty(line))
                {

                    var stringSPlit = line.Split(' ');
                    if (stringSPlit.Any() && stringSPlit[0] != null)
                    {
                        var isReqd = line.ToLower().Contains("not null");
                        var newL = stringSPlit[0].Replace("\t", "").Replace("[", "").Replace("]", "");
                        var type = stringSPlit[1].Any() ? stringSPlit[1].Replace("[", "").Replace("]", "") : "string";
                        var datatype = GetDataType(type);
                        string newString = string.Format(vm.StringMake, datatype + (isReqd ? "" : (datatype == CSDataTypeStrings.String ? "" : "?")), newL);

                        stringBuilder.AppendLine(newString);
                    }
                }


            }

            return stringBuilder.ToString();
        }

        public string GetDataType(string type)
        {
            string datatype = CSDataTypeStrings.String;

            switch (type)
            {
                case SQLDataTypeStrings.bigint:
                    datatype = CSDataTypeStrings.Long;
                    break;
                case SQLDataTypeStrings.Int:
                    datatype = CSDataTypeStrings.Int;
                    break;
                case SQLDataTypeStrings.nvarchar:
                    datatype = CSDataTypeStrings.String;
                    break;
                case SQLDataTypeStrings.datetime:
                    datatype = CSDataTypeStrings.datetime;
                    break;
                case SQLDataTypeStrings.bit:
                    datatype = CSDataTypeStrings.Bool;
                    break;
                case SQLDataTypeStrings.Char:
                    datatype = CSDataTypeStrings.Char;
                    break;
                case SQLDataTypeStrings.unique:
                    datatype = CSDataTypeStrings.guid;
                    break;
            }
            return datatype;
        }

        //public List<SelectListItem> GetAllReplacerTypes()
        //{
        //    List<SelectListItem> selectLists = new List<SelectListItem>()
        //    {
        //         new SelectListItem() { Text = "SQL Table to CS Class", Value = "1" }
        //    };

        //    return selectLists;
        //}
    }
}