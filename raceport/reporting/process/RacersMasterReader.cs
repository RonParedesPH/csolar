using reporting.dtos;
using reporting.entities;
using reporting.unitofwork;
using SpreadsheetGear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reporting.process
{
    public class RacersMasterReader : IWorksheetWorker
    {
        private readonly IWorksheet _worksheet;
        private readonly IUnitOfWork _unitofwork;
        private class StashBag
        {
            public string appName { get; set; }
            public string title { get; set; }
            public string subTitle { get; set; }
            public int headerLine { get; set; }
            public int detailLineStart { get; set; }
            public int details { get; set; }

            public int racerDetailCount { get; set; }
            public int racerOffset { get; set; }
            public int racerHiddenOffset { get; set; }

            public int classDetailCount { get; set; }
            public int classOffset { get; set; }
            public int classHiddenOffset { get; set; }

            public int teamDetailCount { get; set; }
            public int teamOffset { get; set; }
            public int teamHiddenOffset { get; set; }
        }


        public string Path = string.Empty;
        public string Filename = string.Empty;
        public bool Validated = false;

        public RacersMasterReader(IWorksheet worksheet, IUnitOfWork unitofwork) : base(worksheet, unitofwork)
        {
            _unitofwork = unitofwork;
            _worksheet = worksheet;
        }

        public override bool Render(string appName, string title, string subTitle, string key, string tabName)
        {
            throw new NotImplementedException();
        }

        public List<RacerRawDto> ReadRacer()
        {
            var retList = new List<RacerRawDto>();
            columnDefSet racerSet = DefinedSetsOfColumnDefs.RaceSet();
            columnDefSet racerHidden = DefinedSetsOfColumnDefs.RaceHidden();

            rangeHelper rh = new rangeHelper(_worksheet);

            rh.Locate(racerSet.ColumnOffset, racerSet.DetailRow);
            while (rh.ToLeft(3).Formula != string.Empty)
            {
                if (rh.ToLeft(1).Text != string.Empty)
                {
                    retList.Add(new RacerRawDto()
                    {
                        Number= rh.ToLeft(2).Text,
                        Name = rh.ToLeft(3).Text,
                        CarDetails = rh.ToLeft(4).Text,
                        RaceClass = rh.ToLeft(5).Text,
                        Team = rh.ToLeft(6).Text,
                        Id = rh.ToLeft(racerHidden.ColumnOffset).Text,
                        dtlastmodified = rh.ToLeft(racerHidden.ColumnOffset +1).Text,
                    });
                }
                rh.Down(1);
            }
            return retList;
        }

        public List<ClassRawDto> ReadClass()
        {
            var retList = new List<ClassRawDto>();
            columnDefSet classSet = DefinedSetsOfColumnDefs.ClassSet();
            columnDefSet classHidden = DefinedSetsOfColumnDefs.ClassHidden();

            rangeHelper rh = new rangeHelper(_worksheet);

            rh.Locate(classSet.ColumnOffset, classSet.DetailRow);
            while (rh.ToLeft(2).Formula != string.Empty)
            {
                if (rh.ToLeft(1).Text != string.Empty)
                {
                    retList.Add(new ClassRawDto()
                    {
                        Code = rh.ToLeft(2).Text,
                        Description = rh.ToLeft(3).Text,
                        Id = rh.ToLeft(classHidden.ColumnOffset-(classSet.ColumnOffset-1)).Text,
                        dtlastmodified = rh.ToLeft(classHidden.ColumnOffset-(classSet.ColumnOffset-1) + 1).Text,
                    });
                }
                rh.Down(1);
            }
            return retList;
        }
        public List<TeamRawDto> ReadTeam()
        {
            var retList = new List<TeamRawDto>();
            columnDefSet teamSet = DefinedSetsOfColumnDefs.TeamSet();
            columnDefSet teamHidden = DefinedSetsOfColumnDefs.TeamHidden();

            rangeHelper rh = new rangeHelper(_worksheet);

            rh.Locate(teamSet.ColumnOffset, teamSet.DetailRow);
            while (rh.ToLeft(2).Formula != string.Empty)
            {
                if (rh.ToLeft(1).Text != string.Empty)
                {
                    retList.Add(new TeamRawDto()
                    {
                        Name = rh.ToLeft(2).Text,
                        Description = rh.ToLeft(3).Text,
                        Id = rh.ToLeft(teamHidden.ColumnOffset-(teamSet.ColumnOffset - 1)).Text,
                        dtlastmodified = rh.ToLeft(teamHidden.ColumnOffset - (teamSet.ColumnOffset - 1) + 1).Text,
                    });
                }
                rh.Down(1);
            }
            return retList;
        }

    }
}
