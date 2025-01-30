using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stockroom.helpers
{
    public partial class DataTableResponse<T> where T : class
    {
        public DataTableResponse()
        {
            data = new HashSet<T>();
        }

        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public virtual ICollection<T> data { get; set; }
    }
}