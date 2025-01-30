using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reporting.process
{
    internal class columnDefs
    {
        public string id { get; set; }
        public string title { get; set; }
        public int width { get; set; }
        public int pos { get; set; }
        public int align { get; set; }
        public string numberFormat { get; set; }
        public bool sum { get; set; }

        public columnDefs(string id, string title, int width, int align, string numberFormat, bool sum)
        {
            this.id = id;
            this.title = title;
            this.width = width;
            this.align = align;
            this.numberFormat = numberFormat;
            this.sum = sum;
        }

        public int edge()
        {
            return this.pos + this.width - 1;
        }
    }

    internal class columnDefSet
    {
        public string Name { get; set; } = string.Empty;
        public int HeaderRow { get; set; } = 0;
        public int DetailRow { get; set; } = 0;
        public int StartCol { get; set;} = 0;
        public int DetailsCount { get; set; } = 0;
        public int ColumnOffset { get; set; } = 0;
        public List<columnDefs> columnList { get; set; } = new List<columnDefs>();
    }
}
