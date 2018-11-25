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
            else if ((int)ReplacerType.CustomString == vm.ReplacerType)
            {
                sbuilder = BuildCustomString(vm, lines);
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


        private string BuildCustomString(StringReplacerViewModel vm, string[] lines)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //vm.{Id} = post.{Id}
            List<int> dynamicInputIndexes = new List<int>();

            //get dynamic Input accdg to user input
            var splittedSample = vm.StringDynamicInput.Split(Convert.ToChar(vm.Splitter));
            var indexDynamicInput = splittedSample.Select((c, i) => new { c, i }).Where(c => c.c.ToString().StartsWith("{") && c.c.EndsWith("}") && !c.c.StartsWith("{{") && !c.c.EndsWith("}}") ).Select(c => c.i); //splittedSample.Where((c,i) => c.StartsWith("{") && c.EndsWith("}") && !c.StartsWith("{{") && !c.EndsWith("}}")).Select((c,i)=> i).ToArray();


            object [] parameterArgs;
            List<object> parameters = new List<object>(); ;
            StringBuilder newLSb = new StringBuilder();
            string tempLine = "";
            foreach (string line in lines)
            {
                parameters.Clear();
                if (!string.IsNullOrEmpty(line))
                {
                    var charsToremove = vm.CharsToRemove.Split(',');

                    newLSb.Clear();
                    newLSb.AppendLine(line);
                    foreach (var c in charsToremove)
                    {
                        newLSb = newLSb.Replace(c.ToString(), "");
                    }
                    var stringSPlit = newLSb.ToString().Split(Convert.ToChar(vm.Splitter));
             

                    foreach (var param in indexDynamicInput)
                    {
                        var type = GetDataType(stringSPlit[param]);
                        if (vm.IsConvertDatatype && !string.IsNullOrEmpty(type))
                            parameters.Add(type);
                        else parameters.Add(stringSPlit[param]);
                    }
                    parameterArgs = parameters.ToArray();

                    if (stringSPlit.Any() && stringSPlit[0] != null)
                    {
                      
                        string newString = string.Format(vm.StringMake, parameterArgs);

                        stringBuilder.AppendLine(newString);
                    }
                }


            }

            return stringBuilder.ToString();
        }

        public string GetDataType(string type)
        {
            string datatype = "";

            switch (type)
            {
                case SQLDataTypeStrings.bigint:
                    datatype = CSDataTypeStrings.Long;
                    break;
                case SQLDataTypeStrings.Int:
                    datatype = CSDataTypeStrings.Int;
                    break;
                case SQLDataTypeStrings.nvarchar:
                case SQLDataTypeStrings.varchar:
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
                case SQLDataTypeStrings.Decimal:
                    datatype = CSDataTypeStrings.Decimal;
                    break;
                case SQLDataTypeStrings.Float:
                    datatype = CSDataTypeStrings.Float;
                    break;
            }
            return datatype;
        }

    }
}