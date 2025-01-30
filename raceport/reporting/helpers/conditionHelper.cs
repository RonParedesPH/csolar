using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reporting.helpers
{
    public static class ConditionHelper
    {
        public static string BuildCondition(string field, string value, string oper, string datatype)
        {
            switch (datatype)
            {
                case "NVARCHAR":
                    value = string.Format("{1}{0}{1}", value, "'");
                    break;
                case "INT":
                    break;
                case "DATETIME":
                    value = string.Format("{1}{0}{1}", value, "'");
                    break;
            }
            return string.Format("{0} {1} {2}", field, oper, value);
        }
    }
}