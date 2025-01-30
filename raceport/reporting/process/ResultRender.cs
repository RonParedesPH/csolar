using reporting.entities;
using reporting.unitofwork;
using SpreadsheetGear;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace reporting.process
{
    public class ResultRender : IWorksheetWorker
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

        private StashBag _stash;
        public ResultRender(IWorksheet worksheet, IUnitOfWork unitofwork) : base(worksheet, unitofwork)
        {
            _unitofwork = unitofwork;
            _worksheet = worksheet;
        }

        public override bool Render(string appName, string title, string subTitle, string key, string tabName)
        {
            _stash = new StashBag()
            {
                appName = appName,
                title = title,
                subTitle = subTitle,
                headerLine = 10,
                detailLineStart = 12
            };

            bool result = PrepareWorksheet(key);
            result = PrintContent(key);
            result = VerticalParameters(tabName);
            return result;
        }

        bool _printHeaderLines(List<columnDefs> defsList, int offset, int line)
        {
            IRange r = _worksheet.Cells["A1"];
            foreach (columnDefs def in defsList)
            {
                r = _worksheet.Cells[reportHelper.cell(def.pos + offset, line)];
                r.Formula = def.title;
                r.ColumnWidth = def.width * 1.85;
                r.HorizontalAlignment = SpreadsheetGear.HAlign.Center;
                reportHelper.setBorders(
                    _worksheet.Cells[reportHelper.range(def.pos + offset, line, 2, 1)],
                    SpreadsheetGear.BordersIndex.EdgeBottom,
                    System.Drawing.Color.Black, SpreadsheetGear.LineStyle.Continous,
                    SpreadsheetGear.BorderWeight.Medium);

            }
            return true;
        }

        public bool PrepareWorksheet(string key)
        {


            var wsht = _worksheet;
            var cells = wsht.Cells;
            var r = cells["A:MZ"];
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
            r.Formula = _stash.appName;

            r = cells["A3"];
            r.RowHeight = 16.0;
            r.Font.Size = 14.0;
            r.Formula = _stash.title;

            r = cells["A4"];
            r.RowHeight = 13.0;
            cells["A4"].Font.Size = 11;
            r.Formula = _stash.subTitle;

            //int y = 6; int i = 1;
            int line = _stash.headerLine;

            Round round = _unitofwork.Rounds.Find(key);
            if (round != null) {
                r = cells["D6"]; r.Formula = "Round";
                r = cells["D7"]; r.Formula = "Date";
                r = cells["D8"]; r.Formula = "Track";

                r = cells["E6"]; r.Formula = round.Name;
                r = cells["E7"]; r.Formula = round.EventDate.ToLongDateString();
                r = cells["E8"]; r.Formula = round.Venue;

            }


            List<columnDefs> defsList = DefinedSetsOfColumnDefs.ResultSet().columnList;
            int offset = 0;

            _printHeaderLines(defsList, offset, line);

            return true;
        }


        public bool PrintContent(string key)
        {

            var r = _worksheet.Cells["A1"];
            int offset = 0;
            int line = _stash.detailLineStart;

            _stash.racerHiddenOffset = offset + 100;

            List<columnDefs> defsList = DefinedSetsOfColumnDefs.ResultSet().columnList;
            //-------------
            //List<Registration> racersList = _unitofwork.Registrations.GetItemsByRoundId(key).ToList();
            ////List<Registration> racersList = registrations.Where(c=> c.RoundId == key).OrderBy(c => c.RacerId).ToList();
            //foreach (Registration regis in racersList)
            //{
            //    foreach (columnDefs def in defsList)
            //    {
            //        _assignCellValue(
            //            _worksheet.Cells[reportHelper.cell(def.pos + _stash.racerOffset, line)]
            //            , def,
            //            _columnToRacerValue(def.id, regis));
            //    }

            //    int len = _hiddenRacerColumns().Count;
            //    foreach (columnDefs def in _hiddenRacerColumns())
            //    {
            //        _assignCellValue(
            //            _worksheet.Cells[reportHelper.cell(def.pos + _stash.racerHiddenOffset, line)]
            //            , def,
            //            _columnToRacerValue(def.id, regis));
            //        //if (!(def.id == "id" || def.id == "lastmodified" || def.id == "number"))
            //        //{
            //        //    _assignCellValue(
            //        //    _worksheet.Cells[reportHelper.cell(def.pos + offset + 100 + len, line)]
            //        //    , def,
            //        //    string.Format("=IF({0}={1},0,1)",
            //        //    reportHelper.cell(def.pos + offset, line),
            //        //    reportHelper.cell(def.pos + offset + 100, line)));
            //        //}
            //    }

            //    //r = _worksheet.Cells[reportHelper.cell(0 + offset + 2, line)];
            //    //r.Formula = string.Format("=IF(SUM({0}:{1})=0,\"\",\"X\")",
            //    //    reportHelper.cell(offset + 100 + len + (3), line),
            //    //    reportHelper.cell(offset + 100 + len + len, line));
            //    //r.Font.Color = System.Drawing.Color.Red;

            //    line++;
            //}
            //_stash.details = racersList.Count;
            //_stash.racerDetailCount = racersList.Count;
            //-----------------

            
            return true;
        }


        private bool VerticalParameters(string key)
        {
            var wsht = _worksheet;
            var r = wsht.Cells["A1"];

            int i = _stash.detailLineStart;
            int max = _stash.details + 50;
            int col = 1;

            int offset = 0;
            int line = _stash.detailLineStart;

            int len = DefinedSetsOfColumnDefs.ResultHidden().columnList.Count();
            offset = _stash.racerHiddenOffset + len + 1;
            foreach (columnDefs def in DefinedSetsOfColumnDefs.ResultHidden().columnList)
            {
                if (!(def.id == "id" || def.id == "lastmodified"))
                {
                    for (int j = 0; j < max; j++)
                    {
                        _assignCellValue(
                            _worksheet.Cells[reportHelper.cell(def.pos + offset, line + j)]
                            , def,
                            string.Format("=IF({0}={1},0,1)",
                            reportHelper.cell(def.pos + _stash.racerOffset, line + j),
                            reportHelper.cell(def.pos + _stash.racerHiddenOffset, line + j)));
                    }
                }
            }
            line = _stash.detailLineStart;



            for (int j = 0; j < max; j++)
            {
                // Count
                col = 1;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("=IF(LEN({0})=0,\"\",{1}+1)",
                    reportHelper.cell(col + 2, j + i),
                    reportHelper.cell(col, j + i -1));

                // Mark
                col = 2;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("=IF(SUM({0}:{1})>0,\"X\",\"\")",
                    reportHelper.cell(_stash.racerHiddenOffset + (8), j + i),
                    reportHelper.cell(_stash.racerHiddenOffset + (8) + (2), j + i));
                r.Font.Color = System.Drawing.Color.Red;

                //    // Number
                //    col = 3;
                //    r = wsht.Cells[reportHelper.cell(col, j + i)];
                //    r.Formula = string.Format("= IF(VALUE({0}) >0, {0}, IF({1}=\"\", \"\", TEXT({2} + 1,\"@\")))",
                //        reportHelper.cell(100 + (3), j + i),
                //        reportHelper.cell(col - 1, j + i),
                //        reportHelper.cell(col, j + i - 1));

                // Class Name 
                col = 5 ;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("= IF(LEN({0}) = 0, \"\", VLOOKUP({0}, MasterList!S$8:T$2008, 2, FALSE))",
                    reportHelper.cell(col - 1, j + i)
                    );
                //= IF(LEN(E7) = 0, "", VLOOKUP(E7, KN7: KO107, 2, FALSE))

                // Driver 
                col = 6;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("=IF(AND(LEN(C{0})>0,LEN(D{0})>0),VLOOKUP(D{0}&C{0},'{1}'!CQ$12:CR$2012,2,FALSE),\"\")",
                    (j + i).ToString(), key
                    );
                //= IF(LEN(E7) = 0, "", VLOOKUP(E7, KN7: KO107, 2, FALSE))

                // Final time
                col = 10;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("=IF(LEN(G{0})=0,\"\", IF(ISNUMBER(G{0}),G{0}+(IF(LEN(H{0})>0,H{0}*2))+(IF(LEN(I{0})>0,I{0}*4)),G{0}))",
                    (j + i).ToString()
                    );

                // Car 
                col = 11;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("= IF(LEN({0}) = 0, \"\", VLOOKUP({0}, MasterList!C$8:F$2008, 3, FALSE))",
                    reportHelper.cell(col - 8, j + i)
                    );

                // Team Name
                col = 12;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("= IF(LEN({0}) = 0, \"\", VLOOKUP({0}, MasterList!C$8:J$2008, 8, FALSE))",
                    reportHelper.cell(col - 9, j + i)
                    );

                // Driver class
                col = 13;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("= IF(LEN({0}) = 0, \"\", VLOOKUP({0}, MasterList!C$8:J$2008, 7, FALSE))",
                    reportHelper.cell(col - 9, j + i)
                    );


                col = _stash.racerHiddenOffset -5;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("={0}&{1}",
                    reportHelper.cell(_stash.racerOffset +3, j + i),
                    reportHelper.cell(_stash.racerOffset +4, j + i));

                col = _stash.racerHiddenOffset -4;
                r = wsht.Cells[reportHelper.cell(col, j + i)];
                r.Formula = string.Format("={0}",
                    reportHelper.cell(_stash.racerOffset + 5, j + i));
            }

            string rs = reportHelper.cell(_stash.racerOffset + 3, i) + ":" +
                reportHelper.cell(_stash.racerOffset + 4, i + max);
            r = wsht.Range[rs];
            r.Font.Bold = true;
            r.Locked = false;

            rs = reportHelper.cell(_stash.racerOffset + 7, i) + ":" +
            reportHelper.cell(_stash.racerOffset + 9, i + max);
            r = wsht.Range[rs];
            r.Font.Bold = true;
            r.Locked = false;

            return true;
        }


        //List<columnDefs> _resultColumns()
        //{
        //    List<columnDefs> returnList = new List<columnDefs> {
        //    new columnDefs("count", "Count", 2, 0, "", false),
        //    new columnDefs("state", "Mark", 2, 0, "", false),
        //    new columnDefs("number", "Number",4, 0, "", false),
        //    new columnDefs("class", "Class", 4, 0, "", false),
        //    new columnDefs("classname", "Class Name", 12, 0, "", false),
        //    new columnDefs("name", "Driver Name", 12, 0, "", false),
        //    new columnDefs("time", "Time", 8, 0, "", false),
        //    new columnDefs("pylon", "Pylon", 8, 0, "", false),
        //    new columnDefs("stopbox", "Stop Box", 8, 0, "", false),
        //    new columnDefs("finaltime", "Final Time", 8, 0, "", false),
        //    new columnDefs("car", "Car", 12, 0, "", false),
        //    new columnDefs("team", "Team Code", 4, 0, "", false),
        //    new columnDefs("driverclass", "Driver Class", 4, 0, "", false),
        //    };
        //    int i = 0;
        //    foreach (columnDefs def in returnList)
        //    {
        //        def.pos = ++i;
        //    }
        //    return returnList;
        //}

        //List<columnDefs> _hiddenResultColumns()
        //{
        //    List<columnDefs> returnList =
        //        new List<columnDefs> {
        //        new columnDefs("id", "Id", 1, 0, "", false),
        //        new columnDefs("lastmodified", "Last Modified", 1, 0, "", false),
        //        new columnDefs("number", "Number", 1, 0, "", false),
        //        new columnDefs("class", "Class", 1, 0, "", false)
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

        private string _columnToRacerValue(string id, Registration p)
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
                    val = p.Participant.Number.ToString();
                    break;
                case "name":
                    val = p.Participant.Name;
                    break;
                case "cardetails":
                    val = p.Participant.CarDetails;
                    break;
                case "team":
                    //if (p.TeamIdOptional == null)
                    //    val = "";
                    //else
                    //    val = p.TeamIdOptional;
                    val = "";
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
