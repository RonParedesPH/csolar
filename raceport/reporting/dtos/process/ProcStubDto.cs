using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reporting.process
{
    public class ProcStubDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Output { get; set; }
        public string Stage { get; set; }
        public int Progress { get; set; }
        public DateTime? LastTick { get; set; }

        public int status { get; set; }
        public DateTime dtcreated { get; set; }
        public DateTime? dtlastmodified { get; set; }
        public DateTime? dterased { get; set; }


        public DateTime? beginDate { get; set; }
        public DateTime? endDate { get; set; }
        public string Outlet { get; set; }
        public string Username { get; set; }
    }
}