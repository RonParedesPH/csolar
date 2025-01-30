using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reporting.dtos
{
    public class RacerRawDto
    {
        public string Name { get; set; }
        public string Number { get;set; }
        public string CarDetails { get; set; }
        public string RaceClass { get; set; }
        public string Team {  get; set; }
        public string Notes { get; set; }

        public string Id { get; set; }
        public string dtlastmodified { get; set; }
        public string lastResult { get; set; }

    }
}
