using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reporting.dtos
{
    public class TeamRawDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Id { get; set; }
        public string dtlastmodified { get; set; }
        public string lastResult { get; set; }
    }
}
