using reporting.entities;
using reporting.unitofwork;
using SpreadsheetGear;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace reporting.process
{
    public class RacersMasterRender : IWorksheetWorker
    {
        private readonly IWorksheet _worksheet;
        private readonly IUnitOfWork _unitofwork;
        private class StashBag { 
            public string appName { get; set; }
            public string title { get; set; }
            public string subTitle { get; set; }
            //public int headerLine { get; set; }
            //public int detailLineStart { get; set; }
            //public int details { get; set; } 

            //public int racerDetailCount { get; set; }
            //public int racerOffset { get; set; }
            //public int racerHiddenOffset { get; set; }

            //public int classDetailCount { get; set; }
            //public int classOffset { get; set; }
            //public int classHiddenOffset { get; set; }

            //public int teamDetailCount { get; set; }
            //public int teamOffset { get; set; }
            //public int teamHiddenOffset { get; set; }
            public columnDefSet racerSet { get; set; }
            public columnDefSet racerHidden { get; set; }
            public columnDefSet classSet { get; set; }
            public columnDefSet classHidden { get; set; }
            public columnDefSet teamSet { get; set; }
            public columnDefSet teamHidden { get; set; }
        }

        private StashBag stash;
        public RacersMasterRender( IWorksheet worksheet, IUnitOfWork unitofwork) : base(worksheet, unitofwork)
        {
            _unitofwork = unitofwork;
           _worksheet = worksheet;
        }

        public override bool Render(string appName, string title, string subTitle, string key, string tabName)
        {
            stash = new StashBag() {
                appName = appName,
                title = title,
                subTitle = subTitle,
                //headerLine = 7,
                //detailLineStart = 8
                racerSet = DefinedSetsOfColumnDefs.RaceSet(),
                racerHidden = DefinedSetsOfColumnDefs.RaceHidden(),
                classSet = DefinedSetsOfColumnDefs.ClassSet(),
                classHidden = DefinedSetsOfColumnDefs.ClassHidden(),
                teamSet = DefinedSetsOfColumnDefs.TeamSet(),
                teamHidden = DefinedSetsOfColumnDefs.TeamHidden(),
            };

            bool result = PrepareWorksheet();
            result = PrintContent();
            result = VerticalParameters();
            return result;
        }

        //        bool _printHeaderLines(List<columnDefs> defsList, int offset, int line) {
        bool _printHeaderLines(columnDefSet defSet)
        {
            IRange r = _worksheet.Cells["A1"];
            r = _worksheet.Cells[reportHelper.cell(defSet.ColumnOffset, defSet.HeaderRow -1)];
            r.Formula = defSet.Name;

            foreach (columnDefs def in defSet.columnList)
            {
                r = _worksheet.Cells[reportHelper.cell(def.pos + defSet.ColumnOffset, defSet.HeaderRow)];
                r.Formula = def.title;
                r.ColumnWidth = def.width * 1.85;
                r.HorizontalAlignment = SpreadsheetGear.HAlign.Center;
                reportHelper.setBorders(
                    _worksheet.Cells[reportHelper.range(def.pos + defSet.ColumnOffset, defSet.HeaderRow, 0, 0)],
                    SpreadsheetGear.BordersIndex.EdgeBottom,
                    System.Drawing.Color.Black, SpreadsheetGear.LineStyle.Continous,
                    SpreadsheetGear.BorderWeight.Medium);

            }
            return true;
        }        
        
        public bool PrepareWorksheet()
        {


            var wsht = _worksheet;
            
                var cells = wsht.Cells;
                var r = cells["A:OZ"];
                r.ColumnWidth = 1.5; // 1twip=12pixels
                r.RowHeight = 13.5;
                r.Font.Name = "Calibri Light";
                r.Font.Size = 10;

            r = cells["A1"];
                r.RowHeight = 9.0;
                r.Font.Size = 8.0;
                r.Formula = "";

            r = cells["A2"];
                r.RowHeight = 9.0;
                r.Font.Size = 8.0;
                r.Formula = stash.appName;

            r = cells["A3"];
                r.RowHeight = 16.0;
                r.Font.Size = 14.0;
                r.Formula = stash.title;

            r = cells["A4"];
                r.RowHeight = 13.0;
                cells["A4"].Font.Size = 11;
                r.Formula = stash.subTitle;


            _printHeaderLines(stash.racerSet);

            _printHeaderLines(stash.classSet);

            _printHeaderLines(stash.teamSet);
            return true;
        }


        public bool PrintContent() {

            var r = _worksheet.Cells["A1"];
            int offset = 0;
            int line = 0;
            int hidden = 0;

            //_stash.racerHiddenOffset = offset + 100;

            //List<columnDefs> defsList = _racerColumns();
            //columnDefSet racerSet = DefinedSetsOfColumnDefs.RaceMaster();
            //columnDefSet racerHidden = DefinedSetsOfColumnDefs.RaceHidden();
            //List<columnDefs> defsList = racerSet.columnList;
            //int line = racerSet.DetailRow +1;
            IEnumerable<Racer> racers = _unitofwork.Racers.GetAll();
            List<Racer> racersList = racers.OrderBy(c => c.Number).ToList();

            line = stash.racerSet.DetailRow;
            offset = stash.racerSet.ColumnOffset;
            hidden = stash.racerHidden.ColumnOffset;

            foreach (Racer racer in racersList)
            {
                foreach (columnDefs def in stash.racerSet.columnList)
                {
                    _assignCellValue(
                        _worksheet.Cells[reportHelper.cell(def.pos + offset, line)]
                        , def,
                        _columnToRacerValue(def.id, racer));
                }

                foreach (columnDefs def in stash.racerHidden.columnList)
                {
                    _assignCellValue(
                        _worksheet.Cells[reportHelper.cell(def.pos + hidden, line)]
                        , def,
                        _columnToRacerValue(def.id, racer));
                }

                line++;
            }
            stash.racerSet.DetailsCount = racersList.Count;

            //columnDefSet classSet = DefinedSetsOfColumnDefs.ClassMaster();
            //columnDefSet classHidden = DefinedSetsOfColumnDefs.ClassHidden();
            IEnumerable<RaceClass> raceClass = _unitofwork.RaceClass.GetAll();
            List<RaceClass> classList = raceClass.ToList();

            line = stash.classSet.DetailRow;
            offset = stash.classSet.ColumnOffset;
            hidden = stash.classHidden.ColumnOffset;

            foreach (RaceClass clas in classList)
            {
                foreach (columnDefs def in stash.classSet.columnList)
                {
                    _assignCellValue(
                       _worksheet.Cells[reportHelper.cell(def.pos + offset, line)],
                       def, 
                       _columnToClassValue(def.id, clas));
                }

                //int len = classHidden.columnList.Count;
                foreach (columnDefs def in stash.classHidden.columnList)
                {
                    _assignCellValue(
                        _worksheet.Cells[reportHelper.cell(def.pos + hidden, line)]
                        , def,
                        _columnToClassValue(def.id, clas));
                }

                line++;
            }
            stash.classSet.DetailsCount = classList.Count;

            //columnDefSet teamSet = DefinedSetsOfColumnDefs.TeamMaster();
            //columnDefSet teamHidden = DefinedSetsOfColumnDefs.TeamHidden();
            IEnumerable<Team> teams = _unitofwork.Teams.GetAll();
            List<Team> teamList = teams.ToList();

            line = stash.teamSet.DetailRow;
            offset = stash.teamSet.ColumnOffset;
            hidden = stash.teamHidden.ColumnOffset;

            foreach (Team team in teamList)
            {
                foreach (columnDefs def in stash.teamSet.columnList)
                {
                    _assignCellValue(
                        _worksheet.Cells[reportHelper.cell(def.pos + offset, line)],
                       def,
                       _columnToTeamValue(def.id, team));
                }

                //int len = teamHidden.columnList.Count;
                foreach (columnDefs def in stash.teamHidden.columnList)
                {
                    _assignCellValue(
                        _worksheet.Cells[reportHelper.cell(def.pos + hidden, line)]
                        , def,
                        _columnToTeamValue(def.id, team));
                }

                line++;
            }
            stash.teamSet.DetailsCount = teamList.Count;
            return true;
        }


        private bool VerticalParameters() {
            var wsht = _worksheet;
            var r = wsht.Cells["A1"];

            int i = 0;
            int max = stash.racerSet.DetailsCount + 150;
            //int col = 1;

            int line = stash.racerSet.DetailRow;
            int offset = 0;
            int hidden = 0;
            int col = 0;

            //columnDefSet racerHidden = DefinedSetsOfColumnDefs.RaceHidden();

            //int len = stash.racerHidden.columnList.Count();
            offset = stash.racerSet.ColumnOffset -1;
            hidden = stash.racerHidden.ColumnOffset;
            col = stash.racerHidden.ColumnOffset + stash.racerHidden.columnList.Count +4;
            foreach (columnDefs def in stash.racerHidden.columnList)
            {
                if (!(def.id == "id" || def.id == "lastmodified" || def.id == "number"))
                {
                    for (int j = 0; j < max; j++)
                    {
                        _assignCellValue(
                            _worksheet.Cells[reportHelper.cell(def.pos + col, line + j)]
                            , def,
                            string.Format("=IF({0}={1},0,1)",
                            reportHelper.cell(def.pos + offset, line + j),
                            reportHelper.cell(def.pos + hidden, line + j)));
                    }
                }
                i++;
            }

            offset = stash.classSet.ColumnOffset -1;
            hidden = stash.classHidden.ColumnOffset;
            col = stash.classHidden.ColumnOffset + stash.classHidden.columnList.Count +4;
            foreach (columnDefs def in stash.classHidden.columnList)
            {
                if (!(def.id == "lastmodified" || def.id == "id"))
                {
                    for (int j = 0; j < max; j++)
                    {
                        _assignCellValue(
                            _worksheet.Cells[reportHelper.cell(def.pos + col, line + j)]
                            , def,
                            string.Format("=IF({0}={1},0,1)",
                            reportHelper.cell(def.pos + offset, line + j),
                            reportHelper.cell(def.pos + hidden, line + j)));
                    }
                }
                i++;
            }

            offset = stash.teamSet.ColumnOffset - 1;
            hidden = stash.teamHidden.ColumnOffset;
            col = stash.teamHidden.ColumnOffset + stash.teamHidden.columnList.Count + 4;
            foreach (columnDefs def in stash.teamHidden.columnList)
            {
                if (!(def.id == "id" || def.id == "lastmodified" || def.id == "id"))
                {
                    for (int j = 0; j < max; j++)
                    {
                        _assignCellValue(
                            _worksheet.Cells[reportHelper.cell(def.pos + col, line + j)]
                            , def,
                            string.Format("=IF({0}={1},0,1)",
                            reportHelper.cell(def.pos + offset, line + j),
                            reportHelper.cell(def.pos + hidden, line + j)));
                    }
                }
                i++;
            }

            /// ----------------
            //int col = 0;
            i = stash.racerSet.DetailRow;
            int len = 0;
            for (int j = 0; j < max; j++)
            {
                offset = stash.racerSet.ColumnOffset;
                hidden = stash.racerHidden.ColumnOffset;
                // Count
                col = stash.racerSet.ColumnOffset + 0;
                if (j + i < (stash.racerSet.DetailRow + stash.racerSet.DetailsCount ))
                {
                    r = wsht.Cells[reportHelper.cell(col, j + i)];
                    r.Formula = "'" + (j + 1).ToString();
                }

                // Mark
                len = stash.racerHidden.columnList.Count;
                col = stash.racerSet.ColumnOffset + 1; 
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("=IF(SUM({0}:{1})>0,\"X\",\"\")",
                    reportHelper.cell(hidden + len +4 , j + i),
                    reportHelper.cell(hidden + len + 4 + len, j + i));
                r.Font.Color = System.Drawing.Color.Red;

                // Number
                col = stash.racerSet.ColumnOffset + 2;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("= IF(VALUE({0}) >0, {0}, IF({1}=\"\", \"\", TEXT({2} + 1,\"@\")))",
                    reportHelper.cell(stash.racerHidden.ColumnOffset +3, j + i),
                    reportHelper.cell(stash.racerSet.ColumnOffset + 1, j + i),
                    reportHelper.cell(stash.racerSet.ColumnOffset + 2, j + i - 1));

                // Class Description
                col = 9;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("= IF(LEN({0}) = 0, \"\", VLOOKUP({0}, {1}:{2}, 2, FALSE))",
                    reportHelper.cell(col - 3, j + i),
                    reportHelper.cell(stash.classSet.ColumnOffset + 2, stash.racerSet.DetailRow),
                    reportHelper.cell(stash.classSet.ColumnOffset + 3, stash.racerSet.DetailRow + max));

                // Team Description
                col = 10;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("= IF(LEN({0}) = 0, \"\", VLOOKUP({0}, {1}:{2}, 2, FALSE))",
                    reportHelper.cell(col - 3, j + i),
                    reportHelper.cell(stash.teamSet.ColumnOffset + 2, stash.racerSet.DetailRow),
                    reportHelper.cell(stash.teamSet.ColumnOffset + 3, stash.racerSet.DetailRow + max));                //= IF(LEN(E7) = 0, "", VLOOKUP(E7, KN7: KO107, 2, FALSE))


                //------------------------
                offset = stash.classSet.ColumnOffset;
                hidden = stash.classHidden.ColumnOffset;
                // Count
                col = stash.classSet.ColumnOffset + 0;
                if (j + i < (stash.classSet.DetailRow + stash.classSet.DetailsCount))
                {
                    r = wsht.Cells[reportHelper.cell(col, j + i)];
                    r.Formula = "'" + (j + 1).ToString();
                }

                // Mark
                len = stash.classHidden.columnList.Count;
                col = stash.classSet.ColumnOffset + 1;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("=IF(SUM({0}:{1})>0,\"X\",\"\")",
                    reportHelper.cell(hidden + len + 4, j + i),
                    reportHelper.cell(hidden + len + 4 + len, j + i));
                r.Font.Color = System.Drawing.Color.Red;


                //------------------
                offset = stash.teamSet.ColumnOffset;
                hidden = stash.teamHidden.ColumnOffset;
                // Count
                col = stash.teamSet.ColumnOffset + 0;
                if (j + i < (stash.teamSet.DetailRow + stash.teamSet.DetailsCount))
                {
                    r = wsht.Cells[reportHelper.cell(col, j + i)];
                    r.Formula = "'" + (j + 1).ToString();
                }

                // Mark
                len = stash.teamHidden.columnList.Count;
                col = stash.teamSet.ColumnOffset + 1;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("=IF(SUM({0}:{1})>0,\"X\",\"\")",
                    reportHelper.cell(hidden + len + 4, j + i),
                    reportHelper.cell(hidden + len + 4 + len, j + i));
                r.Font.Color = System.Drawing.Color.Red;
            }

            string rs = reportHelper.range(stash.racerSet.ColumnOffset + 3, i, 4, max);
            r = wsht.Range[rs];
            r.Locked = false;

            rs = reportHelper.range(stash.classSet.ColumnOffset + 2, i, 1, max);
            r = wsht.Range[rs];
            r.Locked = false;

            rs = reportHelper.range(stash.teamSet.ColumnOffset + 2, i, 1, max);
            r = wsht.Range[rs];
            r.Font.Bold = true;
            r.Locked = false;

            return true;
        }


        //List<columnDefs> _racerColumns() {
        //    List<columnDefs> returnList = new List<columnDefs> {
        //    new columnDefs("count", "Count", 2, 0, "", false),
        //    new columnDefs("state", "Mark", 2, 0, "", false),
        //    new columnDefs("number", "Number",4, 0, "", false),
        //    new columnDefs("name", "Name", 12, 0, "", false),
        //    new columnDefs("cardetails", "Car Details", 12, 0, "", false),
        //    new columnDefs("class", "Class", 2, 0, "", false),
        //    new columnDefs("team", "Team Code", 4, 0, "", false),
        //    new columnDefs("notes", "Notes", 12, 0, "", false),
        //    new columnDefs("classname", "Classification", 12, 0, "", false),
        //    new columnDefs("teamname", "Team Name", 12, 0, "", false)
        //    };
        //    int i = 0;
        //    foreach(columnDefs def in returnList) { 
        //        def.pos = ++i;
        //    }
        //    return returnList;
        //}

        //List<columnDefs> _classColumns()
        //{
        //    List<columnDefs> returnList = new List<columnDefs> {
        //    new columnDefs("count", "Count", 2, 0, "", false),
        //    new columnDefs("state", "Mark", 2, 0, "", false),
        //    new columnDefs("code", "Code",4, 0, "", false),
        //    new columnDefs("name", "Classification",12, 0, "", false)
        //    };
        //    int i = 0;
        //    foreach (columnDefs def in returnList)
        //    {
        //        def.pos = ++i;
        //    }
        //    return returnList;
        //}

        //List<columnDefs> _teamColumns()
        //{
        //    List<columnDefs> returnList = new List<columnDefs> {
        //    new columnDefs("count", "Count", 2, 0, "", false),
        //    new columnDefs("state", "Mark", 2, 0, "", false),
        //    new columnDefs("code", "Code",4, 0, "", false),
        //    new columnDefs("name", "Team Name",12, 0, "", false)
        //    };
        //    int i = 0;
        //    foreach (columnDefs def in returnList)
        //    {
        //        def.pos = ++i;
        //    }
        //    return returnList;
        //}


        //List<columnDefs> _hiddenRacerColumns() {
        //    List<columnDefs> returnList =
        //        new List<columnDefs> {
        //        new columnDefs("id", "Id", 1, 0, "", false),
        //        new columnDefs("lastmodified", "Last Modified", 1, 0, "", false),
        //        new columnDefs("number", "Number", 1, 0, "", false),
        //        new columnDefs("name", "Name", 1, 0, "", false),
        //        new columnDefs("cardetails", "Car Details", 1, 0, "", false),
        //        new columnDefs("class", "Class", 1, 0, "", false),
        //        new columnDefs("team", "Team", 1, 0, "", false),
        //        new columnDefs("notes", "Notes", 1, 0, "", false)
        //    };

        //    int i = 0;
        //    foreach (columnDefs def in returnList)
        //    {
        //        def.pos = ++i;
        //    }
        //    return returnList;
        //}
        //List<columnDefs> _hiddenClassColumns()
        //{
        //    List<columnDefs> returnList =
        //        new List<columnDefs> {
        //        new columnDefs("id", "Id", 1, 0, "", false),
        //        new columnDefs("lastmodified", "Last Modified", 1, 0, "", false),
        //        new columnDefs("code", "Code",1, 0, "", false),
        //        new columnDefs("name", "Classification",1, 0, "", false)
        //    };

        //    int i = 0;
        //    foreach (columnDefs def in returnList)
        //    {
        //        def.pos = ++i;
        //    }
        //    return returnList;
        //}
        //List<columnDefs> _hiddenTeamColumns()
        //{
        //    List<columnDefs> returnList =
        //        new List<columnDefs> {
        //        new columnDefs("id", "Id", 1, 0, "", false),
        //        new columnDefs("lastmodified", "Last Modified", 1, 0, "", false),
        //        new columnDefs("code", "Code",1, 0, "", false),
        //        new columnDefs("name", "Team Name",1, 0, "", false)
        //    };

        //    int i = 0;
        //    foreach (columnDefs def in returnList)
        //    {
        //        def.pos = ++i;
        //    }
        //    return returnList;
        //}

        private void _assignCellValue(IRange r, columnDefs def, string val)
        {
            r.Formula = val;

            r.ColumnWidth = def.width * 1.85;
            r.HorizontalAlignment = def.align == 0 ? SpreadsheetGear.HAlign.Left : def.align == 1 ? SpreadsheetGear.HAlign.Center : SpreadsheetGear.HAlign.Right;
            if (def.numberFormat != "")
                r.NumberFormat = def.numberFormat;
        }

        private string _columnToRacerValue(string id, Racer p)
        {
            string val = "";
            switch (id)
            {
                case "count":
                    val = "";
                    break;
                case "state":
                    val = "";
                    break;
                case "number":
                    //val = "'" + p.DateSold.ToString(@"MM/dd/yyyy HH:mm");
                    val = p.Number.ToString();
                    break;
                case "name":
                    val = p.Name.ToString();
                    break;
                case "cardetails":
                    val = p.CarDetails.ToString();
                    break;
                case "team":
                    if (p.TeamIdOptional == null)
                        val = "";
                    else
                        val = p.TeamIdOptional;
                    break;
                case "notes":
                    val = "'";
                    break;
                case "class":
                    if (p.RaceClass == null)
                        val = "";
                    else val = p.RaceClass;
                    val = p.RaceClass;
                    break;
                case "classname":
                    val = "";
                    break;
                case "teamname":
                    val = "";
                    break;
                case "id":
                    val = p.Id.ToString();
                    break;
                case "lastmodified":
                    if (p.dtlastmodified == null)
                        val = "";
                    else
                        val = "'" + p.dtlastmodified.ToString(@"MM/dd/yyyy HH:mm:ss");
                    break;
            }
            return val;
        }

        private string _columnToClassValue(string id, RaceClass c)
        {
            string val = "";
            switch (id)
            {
                case "count":
                    val = "";
                    break;
                case "state":
                    val = "";
                    break;
                case "code":
                    //val = "'" + p.DateSold.ToString(@"MM/dd/yyyy HH:mm");
                    val = c.Code.ToString();
                    break;
                case "name":
                    val = c.Description.ToString();
                    break;
                
                case "id":
                    val = c.Id.ToString();
                    break;
                case "lastmodified":
                    if (c.dtlastmodified == null)
                        val = "";
                    else
                        val = "'" + c.dtlastmodified.ToString(@"MM/dd/yyyy HH:mm:ss");
                    break;
            }
            return val;
        }


        private string _columnToTeamValue(string id, Team t)
        {
            string val = "";
            switch (id)
            {
                case "count":
                    val = "";
                    break;
                case "state":
                    val = "";
                    break;
                case "code":
                    //val = "'" + p.DateSold.ToString(@"MM/dd/yyyy HH:mm");
                    val = t.Name.ToString();
                    break;
                case "name":
                    val = t.Description.ToString();
                    break;

                case "id":
                    val = t.Id.ToString();
                    break;
                case "lastmodified":
                    if (t.dtlastmodified == null)
                        val = "";
                    else
                        val = "'" + t.dtlastmodified.ToString(@"MM/dd/yyyy HH:mm:ss");
                    break;
            }
            return val;
        }

    }
}
