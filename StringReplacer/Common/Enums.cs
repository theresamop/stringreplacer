using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringReplacer.Common
{
    public class Enums
    {
        public enum ReplacerType
        {
            SQLTableToCSClass = 1,
        }

    }

    public static class CSDataTypeStrings
    {
        public const string Long = "long";
        public const string nvarchar = "string";
        public const string Char = "char";
        public const string Int = "int";
        public const string guid = "Guid";
        public const string Bool = "bool";
        public const string String = "string";
        public const string datetime = "DateTime";
    }

    public static class SQLDataTypeStrings
    {
        public const string bigint = "bigint";
        public const string nvarchar = "nvarchar";
        public const string varchar = "varchar";
        public const string Char = "char";
        public const string datetime = "datetime";
        public const string unique = "uniqueidentitifer";
        public const string bit = "bit";
        public const string Int = "int";
    }
}