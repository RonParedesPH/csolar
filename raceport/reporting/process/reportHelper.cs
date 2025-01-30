using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reporting.process
{
        internal static class reportHelper
        {

            public static string column(int x)
            {
                string ret = "";
                if (x > 26)
                {
                    int z = x / 26;
                    x = (x - (z * 26));

                    ret = string.Format("{0}", (Char)(z + 64));
                }

                return ret + string.Format("{0}", (Char)(x + 64));
            }
            public static string cell(int x, int y)
            {
                string ret = "";

                int z = (x - 1) / 26;
                if (z > 0)
                {
                    x = (x - (z * 26));
                    if (x == 0)
                        x = 1;

                    ret = string.Format("{0}", (Char)(z + 64));
                }

                ret =  ret + string.Format("{0}{1}", (Char)(x + 64), y);
            return ret;
            }

            public static string range(int x, int y, int extend_x, int extend_y)
            {
                return string.Format("{0}:{1}", cell(x, y), cell(x + extend_x, y + extend_y));
            }

            public static void setBorders(SpreadsheetGear.IRange range,
                SpreadsheetGear.BordersIndex index,
                System.Drawing.Color color,
                SpreadsheetGear.LineStyle linestyle,
                SpreadsheetGear.BorderWeight borderweight)
            {
                SpreadsheetGear.IBorder b = range.Borders[index];
                b.Color = color;
                b.LineStyle = linestyle;
                b.Weight = borderweight;
            }

        }

}
