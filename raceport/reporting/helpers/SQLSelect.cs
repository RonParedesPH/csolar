using System.Collections.Generic;

namespace reporting.helpers
{

    public class SQLSelect
    {


        private List<string> Joins { get; set; } = new List<string>();
        public string Fields { get; set; } = string.Empty;
        public string Table { get; set; } = string.Empty;
        public string Where { get; set; } = string.Empty;

        //public SQLSelect(string? fields, string? table, string? where)
        //{
        //    if (fields is not null) this.Fields = fields;
        //    if (table is not null) this.Table = table;
        //    if (where is not null) this.Where = where;
        //}
        public string Join(string join, string table, string on)
        {
            string ret = join + " " + table + " ON " + on;
            Joins.Add(ret);
            return ret;
        }

        public string CommandText(string fields = null, string table = null, string where = null)
        {
            if (fields != null) this.Fields = fields;
            if (table != null) this.Table = table;
            if (where != null) this.Where = where;
            string joins = string.Empty;
            Joins.ForEach(c => {
                joins += c + " ";
            });

            return "SELECT " + this.Fields + " FROM " + this.Table + " " + joins +
                (this.Where != string.Empty ? " WHERE " + this.Where : string.Empty);

        }
    }
}
