using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reporting.dtos
{
    public class MasterRawDto
    {
        public string errorMessage { get; set; } = string.Empty;
        public List<RacerRawDto> RacerRaw { get; set; }
        public List<ClassRawDto> ClassRaw { get; set; }
        public List<TeamRawDto> TeamRaw { get; set; }
    }
}
