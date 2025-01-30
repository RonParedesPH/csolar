using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetGear;

namespace reporting.process
{
    internal class rangeHelper
    {
        private IWorksheet _worksheet;
        private int _row = 0, _col = 0;
        private int _columns = 0;
        private int _rows = 0;
        public string Address = string.Empty;

        public rangeHelper(IWorksheet worksheet)
        {
            _worksheet = worksheet;
            _col = 1; _row = 1;
            Address = cell(_col, _row);
        }
        public IRange Locate(int col, int row) { 
            _col = col;
            _row = row;

            Address = cell(_col, _row);
            return _worksheet.Cells[cell(_col, _row)];
        }

        public IRange Locate(int col, int row, int columns, int rows) {
            _col = col;
            _row = row;
            _columns = columns;
            _rows = rows;

            Address = cell(_col, _row);
            return _worksheet.Cells[range(col, row, columns, rows)];
        }

        public IRange Down(int row) {
            _row += row;

            Address = cell(_col, _row);
            return _worksheet.Cells[cell(_col, _row)];
        }

        public IRange ToLeft(int cols)
        { 
            return _worksheet.Cells[cell(_col +cols, _row)];
        }

        public IRange Cell() {
            return _worksheet.Cells[cell(_col, _row)];
        }

        private static string cell(int x, int y)
        {
            string ret = "";

            int z = (x - 1) / 26;
            if (z > 0)
            {
                x = (x - (z * 26));
                if (x == 0)
                    x = 1;

                ret = string.Format("{0}", (Char)(z + 64));
            }

            ret = ret + string.Format("{0}{1}", (Char)(x + 64), y);
            return ret;
        }

        private static string range(int x, int y, int extend_x, int extend_y)
        {
            return string.Format("{0}:{1}", cell(x, y), cell(x + extend_x, y + extend_y));
        }


    }

}
