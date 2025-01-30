using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reporting.helpers
{
    public static class UlidFormatter
    {
        public static String ToString(string ulid)
        {
            string ret = string.Empty;

            if (ulid.Length < 26)
                ulid = (ulid + "00000000000000000000000000").Substring(0, 26);
            ret = string.Format("{0}-{1}-{2}-{3}",
                ulid.Substring(0, 8),
                ulid.Substring(8, 4),
                ulid.Substring(12, 6),
                ulid.Substring(18));
            return ret.ToLower();
        }
    }
}
