using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinToolsTwo
{
    class Parser
    {

        public static Table LoadTable(TextBox textBoxSource)
        {
            Table table = new Table();
            if (textBoxSource.Text.Length == 0) throw new ApplicationException("There is no table to parse");

            var lines = textBoxSource.Text.Split(Environment.NewLine);
            var tableRows = new List<Row>();
            foreach (var line in lines)
            {
                if (line == "") continue;
                var columns = line.Split("\t");
                var tableColumns = new List<Column>();
                foreach (var column in columns)
                {
                    tableColumns.Add(new Column { Value = column });
                }
                tableRows.Add(new Row { Columns = tableColumns });
            }
            table.Rows = tableRows;
            return table;
        }

        public class Table
        {
            public List<Row> Rows = new List<Row>();

            internal string Get(int row, int col)
            {
                return Rows[row].Columns[col].Value; ;
            }

            public void Set(int row, int col, string value)
            {
                Rows[row].Columns[col].Value = value;
            }

            internal int ColumnAmount()
            {
                return Rows[0].Columns.Count;
            }

            internal int RowAmount()
            {
                return Rows.Count;
            }
        }

        public class Row
        {
            public List<Column> Columns = new List<Column>();
        }

        public class Column
        {
            public string Value;
        }
    }



}
