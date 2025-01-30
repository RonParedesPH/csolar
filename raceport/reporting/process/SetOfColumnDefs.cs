using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reporting.process
{

    internal static class DefinedSetsOfColumnDefs {
        public static columnDefSet RaceSet()
        {
            List<columnDefs> defList = new List<columnDefs> {
            new columnDefs("count", "Count", 2, 0, "", false),
            new columnDefs("state", "Mark", 2, 0, "", false),
            new columnDefs("number", "Number",4, 0, "", false),
            new columnDefs("name", "Name", 16, 0, "", false),
            new columnDefs("cardetails", "Car Details", 16, 0, "", false),
            new columnDefs("class", "Class", 2, 0, "", false),
            new columnDefs("team", "Team Code", 4, 0, "", false),
            new columnDefs("notes", "Notes", 16, 0, "", false),
            new columnDefs("classname", "Classification", 16, 0, "", false),
            new columnDefs("teamname", "Team Name", 16, 0, "", false)
            };
            int i = 0;
            foreach (columnDefs def in defList)
            {
                def.pos = i++;
            }
            return new columnDefSet { 
                Name = "Race Drivers",
                HeaderRow = 7, 
                DetailRow = 8, 
                ColumnOffset = 1,
                columnList = defList};
        }

        public static columnDefSet ClassSet() { 
            List<columnDefs> defList = new List<columnDefs> {
            new columnDefs("count", "Count", 2, 0, "", false),
            new columnDefs("state", "Mark", 2, 0, "", false),
            new columnDefs("code", "Code",4, 0, "", false),
            new columnDefs("name", "Classification",12, 0, "", false)
            };
            int i = 0;
            foreach (columnDefs def in defList)
            {
                def.pos = i++;
            }
            return new columnDefSet {
                Name = "Race Classifications",
                HeaderRow = 7,
                DetailRow = 8,
                ColumnOffset = 17,
                columnList = defList };
        }
        public static columnDefSet TeamSet()
        {

            List<columnDefs> defList = new List<columnDefs> {
            new columnDefs("count", "Count", 2, 0, "", false),
            new columnDefs("state", "Mark", 2, 0, "", false),
            new columnDefs("code", "Code",4, 0, "", false),
            new columnDefs("name", "Team Name",12, 0, "", false)
            };
            int i = 0;
            foreach (columnDefs def in defList)
            {
                def.pos = i++;
            }
            return new columnDefSet
            {
                Name = "Teams List",
                HeaderRow = 7,
                DetailRow = 8,
                ColumnOffset = 26,
                columnList = defList
            };
        }

        public static columnDefSet RaceHidden()
        {
            List<columnDefs> defList =
                new List<columnDefs> {
                new columnDefs("id", "Id", 1, 0, "", false),
                new columnDefs("lastmodified", "Last Modified", 1, 0, "", false),
                new columnDefs("number", "Number", 1, 0, "", false),
                new columnDefs("name", "Name", 1, 0, "", false),
                new columnDefs("cardetails", "Car Details", 1, 0, "", false),
                new columnDefs("class", "Class", 1, 0, "", false),
                new columnDefs("team", "Team", 1, 0, "", false),
                new columnDefs("notes", "Notes", 1, 0, "", false)
            };

            int i = 0;
            foreach (columnDefs def in defList)
            {
                def.pos = ++i;
            }
            return new columnDefSet
            {
                HeaderRow = 7,
                DetailRow = 8,
                ColumnOffset = 100,
                columnList = defList
            };
        }
        public static columnDefSet ClassHidden()
        {
            List<columnDefs> defList =
                new List<columnDefs> {
                new columnDefs("id", "Id", 1, 0, "", false),
                new columnDefs("lastmodified", "Last Modified", 1, 0, "", false),
                new columnDefs("code", "Code",1, 0, "", false),
                new columnDefs("name", "Classification",1, 0, "", false)
            };

            int i = 0;
            foreach (columnDefs def in defList)
            {
                def.pos = ++i;
            }
            return new columnDefSet
            {
                HeaderRow = 7,
                DetailRow = 8,
                ColumnOffset = 128,
                columnList = defList
            };
        }
        public static columnDefSet TeamHidden()
        {
            List<columnDefs> defList =
                new List<columnDefs> {
                new columnDefs("id", "Id", 1, 0, "", false),
                new columnDefs("lastmodified", "Last Modified", 1, 0, "", false),
                new columnDefs("code", "Code",1, 0, "", false),
                new columnDefs("name", "Team Name",1, 0, "", false)
            };

            int i = 0;
            foreach (columnDefs def in defList)
            {
                def.pos = ++i;
            }
            return new columnDefSet
            {
                HeaderRow = 7,
                DetailRow = 8,
                ColumnOffset = 144,
                columnList = defList
            };
        }



        public static columnDefSet RoundSet()
        {
            List<columnDefs> defList = new List<columnDefs> {
            new columnDefs("count", "Count", 2, 0, "", false),
            new columnDefs("state", "Mark", 2, 0, "", false),
            new columnDefs("number", "Number",4, 0, "", false),
            new columnDefs("class", "Class", 4, 0, "", false),
            new columnDefs("name", "Name", 12, 0, "", false),
            new columnDefs("classname", "Race Classification", 12, 0, "", false),
            new columnDefs("car", "Car", 12, 0, "", false),
            new columnDefs("team", "Team Code", 4, 0, "", false),
            new columnDefs("driverclass", "Driver Class", 4, 0, "", false),
            };
            int i = 0;
            foreach (columnDefs def in defList)
            {
                def.pos = ++i;
            }
            return new columnDefSet
            {
                Name = "Round Registrations",
                HeaderRow = 7,
                DetailRow = 8,
                ColumnOffset = 1,
                columnList = defList
            };
        }


        public static columnDefSet RoundHidden()
        {
            List<columnDefs> defList =
                new List<columnDefs> {
                new columnDefs("id", "Id", 1, 0, "", false),
                new columnDefs("lastmodified", "Last Modified", 1, 0, "", false),
                new columnDefs("number", "Number", 1, 0, "", false),
                new columnDefs("class", "Class", 1, 0, "", false)
            };

            int i = 0;
            foreach (columnDefs def in defList)
            {
                def.pos = ++i;
            }
            return new columnDefSet
            {
                HeaderRow = 7,
                DetailRow = 8,
                ColumnOffset = 100,
                columnList = defList
            };
        }

        public static columnDefSet ResultSet()
        {
            List<columnDefs> defList = new List<columnDefs> {
            new columnDefs("count", "Count", 2, 0, "", false),
            new columnDefs("state", "Mark", 2, 0, "", false),
            new columnDefs("number", "Number",4, 0, "", false),
            new columnDefs("class", "Class", 4, 0, "", false),
            new columnDefs("classname", "Class Name", 12, 0, "", false),
            new columnDefs("name", "Driver Name", 12, 0, "", false),
            new columnDefs("time", "Time", 8, 0, "", false),
            new columnDefs("pylon", "Pylon", 8, 0, "", false),
            new columnDefs("stopbox", "Stop Box", 8, 0, "", false),
            new columnDefs("finaltime", "Final Time", 8, 0, "", false),
            new columnDefs("car", "Car", 12, 0, "", false),
            new columnDefs("team", "Team Code", 4, 0, "", false),
            new columnDefs("driverclass", "Driver Class", 4, 0, "", false),
            };
            int i = 0;
            foreach (columnDefs def in defList)
            {
                def.pos = ++i;
            }
            return new columnDefSet
            {
                Name = "Race Round Results",
                HeaderRow = 7,
                DetailRow = 8,
                ColumnOffset = 1,
                columnList = defList
            };
        }

        public static columnDefSet ResultHidden()
        {
            List<columnDefs> defList =
                new List<columnDefs> {
                new columnDefs("id", "Id", 1, 0, "", false),
                new columnDefs("lastmodified", "Last Modified", 1, 0, "", false),
                new columnDefs("number", "Number", 1, 0, "", false),
                new columnDefs("class", "Class", 1, 0, "", false)
            };

            int i = 0;
            foreach (columnDefs def in defList)
            {
                def.pos = ++i;
            }
            return new columnDefSet
            {
                HeaderRow = 7,
                DetailRow = 8,
                ColumnOffset = 1,
                columnList = defList
            };
        }
    }
}